using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MySqlConnector;
using System.Net;

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
        internal User GetUserByLoginPassword(string login, string password) //получение поль-теля по логину и паролю
        {
            User result = new User();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT u.ID, u.NickName, u.Login, u.Password, ab.Email, ab.Title, ab.ID AS idAddress FROM User u, AdressBook ab WHERE u.Login = @login AND u.Password = @password AND ab.ID_User = u.ID";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("login", login));
                mc.Parameters.Add(new MySqlParameter("password", password));
                using (var reader = mc.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.ID = reader.GetInt32("id");
                        result.NickName = reader.GetString("NickName");
                        result.Login = reader.GetString("Login");
                        result.Password = reader.GetString("Password");
                        result.Email = reader.GetString("Email");
                        result.EmailTitle = reader.GetString("Title");
                        result.IDAddress = reader.GetInt32("idAddress");                      
                    }
                }
                return result;
            }
        }     
        internal User AddUserByLoginPassword(string nickname, string login, string password) //добавление юзера в бд
        {
            User result = new User();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            int id = MySqlDB.Instance.GetAutoID("User");
            string sql = "INSERT INTO User VALUES (0, @nickname, @login, @password, @image)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("nickname", nickname));
                mc.Parameters.Add(new MySqlParameter("login", login));
                mc.Parameters.Add(new MySqlParameter("password", password));

                mc.ExecuteNonQuery();
                result = new User { NickName = nickname, Login = login, Password = password };
                if (mc.ExecuteNonQuery() > 0)
                {
                    sql = "";
                    foreach (var ab in result.AdressBooks)
                    sql += "INSERT INTO AdressBook VALUES (" + id + "," + ab.ID + "," + ab.Email + "," + ab.Title + "," + ab.ID_User + ");";
                    using (var mcCross = new MySqlCommand(sql, connect))
                    mcCross.ExecuteNonQuery();
                }
            }
            return result;
        }
        //internal User AddUserImage() //добавление фото юзера(нерабочее)//жестко думать надо очень
        //{
        //    User result = new User();
        //    byte[] image;
        //    var connect = MySqlDB.Instance.GetConnection();
        //    if (connect == null)
        //        return result;
        //    int id = MySqlDB.Instance.GetAutoID("User");
        //    string sql = "INSERT INTO User VALUES (@image)";
        //    using (var mc = new MySqlCommand(sql, connect))
        //    {
        //        mc.Parameters.Add(new MySqlParameter("image", image));            
        //        mc.ExecuteNonQuery();
        //    }
        //    return result;
        //}
        internal void UpdateUser(User user) 
        {
            User result = new User();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return;
            string sql = "update user set NickName = @nickname, Login = @login, Password = @password where id = " + ActiveUser.Instance.GetUser().ID + ";";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("nickname", user.NickName));
                mc.Parameters.Add(new MySqlParameter("login", user.Login));
                mc.Parameters.Add(new MySqlParameter("password", user.Password));
                mc.ExecuteNonQuery();
            
            //mc.Parameters.Add(new MySqlParameter("image", user.Image));
            return;
            }
        }
    
        internal User DeleteUser(User user) //удаление юзера
            {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null) return user;
            string sql = "DELETE FROM User WHERE id = ' " + user.ID + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();

            return user;
            }
    }
}
  