using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace post2.ViewModel
{
    public class RegVM : BaseVM
    {
      

            #region OpenMenuCommand
            public ICommand OpenMenuCommand { get; }
            private bool CanOpenMenuCommandExecute(object param)
            {
                return true;
            }
            private void OnOpenMenuCommandExecute(object param)
            {

                var args = (object[])param;
                var buttonOpen = (Button)args[0];
                var buttonClose = (Button)args[1];
                buttonOpen.Visibility = Visibility.Collapsed;
                buttonClose.Visibility = Visibility.Visible;
            }
            #endregion



            //public RegVM()
            //{
            //    OpenMenuCommand = new RelayCommand<object>(OnOpenMenuCommandExecute, CanOpenMenuCommandExecute);
            //}

    }
}
