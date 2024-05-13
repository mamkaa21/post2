﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using post2.view;
using post2.model;
using System.Windows.Controls;

namespace post2.ViewModel
{
    public class EnterWindowVM : BaseVM
    {
        public string Login { get; set; }
        public CommandVm Enter {  get; }

        public EnterWindowVM()
        {
            var user = UserRepository.Instance.GetUserByLoginPassword(Login, passwordBox.Password);
            if (user.ID != 0)
            {
                ActiveUser.Instance.SetUser(user);
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
            }          
        }

        EnterWindow enterWindow;
        internal void SetWindow(EnterWindow enterWindow)
        {
            this.enterWindow = enterWindow;
        }
    }
}
