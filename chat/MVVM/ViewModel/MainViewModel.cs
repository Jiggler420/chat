using client.MVVM.Core;
using client.MVVM.Model;
using client.Net;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace client.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerC { get; set; }
        public RelayCommand SendMessageC { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }

        private client.Net.Server server;

        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            server = new client.Net.Server();
            server.connected += ServerUser_connected;
            server.msgRecieved += ServerMessageRecieved;
            server.disconnected += ServerDisconnected;
            ConnectToServerC = new RelayCommand(o => ConnectToServer(), o => !string.IsNullOrEmpty(Username));
            SendMessageC = new RelayCommand(o => SendMessageToServer(), o => !string.IsNullOrEmpty(Message));

            Username = string.Empty;
            Message = string.Empty;
        }

        private void ServerUser_connected()
        {
            string username = server.PacketReader.ReadString();
            string id = server.PacketReader.ReadString();
            var user = new UserModel()
            {
                username = username ?? throw new ArgumentNullException(nameof(username)),
                id = id ?? throw new ArgumentNullException(nameof(id))
            };
            if (!Users.Any(x => x.id == user.id))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }

        private void ServerMessageRecieved()
        {
            var msg = server.PacketReader.ReadString();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void ServerDisconnected()
        {
            var id = server.PacketReader.ReadString();
            var user = Users.FirstOrDefault(x => x.id == id);
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void ConnectToServer()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                server.ConnectToServer(Username);
            }
        }

        private void SendMessageToServer()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                server.SendMessageToServer(Message);
            }
        }
    }
}
