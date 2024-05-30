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
using System.Runtime.CompilerServices;

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
        public CommandVm OpenRandomMenu { get; }
        public CommandVm OpenDeleteMenu { get; }
        public CommandVm OpenUserWindow { get; }
        public CommandVm OpenSpamWindow { get; }
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
        //public void timerStart(MainMenu mainMenu) //таймер для автоматического обновления бд
        //{
        //    this.mainMenu = mainMenu;
        //    timer = new DispatcherTimer();
        //    timer.Tick += new EventHandler(timerTick);
        //    timer.Interval = new TimeSpan(0, 0, 2);
        //    timer.Start();
        //}
        /*public void MessageSee(MainMenu mainMenu) //сделай ее пж эт ОЧЕНЬ важно
        //{
        //    if (SelectedEmail != null)
        //    {
        //        //MouseLeftButtonDown;
        //        //    MessageWindow messageWindow = new MessageWindow();
        //        //    messageWindow.Show();
        //        //    Signal();
        //    }
        //}*/
        //private void timerTick(object sender, EventArgs e) //к таймеру относится 
        //{
        //    Thread thread = new Thread(GetMail);
        //    thread.Start();
        //}
        public string TextSearch { get; set; }
        public ObservableCollection<Popemail> Email { get => email; set => email = value; }
        public ObservableCollection<EmailMenu> Emaildb { get => emailsdb; set => emailsdb = value; }
        public MainMenuVM()
        {
            string sql = "SELECT e.ID, ab.Email, e.Subjecct, e.Body, e.DateSend FROM email e join AdressBook ab on e.ID_AdressFrom= ab.ID where ID_StatusEmail = '3' and ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + ";";
            Emaildb = new ObservableCollection<EmailMenu>(PostRepository.Instance.GetAllPOPEmails(sql));
            UpgratePost = new CommandVm(() =>
            {
                GetMail(Email);
                Emaildb = new ObservableCollection<EmailMenu>(PostRepository.Instance.GetAllPOPEmails(sql));
                Signal(nameof(Emaildb));
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
                        //SelectedEmail.DateSend = DateTime.Now;
                        SelectedEmail.ID_StatusEmail = 1;                       
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
                CloseWindow(mainMenu);
                Signal();
            });
            OpenRandomMenu = new CommandVm(() =>
            {
                RandomMenu randomMenu = new RandomMenu();
                    randomMenu.Show();
                    CloseWindow(mainMenu);
                if (SelectedEmail == null)
                {
                    MessageBox.Show("Обьект не выбран"); return;
                }
                else
                {
                    try
                    {
                        SelectedEmail.ID_StatusEmail = 2;
                        PostRepository.Instance.UpdatePOPEmail(SelectedEmail);
                        Emaildb.Remove(SelectedEmail);
                        Signal();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                    
                    Signal();
                }
            });
            OpenSpamWindow = new CommandVm(() =>
            {
                SpamMenu spamMenu = new SpamMenu();
                    spamMenu.Show();
                    CloseWindow(mainMenu);
                if (SelectedEmail == null)
                {
                    MessageBox.Show("Обьект не выбран"); return;
                }
                else
                {
                    try
                    {
                        SelectedEmail.ID_StatusEmail = 4;
                        PostRepository.Instance.UpdatePOPEmail(SelectedEmail);
                        Emaildb.Remove(SelectedEmail);
                        Signal();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                    
                    Signal();
                }
            });
        }
        private void AddPOPEmail() { }
        //private void GetCoutMessage() {
        //    var c = 0;
        //    var countdb = PostRepository.Instance.GetCoutMessage(c); 
        //}      
        public static Pop3Client ConnectMail() //конект к серверу 
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
        void GetMail(object p)//получение писем с сервера   
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
            int countdb = 0;
            countdb = PostRepository.Instance.GetCoutMessage();                            
            //int counter = 0;
            Message message;
            //for (int i = count; countdb < i; i--) 
            int i = count;
              while (countdb < i) 
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
                Popemail email = new()
                {
                    MessageNumber = i,
                    Subject = message.Headers.Subject,
                    DateSend = message.Headers.DateSent,
                    EmailFrom = message.Headers.From.Address,
                    ID_AdressTo = ActiveUser.Instance.GetUser().IDAddress
                };                                       
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
                email.ID_Statusemail = 3; 
                PostRepository.Instance.AddPOPEmail(email);            
                i--;
              }
            

            first = false;
            try
            {
                pop3Client.Disconnect();
            }
            catch { }
        }


        MainMenu mainMenu;
        internal void SetWindow(MainMenu mainMenu) //привязка окна к вм
        {
            this.mainMenu = mainMenu;
        }
        internal void CloseWindow(MainMenu mainMenu) //закрытие окна 
        {
            this.mainMenu.Close();
        }
    }
}


