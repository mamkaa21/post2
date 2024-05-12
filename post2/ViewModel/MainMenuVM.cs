using OpenPop.Mime;
using OpenPop.Pop3;
using post2.model;
using post2.view;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private Popemail email;
        public Popemail pOPEmail
        {
            get => email;
            set
            {
                email = value;
                Signal();
            }
        }
        public ObservableCollection<Popemail> Email { get; set; } = new();
        public CommandVm UpgratePost { get; set; }
        public CommandVm Search { get; set; }
        public CommandVm Delete { get; set; }
        public CommandVm Send { get; set; }
        public CommandVm OpenDeleteMenu { get; set; }

        private DispatcherTimer timer = null;
        public MainMenuVM()
        {
            UpgratePost = new CommandVm(() =>
            {
                GetMail(email);
            });
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
        private static Pop3Client ConnectMail()
        {
            Pop3Client pop3Client = new Pop3Client();
            var username = "alina1125@suz-ppk.ru";
            var password = "D35de%TJ";
            pop3Client.Connect("pop3.beget.com", 110, false);
            pop3Client.Authenticate(username, password, AuthenticationMethod.UsernameAndPassword);
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
                email.Dispatcher.Invoke(() =>
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

      

    }

}
