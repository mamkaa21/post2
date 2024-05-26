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
        { string sql = "select ID, ID_AdressFrom, Subjecct, Body FROM Email WHERE ID_StatusEmail is not null and ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + "; ";
            Emaildb = new ObservableCollection<EmailMenu>(PostRepository.Instance.GetDelPOPEmail(sql));
            Back = new CommandVm(() =>
            {
                CloseWindow(deleteMenu);
                Signal();
            });
            Delete = new CommandVm(() =>
            {
                //if (SelectedEmail == null)
                //{
                //    MessageBox.Show("Обьект не выбран"); return;
                //}
                //else
                //{
                //    try
                //    {
                //        SelectedEmail.ID_StatusEmail = 1;
                //        PostRepository.Instance.UpdatePOPEmail(SelectedEmail);
                //        Signal();
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show($"Ошибка: {ex.Message}");
                //    }
                //}
                //DeletePost();
            });
            Return = new CommandVm(() =>
            {
                
            });
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
                //pop3Client = ConnectMail();
                //PostRepository.Instance.RemovePOPEmail(selectedEmail);
                //pop3Client.DeleteMessage(SelectedEmail.MessageNumber);
                //pop3Client.Disconnect();
                //var index = SelectedEmail.MessageNumber;
                Emaildb.Remove(selectedEmail);
                //var sort = Email.ToArray();
                //Array.Sort(sort, (x, y) => y.DateSend.CompareTo(x.DateSend));
                //for (int i = 0; i < sort.Length; i++)
                //    sort[i].MessageNumber = i + 1;
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
