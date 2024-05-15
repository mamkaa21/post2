using System;
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
        internal IEnumerable<Popemail> GetAllPOPEmails()
        {
            ObservableCollection<Popemail> result = new ObservableCollection<Popemail>();
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null)
                return result;
            string sql = "SELECT Email.ID, ID_AdressFrom, Subject, DateSend, AdressBook.Email, AdressBook.Title FROM Email, AdressBook where ID_AdressTo = " + ActiveUser.Instance.GetUser().IDAddress + " AND ID_AdressFrom = AdressBook.ID";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pOPEmail = new Popemail();
                    pOPEmail.ID = reader.GetInt32("id");
                    pOPEmail.ID_AdressFrom = reader.GetInt32("ID_AdressFrom");
                    pOPEmail.ID_AdressTo = reader.GetInt32("ID_AdressTo");
                    pOPEmail.Subject = reader.GetString("Subject");
                    pOPEmail.Body = reader.GetString("Body");
                    pOPEmail.DateSend = reader.GetDateTime("DateSend");
                    pOPEmail.EmailFrom = reader.GetString("Email");
                    pOPEmail.TitleFrom = reader.GetString("Title");
                    result.Add(pOPEmail);
                }
            }
            return result;
        }
        internal Popemail AddPOPEmail(Popemail pOPEmail)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null) return pOPEmail;
            string sql = "select id from AdressBook where Email = '" + pOPEmail.ID_AdressFrom + "'";
            using (var mc = new MySqlCommand(sql, connect))
            using (var reader = mc.ExecuteReader())
            {
                if (reader.Read())
                    pOPEmail.ID_AdressFrom = reader.GetInt32(0);
            }
            if (pOPEmail.ID_AdressFrom == 0)
            {
                pOPEmail.ID_AdressFrom = MySqlDB.Instance.GetAutoID("AdressBook");
                sql = "INSERT INTO AdressBook VALUES (0, @AdressFrom, '', null)";
                using (var mc = new MySqlCommand(sql, connect))
                {
                    mc.Parameters.Add(new MySqlParameter("AdressFrom", pOPEmail.ID_AdressFrom));
                    mc.ExecuteNonQuery();
                }
            }
            int id = MySqlDB.Instance.GetAutoID("Email");
            sql = "INSERT INTO Email VALUES (0, @id_AdressFrom, @id_AdressTo, @subject, @body, @datesend)";
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("id_AdressFrom", pOPEmail.ID_AdressFrom));
                mc.Parameters.Add(new MySqlParameter("id_AdressTo", pOPEmail.ID_AdressTo));
                mc.Parameters.Add(new MySqlParameter("subject", pOPEmail.Subject));
                mc.Parameters.Add(new MySqlParameter("body", pOPEmail.Body));
                mc.Parameters.Add(new MySqlParameter("datesend", pOPEmail.DateSend));
                mc.ExecuteNonQuery();
            }
            pOPEmail.ID = id;
            return pOPEmail;
        }
        //internal Popemail GetDelPOPEmail(Popemail pOPEmail)
        //{
        //    var connect = MySqlDB.Instance.GetConnection();
        //    if (connect == null) return pOPEmail;
        //    string sql = "select  FROM Email WHERE id = ' " + pOPEmail.ID + "';";
        //    using (var mc = new MySqlCommand(sql, connect))
        //        mc.ExecuteNonQuery();
        //    return pOPEmail;
        //}

        internal Popemail RemovePOPEmail(Popemail pOPEmail)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null) return pOPEmail;
            string sql = "DELETE FROM Email WHERE id = ' " + pOPEmail.ID + "';";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
            return pOPEmail;
        }

        internal IEnumerable<Popemail> Search(string searchText, Popemail pOPEmail)
        {
            string sql = "select  e.ID_AdressFrom, e.Subject, e.Body, e.DateSend from Email e";
            sql += " AND (e.ID_AdressFrom LIKE '%" + searchText + "%'";
            sql += " OR e.Subject LIKE '%" + searchText + "%') order by e.id";
            if (pOPEmail.ID != 0)
            {
                var result = GetAllPOPEmails().Where(s => s.AdressBooks.FirstOrDefault(s => s.ID == pOPEmail.ID) != null);
                return result;
            }
            return GetAllPOPEmails();
        }

        internal void UpdatePOPEmail(Popemail pOPEmail)
        {
            var connect = MySqlDB.Instance.GetConnection();
            if (connect == null) return;
            string sql = "delete from Email";
            using (var mc = new MySqlCommand(sql, connect))
                mc.ExecuteNonQuery();
            sql = "update Email set ID_AdressFrom = @id_AdressFrom, ID_AdressTo = @id_AdressTo, Subject = @subject, Body =  @body, DateSend =  @datesend where ID = " + pOPEmail.ID;
            using (var mc = new MySqlCommand(sql, connect))
            {
                mc.Parameters.Add(new MySqlParameter("subject", pOPEmail.Subject));
                mc.Parameters.Add(new MySqlParameter("body", pOPEmail.Body));
                mc.Parameters.Add(new MySqlParameter("datesent", pOPEmail.DateSend));
                mc.ExecuteNonQuery();
            }
        }

    }
}

