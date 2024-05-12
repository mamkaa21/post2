using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace post2.model
{
    public class UserRepository
    {
        private UserRepository() { }
        static UserRepository instance;
        public static UserRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new UserRepository();
                return instance;
            }
        }
    }
}
  