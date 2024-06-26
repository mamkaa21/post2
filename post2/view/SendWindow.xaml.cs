﻿using post2.ViewModel;
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

namespace post2.view
{
    /// <summary>
    /// Логика взаимодействия для SendWindow.xaml
    /// </summary>
    public partial class SendWindow : Window
    {
        public SendWindow()
        {
            InitializeComponent();
            ((SendWindowVm)this.DataContext).SetWindow(this);
            var vm = DataContext as SendWindowVm;
            vm.SetImage(SelectedImage);
        }
    }
}
