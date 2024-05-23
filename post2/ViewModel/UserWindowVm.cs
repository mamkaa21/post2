using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using post2.model;
using post2.view;
using OpenPop.Mime;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Channels;


namespace post2.ViewModel
{
    public class UserWindowVm : BaseVM
    {
        public ObservableCollection<User> users = new ();
        public CommandVm Back { get; }
        public CommandVm Edit { get; }
        public ObservableCollection<User> Users { get => users; set => users = value; }
        public UserWindowVm()
        {

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
        //private void GetUserByLoginPassword() { UserRepository.Instance.GetUserByLoginPassword(users,); }
        UserWindow userWindow;
        internal void SetWindow(UserWindow userWindow)
        {
            this.userWindow = userWindow;
        }
        internal void CloseWindow(UserWindow userWindow)
        {
            this.userWindow.Close();
        }      
    }
}
