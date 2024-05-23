using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace post2.model
{
    public class EmailMenu
    {
        public EmailMenu()
        {
            AttachmentsEM = new List<AttachmentsE>();
            AdressBooks = new ObservableCollection<AdressBook>();
        }
        public int ID { get; set; }
        public int ID_AdressFrom { get; set; }
        public int ID_AdressTo { get; set; }
        public int MessageNumber { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime DateSend { get; set; }
        public int ID_StatusEmail { get; set; }
        public DateTime DateDelete { get; set; }
        public List<AttachmentsE> AttachmentsEM { get; set; }
        public ObservableCollection<AdressBook> AdressBooks { get; set; }
        public string EmailFrom { get; internal set; }
        public string TitleFrom { get; internal set; }
    }
    public class AttachmentsE
    {
        public int ID { get; set; }
        public byte[] Content { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
    }
}

