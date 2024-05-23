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

namespace post2.ViewModel
{
    /// <summary>
    /// Логика взаимодействия для UserEditWindowVM.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        public UserEditWindow()
        {
            InitializeComponent();
            ((UserEditWindowVM)this.DataContext).SetWindow(this);
            //var vm = DataContext as UserEditWindowVM;
            //vm.SetImage(SelectedImage);
        }
    }
}
