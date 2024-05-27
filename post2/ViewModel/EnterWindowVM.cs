using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using post2.view;
using post2.model;
using System.Windows.Controls;
using System.IO;
using System.Security.Cryptography;

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
                    CashPassword();
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Show();
                    CloseWindow(enterWindow);
                    Signal();
                }
            });
        }
        internal void CashPassword()
        {
            var bytes = Encoding.ASCII.GetBytes(passwordBox.Password);
            StringBuilder result = new StringBuilder();
            using (var md5 = MD5.Create())
            using (var ms = new MemoryStream(bytes))
            {
                var hash = md5.ComputeHash(ms);
                foreach (var b in hash)
                    result.Append(b.ToString("x2"));
            }
            return /*result.ToString()*/;
        }
        EnterWindow enterWindow;
        private PasswordBox passwordBox;

        internal void SetWindow(EnterWindow enterWindow)
        {
            this.enterWindow = enterWindow;
        }
        internal void CloseWindow(EnterWindow enterWindow)
        {
            this.enterWindow.Close();
        }

        internal void SetPasswordBox(PasswordBox passwordBox)
        {
            this.passwordBox = passwordBox;
        }
    }
}
