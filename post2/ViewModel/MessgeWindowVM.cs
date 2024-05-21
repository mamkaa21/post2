using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using post2.model;
using post2.view;

namespace post2.ViewModel
{
    public class MessgeWindowVM : BaseVM
    {
        private ObservableCollection<Popemail> email = new();
        public Popemail selectedEmail;
        public CommandVm Back { get; }
        public CommandVm Delete { get; }
        public CommandVm SendAnswer { get; }
        public ObservableCollection<Popemail> Email { get => email; set => email = value; }
        public Popemail SelectedEmail
        {
            get => selectedEmail;
            set
            {
                selectedEmail = value;
                Signal();
                //
            }
        }
        public MessgeWindowVM()
        {
            Delete = new CommandVm(() =>
            {
                RemoveMessage();
            });
            Back = new CommandVm(() =>
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                Signal();
            });
            SendAnswer = new CommandVm(() =>
            {
                SendWindow sendWindow = new SendWindow();
                sendWindow.Show();
                Signal();
            }); 

        }

        private void GetSelectedPOPEmails()
        { var email = PostRepository.Instance.GetSelectedPOPEmails(SelectedEmail); }
        private void RemoveMessage()
        {
            if (SelectedEmail == null)
            {
                MessageBox.Show("Не выбран обьект"); return;
            }
            else
                Email.Remove(selectedEmail);
        }


        MessageWindow messageWindow;
        internal void SetWindow(MessageWindow messageWindow)
        {
            this.messageWindow = messageWindow;
        }
    }
}
