﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using post2.view;
using post2.model;
using System.Windows.Controls;
using System.IO;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

namespace post2.ViewModel
{
    public class EnterWindowVM : BaseVM
    {
        public string Login { get; set; }
        public CommandVm Enter { get; }

        public EnterWindowVM()
        {           
              
            Enter = new CommandVm(() =>
            {
                var user = UserRepository.Instance.GetUserByLoginPassword(Login, passwordBox.Password);
                if (user.ID != 0)
                {
                    ActiveUser.Instance.SetUser(user);
                   
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Show();
                    CloseWindow(enterWindow);
                    Signal();
                }
            });
        }     
        EnterWindow enterWindow;
        private PasswordBox passwordBox;

        internal void SetWindow(EnterWindow enterWindow) //привязка окна к вм
        {
            this.enterWindow = enterWindow;
        }
        internal void CloseWindow(EnterWindow enterWindow) //закрытие окна
        {
            this.enterWindow.Close();
        }
        internal void SetPasswordBox(PasswordBox passwordBox) //привязка? PasswordBox к вм
        {
            this.passwordBox = passwordBox;
        }
    }
}
