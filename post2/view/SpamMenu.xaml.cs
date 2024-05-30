using post2.ViewModel;
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
    /// Логика взаимодействия для SpamWindow.xaml
    /// </summary>
    public partial class SpamMenu : Window
    {
        public SpamMenu()
        {
            InitializeComponent();
            ((SpamMenuVM)this.DataContext).SetWindow(this);
        }
    }
}
