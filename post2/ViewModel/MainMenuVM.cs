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
    public class MainMenuVM : BaseVM
    {
        Pop3Client pop3Client;
        private ObservableCollection<Popemail> email = new();
        private ObservableCollection<EmailMenu> emailsdb = new();
        public EmailMenu selectedEmail = new();
        public CommandVm UpgratePost { get; }
        public CommandVm Search { get; }
        public CommandVm Edit { get; }
        public CommandVm Delete { get; }
        public CommandVm SendWindow { get; }
        public CommandVm OpenDeleteMenu { get; }
        public CommandVm OpenUserWindow { get; }
        private DispatcherTimer timer = null;     
        public EmailMenu SelectedEmail
        {
            get => selectedEmail;
            set
            {
                selectedEmail = value;
                Signal();
            }
        }
        //public void timerStart(MainMenu mainMenu)
        //{
        //    this.mainMenu = mainMenu;
        //    timer = new DispatcherTimer();
        //    timer.Tick += new EventHandler(timerTick);
        //    timer.Interval = new TimeSpan(0, 0, 2);
        //    timer.Start();
        //}
        public void MessageSee(MainMenu mainMenu) //сделай ее пж эт ОЧЕНЬ важно
        {
            if (SelectedEmail != null)
            {
                //MouseLeftButtonDown;
                //    MessageWindow messageWindow = new MessageWindow();
                //    messageWindow.Show();
                //    Signal();
            }
        }
        //private void timerTick(object sender, EventArgs e)
        //{
        //    Thread thread = new Thread(GetMail);
        //    thread.Start();
        //}
        public string TextSearch { get; set; }
        public ObservableCollection<Popemail> Email { get => email; set => email = value; }
        public ObservableCollection<EmailMenu> Emaildb { get => emailsdb; set => emailsdb = value; }

        public MainMenuVM()
        {
            string sql = "SELECT ID, ID_AdressFrom, Subjecct, Body, DateSend FROM email where ID_StatusEmail is null and ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + ";";
            Emaildb = new ObservableCollection<EmailMenu>(PostRepository.Instance.GetAllPOPEmails(sql));
            UpgratePost = new CommandVm(() =>
            {
                GetMail(Email);
                Signal();

            });
            Delete = new CommandVm(() => {
                if (SelectedEmail == null)
                {
                    MessageBox.Show("Обьект не выбран"); return;
                }
                else
                {
                    try
                    {
                        SelectedEmail.ID_StatusEmail = 1;                       
                        PostRepository.Instance.UpdatePOPEmail(SelectedEmail);
                        //Emaildb.Remove(SelectedEmail);
                        Signal();                     
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            });
            Search = new CommandVm(() =>
            {
                //PostRepository.Instance.Search(TextSearch, selectedEmail);
                //Signal();
            });
            SendWindow = new CommandVm(() =>
            {
                SendWindow sendWindow = new SendWindow();
                sendWindow.Show();
                Signal();
            });
            OpenUserWindow = new CommandVm(() =>
            {
                UserWindow userWindow = new UserWindow();
                userWindow.Show();
                Signal();
            });
            OpenDeleteMenu = new CommandVm(() =>
            {
                DeleteMenu deletemenu = new DeleteMenu();
                deletemenu.Show();
                Signal();
            });
        }
        private void AddPOPEmail() { }
        private void GetCoutMessage() {
            var c = 0;
            var countdb = PostRepository.Instance.GetCoutMessage(c); 
        }
        //private void GetAllPOPEmails()
        //{ var email = PostRepository.Instance.GetAllPOPEmails(); }
        public static Pop3Client ConnectMail()
        {
            Pop3Client pop3Client = new Pop3Client();
            var username = "alina1125@suz-ppk.ru";
            var password = "D35de%TJ";
            pop3Client.Connect("pop3.beget.com", 110, false);
            pop3Client.Authenticate(username, password, AuthenticationMethod.UsernameAndPassword);
            var user = ActiveUser.Instance.GetUser();
            return pop3Client;
        }
        bool first = true;
        int lastCount = 0;  
        void GetMail(object p)             
        {
            
            try
            {
                pop3Client = ConnectMail();
            }
            catch 
            {
                MessageBox.Show("Error");
            }
            int count = 0;
            try
            {
                count = pop3Client.GetMessageCount();
            }
            catch { return; }
            int lastCountFor = 0;
            PostRepository.Instance.GetCoutMessage(lastCountFor);          
            var countdb = lastCountFor;           
            int counter = 0;
            Message message;
            for (int i = count; countdb > i; i--)
            {
                try
                {
                    message = pop3Client.GetMessage(i);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    continue;
                }
                Popemail email = new Popemail()
                {
                    MessageNumber = i,
                    Subject = message.Headers.Subject,
                    DateSend = message.Headers.DateSent,
                    EmailFrom = message.Headers.From.Address,
                    ID_AdressTo = ActiveUser.Instance.GetUser().IDAddress
                };             
                PostRepository.Instance.AddPOPEmail(email);
               
                MessagePart body = message.FindFirstHtmlVersion();
                if (body != null)
                {
                    email.Body = HttpUtility.HtmlDecode(Regex.Replace(body.GetBodyAsText(), "<(.|\n)*?>", ""));
                }
                else
                {
                    body = message.FindFirstPlainTextVersion();
                    if (body != null)
                    {
                        email.Body = body.GetBodyAsText();
                    }
                }
                List<MessagePart> attachments = message.FindAllAttachments();

                foreach (MessagePart part in attachments)
                {
                    email.Attachments.Add(new Attachments
                    {
                        Title = part.FileName,
                        ContentType = part.ContentType.MediaType,
                        Content = part.Body
                    });
                }
                mainMenu.Dispatcher.Invoke(() =>
                {
                    if (first)
                        Email.Add(email);
                    else
                        Email.Insert(0, email);
                });
                counter++;
            }
            first = false;
            try
            {
                pop3Client.Disconnect();
            }
            catch { }
        }
        MainMenu mainMenu;
        internal void SetWindow(MainMenu mainMenu)
        {
            this.mainMenu = mainMenu;
        }
        //private void DeletePost()
        //{
        //    if (SelectedEmail == null)
        //    {
        //        MessageBox.Show("Не выбран обьект");
        //        return;
        //    }
        //    try
        //    {
        //        //pop3Client = ConnectMail();
        //        PostRepository.Instance.RemovePOPEmail(SelectedEmail);
        //        //pop3Client.DeleteMessage(SelectedEmail.MessageNumber);
        //        //pop3Client.Disconnect();
        //        var index = SelectedEmail.MessageNumber;
        //        PostRepository.Instance.UpdatePOPEmail(SelectedEmail);

        //        Emaildb.Remove(SelectedEmail);
        //        var sort = Emaildb.ToArray();
        //        Array.Sort(sort, (x, y) => y.DateSend.CompareTo(x.DateSend));
        //        for (int i = 0; i < sort.Length; i++)
        //            sort[i].MessageNumber = i + 1;
        //    }
        //    catch { }
           
        //}
        private void RemoveMessage()
        {
            if (SelectedEmail == null)
            {
                MessageBox.Show("Не выбран обьект"); return;
            }
            else
                Emaildb.Remove(selectedEmail);
        }
    }
}


