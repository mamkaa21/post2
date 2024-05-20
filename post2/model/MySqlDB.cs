using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace post2.model
{
    public class MySqlDB
    {
        MySqlConnection mySqlConnection;
        private MySqlDB()
        {
            MySqlConnectionStringBuilder stringBuilder = new();
            stringBuilder.UserID = "root";
            stringBuilder.Password = "kimmik89";
            stringBuilder.Database = "postalina1125";
            stringBuilder.Server = "127.0.0.1";
            stringBuilder.CharacterSet = "utf8mb4";
            mySqlConnection = new MySqlConnection(stringBuilder.ToString());
            OpenConnection();
        }
        private bool OpenConnection()
        {
            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void CloseConnection()
        {
            try
            {
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal MySqlConnection GetConnection()
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                if (!OpenConnection())
                    return null;

            return mySqlConnection;
        }

        static MySqlDB instance;
        public static MySqlDB Instance
        {
            get
            {
                if (instance == null)
                    instance = new MySqlDB();
                return instance;
            }
        }

        public int GetAutoID(string table)
        {
            try
            {
                string sql = "SHOW Table STATUS WHERE Name = '" + table + "'";
                using (var mc = new MySqlCommand(sql, mySqlConnection))
                using (var reader = mc.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32("Auto_increment");
                }
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
    }
}
