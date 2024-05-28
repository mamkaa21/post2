using OpenPop.Mime;
using OpenPop.Pop3;
using post2.model;
using post2.view;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Threading;
using MySqlConnector;


namespace post2.ViewModel
{
    public class UserWindowVm : BaseVM
    {
        private ObservableCollection<User> user = new ();
        public CommandVm Back { get; }
        public CommandVm Edit { get; }
        public ObservableCollection<User> Users { get => user; set => user = value; }
        string login;
        string password;
        public UserWindowVm()
        {           
            string sql = "SELECT u.ID, u.NickName, u.Login, u.Image, ab.Email, ab.Title, ab.ID AS idAddress FROM User u, AdressBook ab WHERE ab.ID_User = u.ID";
            //Users = UserRepository.Instance.GetUserByLoginPassword(login, password);        
            Edit = new CommandVm(() =>
            {
                UserEditWindow userEditWindow = new UserEditWindow();
                userEditWindow.Show();
                Signal();
            });
            Back = new CommandVm(() =>
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                CloseWindow(userWindow);
                Signal();
            });
        }
      
        UserWindow userWindow;
        internal void SetWindow(UserWindow userWindow) //привязка окна к вм
        {
            this.userWindow = userWindow;
        }
        internal void CloseWindow(UserWindow userWindow) //закрытие окна
        {
            this.userWindow.Close();
        }      
    }
}
