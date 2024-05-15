using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using post2.model;
using post2.view;
using OpenPop.Mime;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Channels;

namespace post2.ViewModel
{
    public class SendWindowVm : BaseVM
    {
        private MainMenuVM mainMenuVM;
        public string Adress { get; set; }
        public string bbody { get; set; }
        public string ssubject { get; set; }

        string selectedImagePath = "";
        public CommandVm SendPost { get; }
        public CommandVm Image { get; }
        public CommandVm Back { get; }
        private ObservableCollection<Popemail> email;
        public SendWindowVm()
        {
            SendPost = new CommandVm(() =>
            {
                Sending(this, null);              
                Signal();
            });

            Image = new CommandVm(() =>
            {
                SelectImage(this, null);
                Signal();
            });
            Back = new CommandVm(() => 
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                CloseWindow(sendWindow);
                Signal();
            }
            );
        }
        private void Sending(object sender, EventArgs e)
        {
            MailAddress fromAdress = new MailAddress("alina1125@suz-ppk.ru", "Alina");
            MailAddress toAdress = new MailAddress(Adress);
            MailMessage message = new MailMessage(fromAdress, toAdress);
            message.Body = bbody;
            message.Subject = ssubject;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.beget.com";
            smtpClient.Port = 25;
            smtpClient.EnableSsl = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(fromAdress.Address, "D35de%TJ");
            if(!string.IsNullOrEmpty(selectedImagePath))
                message.Attachments.Add(new Attachment(selectedImagePath)); 
            smtpClient.Send(message); 
            CloseWindow(sendWindow);
        }
        
                
    private void SelectImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                if (selectedImagePath.EndsWith(".png") || selectedImagePath.EndsWith(".jpg"))
                {
                    byte[] imageData = File.ReadAllBytes(selectedImagePath);

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(imageData);
                    bitmapImage.EndInit();

                    selectedImage.Source = bitmapImage;
                }
            }
        }
        SendWindow sendWindow;
        internal void SetWindow(SendWindow sendWindow)
        {
            this.sendWindow = sendWindow;
        }
        internal void CloseWindow(SendWindow sendWindow)
        {
            this.sendWindow.Close();
        }
        Image selectedImage;
        internal void SetImage(Image selectedImage)
        {
            this.selectedImage = selectedImage;
        }
    }
}
