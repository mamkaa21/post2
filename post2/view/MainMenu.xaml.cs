using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenPop.Mime;
using OpenPop.Pop3;
using System.Net;
using System.Net.Mail;
using post2.model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Threading;


namespace post2.view
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        Pop3Client pop3Client;
        public MainMenu()
        {
            InitializeComponent();
            //DataContext = new MainMenuVM();
        }
        //private DispatcherTimer timer = null;
        //private void timerStart()
        //{
        //    timer = new DispatcherTimer();
        //    timer.Tick += new EventHandler(timerTick);
        //    timer.Interval = new TimeSpan(0, 0, 2);
        //    timer.Start();
        //}

        //private void timerTick(object sender, EventArgs e)
        //{
        //    Thread thread = new Thread(GetMail);
        //    thread.Start();
        //}
        ////private void Button_Send_Window(object sender, EventArgs e)
        ////{
        ////    SendingWindow sendingWindow = new SendingWindow();
        ////    sendingWindow.Show();
        ////}

        //bool first = true;
        //int lastCount = 0;

        //void GetMail(object p)
        //{
        //    pop3Client = ConnectMail();
        //    int count = 0;
        //    try
        //    {
        //        count = pop3Client.GetMessageCount();
        //    }
        //    catch { return; }

        //    var lastCountFor = lastCount;
        //    lastCount = count;

        //    int counter = 0;
        //    Message message;
        //    for (int i = count; i > lastCountFor; i--)
        //    {
        //        try
        //        {
        //            message = pop3Client.GetMessage(i);
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.Message);
        //            continue;
        //        }
        //        Popemail email = new Popemail()
        //        {
        //            MessageNumber = i,
        //            Subject = message.Headers.Subject,
        //            DateSent = message.Headers.DateSent,
        //            //ID_AdressFrom = message.Headers.From.Address,
        //            //ID_AdressTo = ActiveUser.Instance.GetUser().IDAddress
        //        };
        //        //PostRepository.Instance.AddPOPEmail(email);
        //        MessagePart body = message.FindFirstHtmlVersion();
        //        if (body != null)
        //        {
        //            email.Body = HttpUtility.HtmlDecode(Regex.Replace(body.GetBodyAsText(), "<(.|\n)*?>", ""));
        //        }
        //        else
        //        {
        //            body = message.FindFirstPlainTextVersion();
        //            if (body != null)
        //            {
        //                email.Body = body.GetBodyAsText();
        //            }
        //        }
        //        List<MessagePart> attachments = message.FindAllAttachments();
        //        foreach (MessagePart part in attachments)
        //        {
        //            email.Attachments.Add(new Attachments
        //            {
        //                Title = part.FileName,
        //                ContentType = part.ContentType.MediaType,
        //                Content = part.Body
        //            });
        //        }
        //        this.Dispatcher.Invoke(() =>
        //        {
        //            if (first)
        //                Email.Add(email);
        //            else
        //                Email.Insert(0, email);
        //        });
        //        counter++;
        //    }
        //    first = false;

        //    try
        //    {
        //        pop3Client.Disconnect();
        //    }
        //    catch { }
        //}

        //private void Button_Upgrate(object sender, RoutedEventArgs e)
        //{
        //    Thread thread = new Thread(GetMail);
        //    thread.Start();
        //}

        //private static Pop3Client ConnectMail()
        //{
        //    Pop3Client pop3Client = new Pop3Client();

        //    var username = "alina1125@suz-ppk.ru";
        //    var password = "D35de%TJ";
        //    pop3Client.Connect("pop3.beget.com", 110, false);
        //    pop3Client.Authenticate(username, password, AuthenticationMethod.UsernameAndPassword);
        //    return pop3Client;
        //}

        //public event PropertyChangedEventHandler? PropertyChanged;
        //public ObservableCollection<Popemail> Email { get; set; } = new();

        //private Popemail email;
        //public Popemail pOPEmail
        //{
        //    get => email;
        //    set
        //    {
        //        email = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Popemail)));
        //    }
        //}

        //private void Button_Delete(object sender, RoutedEventArgs e)
        //{
        //    //if (pOPEmail == null)
        //    //{
        //    //    MessageBox.Show("Не выбран обьект");
        //    //    return;
        //    //}
        //    //try
        //    //{
        //    //    pop3Client = ConnectMail();
        //    //    PostRepository.Instance.RemovePOPEmail(email);
        //    //    pop3Client.DeleteMessage(pOPEmail.MessageNumber);
        //    //    pop3Client.Disconnect();
        //    //    var index = pOPEmail.MessageNumber;
        //    //    Email.Remove(email);
        //    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
        //    //    var sort = Email.ToArray();
        //    //    Array.Sort(sort, (x, y) => y.DateSent.CompareTo(x.DateSent));
        //    //    for (int i = 0; i < sort.Length; i++)
        //    //        sort[i].MessageNumber = i + 1;
        //    //}
        //    //catch { }
        //    //PostRepository.Instance.UpdatePOPEmail(email);
        //}

        //private void OnClose(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        pop3Client.Disconnect();
        //    }
        //    catch { }
        //}
        ////private void But_DelDel(object sender, RoutedEventArgs e)
        ////{
        ////    DeleteMenu deleteMenu = new DeleteMenu();
        ////    deleteMenu.Show();
        ////    this.Close();
        ////    //if (POPEmail == null)
        ////    //{
        ////    //    MessageBox.Show("Не выбран обьект");
        ////    //    return;
        ////    //}
        ////    //try
        ////    //{
        ////    //    pop3Client = ConnectMail();
        ////    //    pop3Client.DeleteMessage(POPEmail.MessageNumber);
        ////    //    pop3Client.Disconnect();
        ////    //    var index = POPEmail.MessageNumber;
        ////    //    Email.Remove(POPEmail);
        ////    //    var sort = Email.ToArray();
        ////    //    Array.Sort(sort, (x, y) => y.DateSent.CompareTo(x.DateSent));
        ////    //    for (int i = 0; i < sort.Length; i++)
        ////    //        sort[i].MessageNumber = i + 1;
        ////    //}
        ////    //catch { }
        ////}
        //private void But_Search(object sender, RoutedEventArgs e) { }

        ////private void But_SendPost(object sender, RoutedEventArgs e)
        ////{
        ////    WindowSendPost windowSendPost = new WindowSendPost();
        ////    windowSendPost.Show();
        ////    this.Close();
        ////}
        ////private void But_WinUser(object sender, RoutedEventArgs e)
        ////{
        ////    WindowUser windowUser = new WindowUser();
        ////    windowUser.Show();
        ////    this.Close();
        ////}
    }
}
