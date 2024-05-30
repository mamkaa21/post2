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
    public class RandomMenuVM: BaseVM
    {
        private ObservableCollection<EmailMenu> emaildb;
        public EmailMenu selectedEmail = new();
        public CommandVm Back { get; }
        public CommandVm Delete { get; }
        public CommandVm Return { get; }
        public ObservableCollection<EmailMenu> Emaildb { get => emaildb; set => emaildb = value; }
        public EmailMenu SelectedEmail
        {
            get => selectedEmail;
            set
            {
                selectedEmail = value;
                Signal();
            }
        }
        public RandomMenuVM()
        {
            string sql = "SELECT e.ID, ab.Email, e.Subjecct, e.Body, e.DateSend FROM email e join AdressBook ab on e.ID_AdressFrom = ab.ID where ID_StatusEmail ='2' and ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + ";";
            Emaildb = new ObservableCollection<EmailMenu>(PostRepository.Instance.GetDelPOPEmail(sql));
            Back = new CommandVm(() =>
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                CloseWindow(randomMenu);
                Signal();
            });
            Delete = new CommandVm(() =>
            {
                if (SelectedEmail == null)
                {
                    MessageBox.Show("Обьект не выбран"); return;
                }
                else
                {
                    PostRepository.Instance.RemovePOPEmail(selectedEmail);
                    Emaildb.Remove(SelectedEmail);
                }
            });
            Return = new CommandVm(() =>
            {
                if (SelectedEmail == null)
                {
                    MessageBox.Show("Обьект не выбран"); return;
                }
                else
                {
                    try
                    {
                        SelectedEmail.ID_StatusEmail = 3;
                        PostRepository.Instance.UpdatePOPEmail(SelectedEmail);
                        Emaildb.Remove(SelectedEmail);
                        Signal();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            });
        }
        RandomMenu randomMenu;
        internal void SetWindow(RandomMenu randomMenu) //привязка окна к вм
        {
            this.randomMenu = randomMenu;
        }
        internal void CloseWindow(RandomMenu randomMenu) //закрытие окна 
        {
            this.randomMenu.Close();
        }

    }
}
