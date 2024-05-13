using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace post2.model
{
    public class ActiveUser
    {
        private ActiveUser() { }

        static ActiveUser instance;
        public static ActiveUser Instance
        {
            get
            {
                if (instance == null)
                    instance = new ActiveUser();
                return instance;
            }
        }
        private User user;
        public User GetUser()
        {
            return user;
        }

        public void SetUser(User value)
        {
            user = value;
        }
    }
}
