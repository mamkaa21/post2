using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using post2.model;
using post2.view;

namespace post2.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        public CommandVm EnterWindowOpen { get; }
  
        public MainWindowVM()
        {
            EnterWindowOpen = new CommandVm(() =>
            {
                EnterWindow enterWindow = new EnterWindow();
                enterWindow.Show();
                CloseWindow(mainWindow);
                Signal();
            });
        }
        MainWindow mainWindow;
        internal void SetWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        internal void CloseWindow(MainWindow mainWindow)
        { 
            this.mainWindow.Close();
        }
    }
}
