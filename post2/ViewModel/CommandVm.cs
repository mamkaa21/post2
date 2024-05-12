using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace post2.ViewModel
{
    public class CommandVm : ICommand
    {
        Action action;
        public CommandVm(Action action)
        {
            this.action = action;
        }
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public void Execute(object? parameter)
        {
            action();
        }
    }
}
