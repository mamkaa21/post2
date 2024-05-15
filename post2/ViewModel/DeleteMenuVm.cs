using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using post2.view;
using post2.model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using OpenPop.Pop3;
using System.Windows;

namespace post2.ViewModel
{
    public class DeleteMenuVm : BaseVM
    {
        Pop3Client pop3Client;
        private ObservableCollection<Popemail> email;
        public Popemail selectedEmail;
        public CommandVm Back { get; }
        public CommandVm Delete { get; }
        public ObservableCollection<Popemail> Email { get => email; set => email = value; }
        public CommandVm Return { get; }
        public Popemail SelectedEmail
        {
            get => selectedEmail;
            set
            {
                selectedEmail = value;
                Signal();
            }
        }
        public DeleteMenuVm()
        {
            Back = new CommandVm(() =>
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                CloseWindow(deleteMenu);
                Signal();
            }
           );
            Delete = new CommandVm(() =>
            {
                DeletePost();
            });
            Return = new CommandVm(() =>
            { });
        }
        private static Pop3Client ConnectMail()
        {
            Pop3Client pop3Client = new Pop3Client();

            var username = "alina1125@suz-ppk.ru";
            var password = "D35de%TJ";

            pop3Client.Connect("pop3.beget.com", 110, false);
            pop3Client.Authenticate(username, password, AuthenticationMethod.UsernameAndPassword);
            return pop3Client;
        }
        private void DeletePost()
        {
            if (SelectedEmail == null)
            {
                MessageBox.Show("Не выбран обьект");
                return;
            }
            try
            {
                pop3Client = ConnectMail();
                PostRepository.Instance.RemovePOPEmail(selectedEmail);
                pop3Client.DeleteMessage(SelectedEmail.MessageNumber);
                pop3Client.Disconnect();
                var index = SelectedEmail.MessageNumber;
                Email.Remove(selectedEmail);
                var sort = Email.ToArray();
                Array.Sort(sort, (x, y) => y.DateSend.CompareTo(x.DateSend));
                for (int i = 0; i < sort.Length; i++)
                    sort[i].MessageNumber = i + 1;
            }
            catch { }
            //PostRepository.Instance.UpdatePOPEmail(selectedEmail);
        }
        DeleteMenu deleteMenu;
        internal void SetWindow(DeleteMenu deleteMenu)
        {
            this.deleteMenu = deleteMenu;
        }
        internal void CloseWindow(DeleteMenu deleteMenu)
        {
            this.deleteMenu.Close();
        }
    }
}
