﻿using System;
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
using System.Drawing.Imaging;
using System.Windows.Controls;
using System.Threading.Channels;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace post2.ViewModel
{
    public class UserEditWindowVM : BaseVM
    {
        private User user = new();
        string selectedImagePath = "";
        byte[] SelectedImageUser;
        public CommandVm Ok { get; }
        //public CommandVm ImageAdd { get; }
        public User Users { get => user; set => user = value; }
        public UserEditWindowVM()
        {
            Users = ActiveUser.Instance.GetUser();
            Ok = new CommandVm(() =>
            {
                //MemoryStream ms = new MemoryStream();
                //Users.Image.Save(ms, selectedImageUser.Image.RawFormat);
                //byte[] img = ms.ToArray();
                //String insertQuery = "insert into user(image)";
                //var connect = MySqlDB.Instance.GetConnection();
                //if (connect == null)
                //    return;
                //using (var mc = new MySqlCommand(insertQuery, connect))
                //{ mc.Parameters.Add("@image", MySqlDbType.MediumBlob); }
                //UserRepository.Instance.UpdateUser(user);
                CloseWindow(userEditWindow);
                Signal();
            });
            //ImageAdd = new CommandVm(() =>
            //{
            //    SelectImage(this, null);
            //    Signal();
            //});
        }
        private void SelectImage(object sender, RoutedEventArgs e) //добавление изображение поль-лю
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

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
                    //UserRepository.Instance.AddUserImage();
                }
            }
        }


            //        if (openFileDialog.ShowDialog() == DialogResult.OK)
            //        {
            //            pictureSet.Image = Image.FromFile(openFileDialog.FileName);
            //        }
            //    }
            //    //

            //    //
            //}
            UserEditWindow userEditWindow;
            //internal void Updateuser(UserEditWindow userEditWindow) 
            //{
            //    this.userEditWindow = userEditWindow;
            //    UserRepository.Instance.UpdateUser(user);
            //}
            internal void SetWindow(UserEditWindow userEditWindow) //привязка окна к вм
            {
                this.userEditWindow = userEditWindow;
            }
            internal void CloseWindow(UserEditWindow userEditWindow) //закрытие окна
            {
                this.userEditWindow.Close(); }

            Image selectedImage;
            internal void SetImage(Image selectedImage) //привязка Image к вм
            {
                this.selectedImage = selectedImage;
            }
        
    }
}

