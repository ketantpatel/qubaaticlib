using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MDACLib
{
    public class ImportCiteis
    {
        MySqlCommand _Command = new MySqlCommand();
        MySqlConnection _Connection = new MySqlConnection();
        SqlConnectionStringBuilder _ConnectionString = new SqlConnectionStringBuilder();
        public ImportCiteis()
        {
           
            _ConnectionString.ConnectionString = "Server=localhost;Database=mysql_qubaatic;Uid=root;Pwd=";
            _Connection.ConnectionString = _ConnectionString.ConnectionString;
            _Connection.Open();
        }
        public void addValue(string query)
        {
            try
            {
                _Command.Connection = _Connection;
                _Command.CommandText = query;
                _Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _Command.CommandText = "insert into import_logs(msg) values("+query+")";
                _Command.ExecuteNonQuery();
            }
        }

        public void closeConnection()
        {
            _Command.Dispose();
            _Connection.Close();
        }
    }
}
