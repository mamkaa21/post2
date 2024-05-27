﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MySqlConnector;

namespace post2.model
{
    public class PostRepository
    {
        private PostRepository() { }
        static PostRepository instance;
        public static PostRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new PostRepository();
                return instance;
            }
        }
        internal IEnumerable<EmailMenu> GetAllPOPEmails(string sql) //работает но не правильно
        {
            ObservableCollection<EmailMenu> result = new ObservableCollection<EmailMenu>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            sql = "SELECT e.ID, ab.Email, e.Subjecct, e.Body, e.DateSend FROM email e, AdressBook ab where ID_StatusEmail is null and ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + ";";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                while (reader.Read())
                {
                    var menuemail = new EmailMenu();
                    menuemail.ID = reader.GetInt32("id");
                    menuemail.EmailFrom = reader.GetString("email");
                    menuemail.Subject = reader.GetString("Subjecct");
                    menuemail.Body = reader.GetString("Body");
                    menuemail.DateSend = reader.GetDateTime("DateSend");
                    result.Add(menuemail);                
                }
            }
            //добавитт проверку на аттачментс, если не нул, то вывести, если нул, то ток , что выше
            return result;
        }

        internal IEnumerable<EmailMenu> GetCoutMessage(int countdb)
        {
            ObservableCollection<EmailMenu> result = new ObservableCollection<EmailMenu>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
          string sql = "SELECT COUNT(*) AS countdb FROM email e, user u WHERE u.id = " + ActiveUser.Instance.GetUser().ID + ";";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                if (reader.Read())
                    countdb = reader.GetInt32(0);            
            }
                return result;
        }
        /*  internal IEnumerable<EmailMenu> GetSelectedPOPEmails(EmailMenu menuemail)
          {
              ObservableCollection<EmailMenu> result = new ObservableCollection<EmailMenu>();
              var connect = MySqlDB.Instance.GetConnection();
              if (connect == null)
                  return result;
              if (menuemail != null)
              {
                  string sql = "SELECT ID, ID_AdressFrom, ID_AdressTo, Subjecct, Body, DateSend, FROM email where ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + " AND ID_AdressFrom = AdressBook.ID";
                  using (var mc = new MySqlCommand(sql, connect))
                  using (var reader = mc.ExecuteReader())
                  {
                      while (reader.Read())
                      {
                          var menuemail = new EmailMenu();
                          menuemail.ID = reader.GetInt32("id");
                          menuemail.ID_AdressFrom = reader.GetInt32("ID_AdressFrom");
                          menuemail.ID_AdressTo = reader.GetInt32("ID_AdressTo");
                          menuemail.Subject = reader.GetString("Subjecct");
                          menuemail.Body = reader.GetString("Body");
                          menuemail.DateSend = reader.GetDateTime("DateSend");
                          result.Add(menuemail);
                      }
                  }
              }
              return result;     
          }*/
        internal Popemail AddPOPEmail(Popemail email)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null) return email;
            string sql = "select id from AdressBook where Email = '" + email.EmailFrom + "'";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                if (reader.Read())
                    email.ID_AdressFrom = reader.GetInt32(0);
            }
            if (email.ID_AdressFrom == 0)
            {
                email.ID_AdressFrom = MySqlDB.Instance.GetAutoID("AdressBook");
                sql = "INSERT INTO AdressBook VALUES (0, @AdressFrom, '', null)";
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("AdressFrom", email.EmailFrom));
                    mc.ExecuteNonQuery();
                }
            }
            int id = MySqlDB.Instance.GetAutoID("Email");         
            { sql = "INSERT INTO Email VALUES (0, @id_AdressFrom, @id_AdressTo, @subjecct, @body, @datesend, null, null)";
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("id_AdressFrom", email.ID_AdressFrom));
                    mc.Parameters.Add(new MySqlParameter("id_AdressTo", email.ID_AdressTo));
                    mc.Parameters.Add(new MySqlParameter("subjecct", email.Subject));
                    mc.Parameters.Add(new MySqlParameter("body", email.Body));
                    mc.Parameters.Add(new MySqlParameter("datesend", email.DateSend));
                    mc.ExecuteNonQuery();
                }
                email.ID = id;
            }
                return email;         
        }
        internal Popemail AddDateDelete(Popemail email)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null) return email;
            string sql = "select id from AdressBook where Email = '" + email.ID_AdressFrom + "'";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                if (reader.Read())
                    email.ID_AdressFrom = reader.GetInt32(0);
            }
            if (email.ID_AdressFrom == 0)
            {
                email.ID_AdressFrom = MySqlDB.Instance.GetAutoID("AdressBook");
                sql = "INSERT INTO AdressBook VALUES (0, @AdressFrom, '', null)";
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("AdressFrom", email.ID_AdressFrom));
                    mc.ExecuteNonQuery();
                }
            }
            int id = MySqlDB.Instance.GetAutoID("Email");
            {
                sql = "INSERT INTO Email VALUES (0, @id_AdressFrom, @id_AdressTo, @subjecct, @body, @datesend, null, null)";
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("id_AdressFrom", email.ID_AdressFrom));
                    mc.Parameters.Add(new MySqlParameter("id_AdressTo", email.ID_AdressTo));
                    mc.Parameters.Add(new MySqlParameter("subjecct", email.Subject));
                    mc.Parameters.Add(new MySqlParameter("body", email.Body));
                    mc.Parameters.Add(new MySqlParameter("datesend", email.DateSend));
                    mc.ExecuteNonQuery();
                }
                email.ID = id;
            }
            return email;
        }
        internal IEnumerable<EmailMenu> GetDelPOPEmail(string sql)
        { 
            ObservableCollection<EmailMenu> result = new ObservableCollection<EmailMenu>();
            var connect = MySqlDB.Instance.GetConnection(); 
            if (connect == null) return result;
            sql = "select e.ID, ab.email, e.Subjecct, e.Body FROM Email e, adressbook ab WHERE ID_StatusEmail = '1' and ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + ";";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                while (reader.Read())
                {
                    var menuemail = new EmailMenu();
                    menuemail.ID = reader.GetInt32("id");
                    menuemail.EmailFrom = reader.GetString("email");
                    menuemail.Subject = reader.GetString("Subjecct");
                    menuemail.Body = reader.GetString("Body");
                    //menuemail.DateDelete = reader.GetDateTime("DateDelete");
                    result.Add(menuemail);             
                }
            }        
            return result;
        }
        internal EmailMenu RemovePOPEmail(EmailMenu menuemail)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null) return menuemail;
            string sql = "DELETE FROM Email WHERE id = ' " + menuemail.ID + "';";
            using (var mc = new MySqlCommand(sql, connect))
            mc.ExecuteNonQuery();
            return menuemail;
        }
        //internal IEnumerable<EmailMenu> Search(string searchText, EmailMenu menuemail)
        //{
        //    string sql = "select  e.ID_AdressFrom, e.Subjecct, e.Body, e.DateSend from Email e";
        //    if (menuemail.ID != 0)
        //    {
        //        var result = GetAllPOPEmails().Where(s => s.AdressBooks.FirstOrDefault(s => s.ID == menuemail.ID) != null);
        //        return result;
        //    }
        //    return GetAllPOPEmails();
        //}
        internal void UpdatePOPEmail(EmailMenu menuemail)
        {
            ObservableCollection<EmailMenu> result = new ObservableCollection<EmailMenu>();
            var connect = MySqlDB.Instance.GetConnection();
            {
                if (connect == null) return;
                string sql = "update Email set ID_StatusEmail = @ID_StatusEmail where ID = " + menuemail.ID;
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.AddWithValue("@ID_StatusEmail", menuemail.ID_StatusEmail);
                    //mc.Parameters.AddWithValue("@Datedelete", menuemail.DateDelete);
                    mc.Parameters.AddWithValue("@ID", menuemail.ID);                 
                    mc.ExecuteNonQuery();
                    result.Clear();
                    sql += "SELECT e.ID, ab.email, e.Subjecct, e.Body, e.DateSend FROM email e, adressbook ab where ID_StatusEmail is null and ID_AdressTo " +
                        "= " + ActiveUser.Instance.GetUser().IDAddress + ";";
                    using (var reader = mc.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuemail.ID = reader.GetInt32("id");
                            menuemail.EmailFrom = reader.GetString("email");
                            menuemail.Subject = reader.GetString("Subjecct");
                            menuemail.Body = reader.GetString("Body");
                            menuemail.DateSend = reader.GetDateTime("DateSend");
                            result.Add(menuemail);
                            mc.ExecuteNonQuery();
                        }
                    }
                }
            }       
        }
        //string sql = "update Email set DateDelete = @DateDelete where ID = " + menuemail.ID;       
        //using (var mc = new MySqlCommand(sql, connect))
        //{             
        //    mc.Parameters.Add(new MySqlParameter("DateDelete", menuemail.DateDelete)); 
        //    mc.ExecuteNonQuery();
        //}
        //датеделете добавить когда удаляю
    }
}


