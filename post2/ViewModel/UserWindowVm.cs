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
    public class UserWindowVm : BaseVM
    {
        public CommandVm Back { get; }
        public CommandVm Edit { get; }
        public UserWindowVm() 
        {
            Edit = new CommandVm(() =>
            { 
                UserEditWindow userEditWindow = new UserEditWindow();
                userEditWindow.Show();
                Signal();
            });
            //Ok = new CommandVm(() =>
            //{
            //    MainMenu mainMenu = new MainMenu();
            //    mainMenu.Show();
            //    CloseWindow(userWindow);
            //    Signal();
            //});
            Back = new CommandVm(() =>
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                CloseWindow(userWindow);
                Signal();
            });
            //Image = new CommandVm(() =>
            //{
            //      SelectImage(this, null);
            //      Signal();
            //});
        }
        UserWindow userWindow;
        internal void SetWindow(UserWindow userWindow)
        {
            this.userWindow = userWindow;
        }
        internal void CloseWindow(UserWindow userWindow)
        {
            this.userWindow.Close();
        }
        //private void SelectImage(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        selectedImagePath = openFileDialog.FileName;
        //        if (selectedImagePath.EndsWith(".png") || selectedImagePath.EndsWith(".jpg"))
        //        {
        //            byte[] imageData = File.ReadAllBytes(selectedImagePath);

        //            BitmapImage bitmapImage = new BitmapImage();
        //            bitmapImage.BeginInit();
        //            bitmapImage.StreamSource = new MemoryStream(imageData);
        //            bitmapImage.EndInit();

        //            selectedImage.Source = bitmapImage;
        //        }
        //    }
        //} 

        //Image selectedImage;
        //internal void SetImage(Image selectedImage)
        //{
        //    this.selectedImage = selectedImage;
        //}
    }
}
