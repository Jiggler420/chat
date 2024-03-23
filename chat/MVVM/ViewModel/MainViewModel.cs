using client.MVVM.Core;
using client.MVVM.Model;
using client.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace client.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public RelayCommand ConnectToServerC {  get; set; }
        public string username { get; set; }
        
        private Server server;

        public MainViewModel()
        { 
            Users = new ObservableCollection<UserModel>();
            server = new Server();
            server.connected += ServerUser_connected;
            ConnectToServerC = new RelayCommand(o => server.ConnectToServer(username), o => !string.IsNullOrEmpty(username));
        }

        private void ServerUser_connected()
        {
            var user = new UserModel
            {
                username = server.PacketReader.ReadMessage(),
                id = server.PacketReader.ReadMessage()
            };
            if(!Users.Any(x => x.id == user.id))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
  