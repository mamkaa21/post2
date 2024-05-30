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
        private ObservableCollection<EmailMenu> emaildb;
        public EmailMenu selectedEmail = new();
        public CommandVm Back { get; }
        public CommandVm Delete { get; }
        public ObservableCollection<EmailMenu> Emaildb { get => emaildb; set => emaildb = value; }
        public CommandVm Return { get; }
        public EmailMenu SelectedEmail
        {
            get => selectedEmail;
            set
            {
                selectedEmail = value;
                Signal();
            }
        }
        public DeleteMenuVm()
        {   string sql = "SELECT e.ID, ab.Email, e.Subjecct, e.Body, e.DateSend FROM email e join AdressBook ab on e.ID_AdressFrom = ab.ID where ID_StatusEmail ='1' and ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + ";";
            Emaildb = new ObservableCollection<EmailMenu>(PostRepository.Instance.GetDelPOPEmail(sql));
            Back = new CommandVm(() =>
            {
                CloseWindow(deleteMenu);
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
                //if (SelectedEmail == null)
                //{
                //    MessageBox.Show("Обьект не выбран"); return;
                //}
                //else
                //{
                //    try
                //    {                      
                //        SelectedEmail.ID_StatusEmail = 0;
                //        PostRepository.Instance.UpdatePOPEmail(SelectedEmail);
                //        Emaildb.Remove(SelectedEmail);
                //        Signal();
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show($"Ошибка: {ex.Message}");
                //    }
                //}
            });
        }     
        DeleteMenu deleteMenu;
        internal void SetWindow(DeleteMenu deleteMenu) //привязка окна к вм
        {
            this.deleteMenu = deleteMenu;
        }
        internal void CloseWindow(DeleteMenu deleteMenu) //закрытие окна 
        {
            this.deleteMenu.Close();
        }
    }
}
