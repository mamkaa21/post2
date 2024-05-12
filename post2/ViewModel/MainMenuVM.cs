using OpenPop.Mime;
using OpenPop.Pop3;
using post2.model;
using post2.view;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Threading;

namespace post2.ViewModel
{
    public class MainMenuVM : BaseVM
    {
        Pop3Client pop3Client;
        private ObservableCollection<Popemail> email;
        private Popemail newEmail;
        public Popemail selectedEmail = new();
        public CommandVm UpgratePost { get; }
        public CommandVm Search { get; }
        public CommandVm Edit { get; }
        public CommandVm Delete { get; }
        public CommandVm SendWindow { get; }
        public CommandVm OpenDeleteMenu { get; }
        private DispatcherTimer timer = null;
        string sql = "select e.id, e.subject, e.body, e.datesend from email e";
        //Popemail = ObservableCollection<Popemail>(PostRepository.Instance.GetAllEmail);
        public Popemail SelectedEmail
        {
            get => selectedEmail;
            set
            {
                selectedEmail = value;
                Signal();
            }
        }
        public Popemail NewEmail
        {
            get => newEmail; 
            set
            {
                selectedEmail = value;
                Signal();
            }
        }
        private void timerStart()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
        } 
        private void timerTick(object sender, EventArgs e)
        {
            Thread thread = new Thread(GetMail);
            thread.Start();
        }
        public MainMenuVM()
        {
            UpgratePost = new CommandVm(() =>
            {
                GetMail(email);
            });

            //Edit = new CommandVm(() =>
            //{
            //    if (SelectedEmail == null)
            //        return;
            //    else
            //    {
            //        PostRepository.Instance.Edit(NewEmail);
            //        Signal();
            //    }
            //});
            //Delete = new CommandVm(() =>
            //{
            //    if (SelectedEmail == null)
            //        return;
            //    else
            //    {
            //       PostRepository.Instance.RemoveEmail(Email);
            //        Signal();
            //    }
            //});
            //Search = new CommandVm(() =>
            //{
            //    if (selectedEmail == null)
            //        return;
            //    else
            //    {
            //        PostRepository.Instance.Search(Email);
            //        Signal();
            //    }
            //});

            SendWindow = new CommandVm(() => 
            {
                SendWindow sendWindow = new SendWindow();
                sendWindow.Show();
            });

        
        }
        public static Pop3Client ConnectMail()
        {
            Pop3Client pop3Client = new Pop3Client();
            var username = "alina1125@suz-ppk.ru";
            var password = "D35de%TJ";
            pop3Client.Connect("pop3.beget.com", 110, false);
            pop3Client.Authenticate(username, password, AuthenticationMethod.UsernameAndPassword);
           //var user = ActiveUser.Instance.GetUser();
            return pop3Client;
        }
        bool first = true;
        int lastCount = 0;
        void GetMail(object p)
        {
            pop3Client = ConnectMail();
            int count = 0;
            try
            {
                count = pop3Client.GetMessageCount();
            }
            catch { return; }
            var lastCountFor = lastCount;
            lastCount = count;
            int counter = 0;
            Message message;
            for (int i = count; i > lastCountFor; i--)
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
                    DateSent = message.Headers.DateSent,
                    EmailFrom = message.Headers.From.Address,
                    //ID_AdressTo = ActiveUser.Instance.GetUser().IDAddress
                };
                //PostRepository.Instance.Popemail(email);

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
                //email.Dispatcher.Invoke(() =>
                //{
                //    if (first)
                //        newEmail.Add(email);
                //    else
                //        newEmail.Insert(0, email);
                //});
                //counter++;
            }
            first = false;
            try
            {
                pop3Client.Disconnect();
            }
            catch { }
        }

      

    }

}
