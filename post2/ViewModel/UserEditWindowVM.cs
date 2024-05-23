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
using MySqlConnector;

namespace post2.ViewModel
{
   public class UserEditWindowVM : BaseVM
    {
        byte[] selectedImageUser;
        string selectedImagePath = "";
        public CommandVm Ok { get; }
        public CommandVm Image { get; }

        public UserEditWindowVM()
        {
            Ok = new CommandVm(() =>
            {
                MemoryStream ms = new MemoryStream();
                selectedImageUser.Image.Save(ms, selectedImageUser.Image.RawFormat);
                byte[] img = ms.ToArray();
                String insertQuery = "insert into user(image)";
                var connect = MySqlDB.Instance.GetConnection();
                if (connect == null)
                    return;
                using (var mc = new MySqlCommand(insertQuery, connect))
                { mc.Parameters.Add("@image", MySqlDbType.MediumBlob); }
                    MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                CloseWindow(userEditWindow);
                Signal();
            });
            Image = new CommandVm(() =>
            {
                SelectImage(this, null);
                Signal();
            });
        }
        UserEditWindow userEditWindow;
        internal void SetWindow(UserEditWindow userEditWindow)
        {
            this.userEditWindow = userEditWindow;
        }
        internal void CloseWindow(UserEditWindow userEditWindow)
        {
            this.userEditWindow.Close();
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
        Image selectedImage;
        internal void SetImage(Image selectedImage)
        {
            this.selectedImage = selectedImage;
        }
    }
}
