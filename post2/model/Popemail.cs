using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace post2.model
{
    public class Popemail
    {
        public Popemail()
        {
            Attachments = new List<Attachments>();
            AdressBooks = new ObservableCollection<AdressBook>();
        }
        public int ID { get; set; }
        public int ID_AdressFrom { get; set; }
        public int ID_AdressTo { get; set; }
        public int MessageNumber { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateSend { get; set; }
        public List<Attachments> Attachments { get; set; }
        public ObservableCollection<AdressBook> AdressBooks { get; set; }
        public string EmailFrom { get; internal set; }
        public string TitleFrom { get; internal set; }
    }
    [Serializable]
    public class Attachments
    {
        public int ID { get; set; }
        public byte[] Content { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
    }
}

