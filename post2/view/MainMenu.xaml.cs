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
using post2.ViewModel;



namespace post2.view
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {     
        public MainMenu()
        {
            InitializeComponent();
            ((MainMenuVM)this.DataContext).SetWindow(this);
        }
      
    }
}
