using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using post2.model;
using post2.view;

namespace post2.ViewModel
{
    public class SendWindowVm : BaseVM
    {
        private MainMenuVM mainMenuVM;
        public string Adress { get; set; }
        public string bbody { get; set; }
        public string ssubject { get; set; }
        string selectedImagePath = "";

    }
}
