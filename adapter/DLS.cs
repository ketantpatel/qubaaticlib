using MDACLib.beans;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MDACLib
{
    public class DLS: IDisposable
    {
        private MySqlCommand _Command = new MySqlCommand();
        private MySqlCommand _Command2 = new MySqlCommand();
        private MySqlConnection _Connection = new MySqlConnection();
        private SqlConnectionStringBuilder _ConnectionString = new SqlConnectionStringBuilder();
        private string _DBPathWithName = "";
        private bool _disposed;
        private string _LastError = "";
        private string _LastSQL = "";
        private MySqlParameter _Parameter = new MySqlParameter();
        private MySqlTransaction _Transaction;
        public UserBean user;

        //ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DLS(UserBean user)
        {
            this.user = user;
            Connect();
        }

        public void AddParameters(string FieldName, object Value, MyDBTypes Datatype)
        {
            _Command.Parameters.AddWithValue("@" + FieldName, Value);
            if (((((Datatype.ToString() == "Bit") || (Datatype.ToString() == "Int") ||
                   (Datatype.ToString() == "Numeric")) ||
                  ((Datatype.ToString() == "SmallInt") || (Datatype.ToString() == "Double"))) ||
                 (Datatype.ToString() == "Float")) || (Datatype.ToString() == "Decimal"))
            {
                if ((Value == null) || (Value.ToString() == ""))
                {
                    _Command.Parameters["@" + FieldName].Value = 0;
                }
                else
                {
                    _Command.Parameters["@" + FieldName].Value = Value;
                }
            }
            else
            {
                _Command.Parameters["@" + FieldName].Value = Value;
            }
        }

        public void BeginTransaction()
        {
            _Transaction = _Connection.BeginTransaction();
            _Command.Transaction = _Transaction;
        }

        public void CommitTransaction()
        {
            if (_Transaction != null)
            {
                _Transaction.Commit();
                _Transaction.Dispose();
                _Command.Parameters.Clear();
            }
        }

        public bool Connect()
        {
            _ConnectionString.ConnectionString = "Server=" + user.db_ip + ";Database=" + user.db_name + ";Uid=" + user.db_user + ";Pwd=" + user.db_pass + "";
            //_ConnectionString.ConnectionString ="Data Source="+user.db_ip+";Initial Catalog="+user.db_name+";Persist Security Info=True;User ID="+user.db_user+";Password="+user.db_pass+";connection timeout=0;Max Pool Size = 100;Pooling=false";
            
            _Connection.ConnectionString = _ConnectionString.ConnectionString;
            //this._Connection1.ConnectionString = this._ConnectionString.ConnectionString;
            //this._Connection2.ConnectionString = this._ConnectionString.ConnectionString;
            //this._Connection3.ConnectionString = this._ConnectionString.ConnectionString;
            //this._Connection4.ConnectionString = this._ConnectionString.ConnectionString;

            if (_Connection.State == ConnectionState.Open)
            {
                _Connection.Close();
                //this._Connection1.Close();
                //this._Connection2.Close();
                //this._Connection3.Close();
                //this._Connection4.Close();
            }
            try
            {
                _Connection.Open();
                //this._Connection1.Open();
                //this._Connection2.Open();
                //this._Connection3.Open();
                //this._Connection4.Open();
                _Command.Connection = _Connection;
                //this._Command2.Connection = this._Connection2;
                return true;
            }
            catch (Exception exception)
            {
                LastError = "error: '" + exception.Message + "'\nError open connection.\nDataBase: '" +
                            _ConnectionString.DataSource + "'";
                user.message = LastError;
                return false;
            }
        }

        public bool Delete(string _TableName, string WhereCondition)
        {
            try
            {
                _Command.CommandText = "Delete From " + _TableName;
                if (WhereCondition != "")
                {
                    _Command.CommandText = _Command.CommandText + " Where " + WhereCondition;
                }
                _Command.ExecuteNonQuery();
                LastSQL = _Command.CommandText;
                return true;
            }
            catch (Exception exception)
            {
                LastError = "error: '" + exception.Message;
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_Command != null)
                    {
                        _Command.Dispose();
                    }
                    if (_Connection != null)
                    {

                        if (_Connection.State == ConnectionState.Open)
                        {
                            _Connection.Close();
                        }
                        // SqlConnection.ClearPool(_Connection);
                        _Connection.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        ~DLS()
        {
            Dispose(false);
        }

        public DataSet GetDataSet(string strTable, string strFields, string strWhere, string strGroup, string strOrder,
                                  string strDataSetName)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string selectCommandText;
                if (!string.IsNullOrEmpty(strFields))
                {
                    selectCommandText = "SELECT " + strFields + " FROM " + strTable;
                }
                else
                {
                    selectCommandText = "SELECT * FROM " + strTable;
                }

                if (!string.IsNullOrEmpty(strWhere))
                {
                    selectCommandText = selectCommandText + " WHERE " + strWhere;
                }

                if (!string.IsNullOrEmpty(strGroup))
                {
                    selectCommandText = selectCommandText + " GROUP BY " + strGroup;
                }

                if (!string.IsNullOrEmpty(strOrder))
                {
                    selectCommandText = selectCommandText + " ORDER BY " + strOrder;
                }

                new MySqlDataAdapter(selectCommandText, _Connection).Fill(dataSet, strDataSetName);
                LastSQL = selectCommandText;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
            }
            return dataSet;
        }

        public DataSet GetDataSet(string sql)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string selectCommandText = sql;

                new MySqlDataAdapter(selectCommandText, _Connection).Fill(dataSet);
                LastSQL = selectCommandText;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
            }
            return dataSet;
        }


        /// <summary>
        /// 	Gets the data table.
        /// </summary>
        /// <param name = "strTable">The STR table.</param>
        /// <param name = "strFields">The STR fields.</param>
        /// <param name = "strWhere">The STR where.</param>
        /// <param name = "strGroup">The STR group.</param>
        /// <param name = "strOrder">The STR order.</param>
        /// <returns></returns>
        public DataTable GetDataTable(string strTable, string strFields, string strWhere, string strGroup,
                                      string strOrder)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string selectCommandText;
                if (!string.IsNullOrEmpty(strFields))
                {
                    selectCommandText = "SELECT " + strFields + " FROM " + strTable;
                }
                else
                {
                    selectCommandText = "SELECT * FROM " + strTable;
                }
                if (!string.IsNullOrEmpty(strWhere))
                {
                    selectCommandText = selectCommandText + " WHERE " + strWhere;
                }
                if (!string.IsNullOrEmpty(strGroup))
                {
                    selectCommandText = selectCommandText + " GROUP BY " + strGroup;
                }
                if (!string.IsNullOrEmpty(strOrder))
                {
                    selectCommandText = selectCommandText + " ORDER BY " + strOrder;
                }
                new MySqlDataAdapter(selectCommandText, _Connection).Fill(dataTable);
                LastSQL = selectCommandText;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
            }
            return dataTable;
        }

        public DataTable GetDataTable(string query)
        {
            DataTable tb = new DataTable();
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _Connection);
                adapter.Fill(tb);
                //log.Info("Executing Query : " + query);
                return tb;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                //log.Error("Error executing Query : " + query + "\n Error : " + exception.Message);
                return null;
            }
        }

        public DataTable GetDataTable()
        {
            DataTable tb = new DataTable();
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(MyCommand.CommandText, _Connection);
                adapter.Fill(tb);
                //log.Info("Executing Query : " + query);
                return tb;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                //log.Error("Error executing Query : " + query + "\n Error : " + exception.Message);
                return null;
            }
        }

        public DataRow GetSingleDataRow(string query)
        {
            DataTable tb = new DataTable();
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, _Connection);
                adapter.Fill(tb);
                //log.Info("Executing Query : " + query);
                return tb.Rows[0];
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                //log.Error("Error executing Query : " + query + "\n Error : " + exception.Message);
                return null;
            }
        }


        /// <summary>
        /// 	Return Attendance for an employee
        /// </summary>
        /// <param name = "roleId">empid</param>
        /// <param name = "moduleId">parent module id</param>
        /// <returns>datatable filled with menus</returns>
        public DataTable GetAuthenticatedMenu(int roleId, int moduleId)
        {
            _Command.Parameters.AddWithValue("@role_Id", roleId);
            _Command.Parameters.AddWithValue("@module_id", moduleId);
            _Command.CommandText = "[GetEmployeeAttendance]";
            _Command.CommandType = CommandType.StoredProcedure;
            MySqlDataAdapter da = new MySqlDataAdapter(_Command);
            DataTable tb = new DataTable();
            da.Fill(tb);
            return tb;
        }

        /// <summary>
        /// Get an employee attendance for the month
        /// </summary>
        /// <param name="empid">Employee code</param>
        /// <param name="mnth">Month</param>
        /// <param name="year">Year</param>
        /// <returns>Datatable</returns>
        public DataTable GetEmployeeAttendance(int empid, int mnth, int year)
        {
            _Command.Parameters.AddWithValue("@empid", empid);
            _Command.Parameters.AddWithValue("@mnth", mnth);
            _Command.Parameters.AddWithValue("@yr", year);
            _Command.CommandText = "[GetEmployeeAttendance]";
            _Command.CommandType = CommandType.StoredProcedure;
            MySqlDataAdapter da = new MySqlDataAdapter(_Command);
            DataTable tb = new DataTable();
            da.Fill(tb);
            _Command.Parameters.Clear();
            return tb;
        }

        /// <summary>
        /// Get an employee attendance for the month
        /// </summary>
        /// <param name="empid">Employee code</param>
        /// <param name="mnth">Month</param>
        /// <param name="year">Year</param>
        /// <returns>Datatable</returns>
        public DataTable GetEmployeeAttendanceD(int empid, int mnth, int year)
        {
            _Command.Parameters.AddWithValue("@empid", empid);
            _Command.Parameters.AddWithValue("@mnth", mnth);
            _Command.Parameters.AddWithValue("@year", year);
            _Command.CommandText = "[EmpAttendanceDetails]";
            _Command.CommandType = CommandType.StoredProcedure;
            MySqlDataAdapter da = new MySqlDataAdapter(_Command);
            DataTable tb = new DataTable();
            da.Fill(tb);
            _Command.Parameters.Clear();
            return tb;
        }


        /// <summary>
        /// 	Return SqldataReader based on parameters
        /// </summary>
        /// <param name = "strTable">Table Name from which data should be fetched</param>
        /// <param name = "strFields">Fields name saparated with comma(,)</param>
        /// <param name = "strWhere">Filter condition</param>
        /// <param name = "strGroup">Grouping condition</param>
        /// <param name = "strOrder">Datasort order</param>
        /// <returns>SqlDataReader object</returns>
        /// <see cref = "http://msdn.microsoft.com/en-us/library/haa3afyz.aspx" />
        public MySqlDataReader GetDataReader(string strTable, string strFields, string strWhere, string strGroup,
                                           string strOrder)
        {
            MySqlDataReader dr = null;
            try
            {
                string selectCommandText;
                if (!string.IsNullOrEmpty(strFields))
                {
                    selectCommandText = "SELECT " + strFields + " FROM " + strTable;
                }
                else
                {
                    selectCommandText = "SELECT * FROM " + strTable;
                }
                if (!string.IsNullOrEmpty(strWhere))
                {
                    selectCommandText = selectCommandText + " WHERE " + strWhere;
                }
                if (!string.IsNullOrEmpty(strGroup))
                {
                    selectCommandText = selectCommandText + " GROUP BY " + strGroup;
                }
                if (!string.IsNullOrEmpty(strOrder))
                {
                    selectCommandText = selectCommandText + " ORDER BY " + strOrder;
                }
                MyCommand.CommandText = selectCommandText;
                MyCommand.Connection = _Connection;
                dr = MyCommand.ExecuteReader();
                LastSQL = selectCommandText;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
            }
            return dr;
        }

        /// <summary>
        /// 	Return SqldataReader based on parameters
        /// </summary>
        /// <returns>SqlDataReader object</returns>
        /// <see cref = "http://msdn.microsoft.com/en-us/library/haa3afyz.aspx" />
        public MySqlDataReader GetDataReader(string sql)
        {
            string selectCommandText = sql;
            MySqlDataReader dr = null;
            try
            {
                MyCommand.CommandText = selectCommandText;
                MyCommand.Connection = _Connection;
                dr = MyCommand.ExecuteReader();
                LastSQL = selectCommandText;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
            }
            return dr;
        }

        public string GetSingleValue(string TableName, string FieldName, string WhereCondition, string OrderBy)
        {
            string str;
            if (WhereCondition == "")
            {
                str = "SELECT TOP 1 " + FieldName + " FROM " + TableName;
            }
            else
            {
                str = "SELECT TOP 1 " + FieldName + " FROM " + TableName + " where " + WhereCondition;
            }
            if (OrderBy.Trim() != "")
            {
                str = str + " Order by " + OrderBy;
            }
            _Command.CommandText = str;
            LastSQL = str;
            try
            {
                object obj2 = _Command.ExecuteScalar();
                string str2 = "";
                if (obj2 != null)
                {
                    str2 = obj2.ToString();
                }
                return str2;
            }
            catch (Exception exception)
            {
                LastError = "error: '" + exception.Message;
                return "Error";
            }
        }

        public string GetSingleValue(string sql)
        {

            _Command.CommandText = sql;
            LastSQL = sql;
            try
            {
                _Command.CommandType = CommandType.Text;
                object obj2 = _Command.ExecuteScalar();
                string str2 = "";
                if (obj2 != null)
                {
                    str2 = obj2.ToString();
                }
                return str2;
            }
            catch (Exception exception)
            {
                LastError = "error: '" + exception.Message;
                return "Error";
            }
        }

        public int GetLastAutoID()
        {
            const string str = "SELECT @@IDENTITY";
            _Command.CommandText = str;
            LastSQL = str;
            try
            {
                object obj2 = _Command.ExecuteScalar();
                string str2 = obj2 != null ? obj2.ToString() : "0";
                return int.Parse(str2);
            }
            catch (Exception exception)
            {
                LastError = "error: '" + exception.Message;
                return 0;
            }
        }

        public string GetSingleValueUsingTransaction(string TableName, string FieldName, string WhereCondition,
                                                     string OrderBy)
        {
            string str;
            SqlCommand command = new SqlCommand();
            SqlConnection connection = new SqlConnection(MyConnectionString.ConnectionString);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            if (WhereCondition == "")
            {
                str = "SELECT TOP 1 " + FieldName + " FROM " + TableName;
            }
            else
            {
                str = "SELECT TOP 1 " + FieldName + " FROM " + TableName + " where " + WhereCondition;
            }
            if (OrderBy.Trim() != "")
            {
                str = str + " Order by " + OrderBy;
            }
            command.CommandText = str;
            LastSQL = str;
            try
            {
                object obj2 = command.ExecuteScalar();
                string str2 = "";
                if (obj2 != null)
                {
                    str2 = obj2.ToString();
                }
                transaction.Commit();
                return str2;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                LastError = "error: '" + exception.Message;
                return "Error";
            }
        }

        public ArrayList GetValues(string TableName, string FieldName, string WhereCondition, string OrderBy,
                                   bool Distinct, bool DecriptData)
        {
            ArrayList list = new ArrayList();
            if (OrderBy.Trim() != "")
            {
                OrderBy = " Order by " + OrderBy;
            }
            if (WhereCondition.Trim() != "")
            {
                WhereCondition = " WHERE " + WhereCondition;
            }
            string str2 = "";
            if (Distinct)
            {
                str2 = " DISTINCT ";
            }
            string str = "SELECT " + str2 + FieldName + " FROM " + TableName + " " + WhereCondition + " " + OrderBy;
            _Command.CommandText = str;
            _Command.CommandType = CommandType.Text;
            LastSQL = str;
            try
            {
                MySqlDataReader reader = _Command.ExecuteReader();
                while (reader.Read())
                {
                    if (DecriptData)
                    {
                        //list.Add(clsFunctions.Decryption(reader[FieldName].ToString()));
                    }
                    else
                    {
                        list.Add(reader[FieldName].ToString());
                    }
                }
                reader.Close();
            }
            catch (SqlException exception)
            {
                LastError = exception.Message;
            }
            return list;
        }

        public List<string> GetValues(string TableName, string FieldName, string WhereCondition, string OrderBy)
        {
            List<string> list = new List<string>();
            if (OrderBy.Trim() != "")
            {
                OrderBy = " Order by " + OrderBy;
            }
            if (WhereCondition.Trim() != "")
            {
                WhereCondition = " WHERE " + WhereCondition;
            }
            string str2 = "";
            string str = "SELECT " + str2 + FieldName + " FROM " + TableName + " " + WhereCondition + " " + OrderBy;
            _Command.CommandText = str;
            _Command.CommandType = CommandType.Text;
            LastSQL = str;
            try
            {
                MySqlDataReader reader = _Command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[FieldName].ToString());
                }
                reader.Close();
            }
            catch (SqlException exception)
            {
                LastError = exception.Message;
            }
            return list;
        }

        public List<string> GetValues(string sql, string FieldName)
        {
            List<string> list = new List<string>();
            string str = sql;
            _Command.CommandText = str;
            _Command.CommandType = CommandType.Text;
            LastSQL = str;
            try
            {
                MySqlDataReader reader = _Command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader[FieldName].ToString());
                }
                reader.Close();
            }
            catch (SqlException exception)
            {
                LastError = exception.Message;
            }
            return list;
        }

        public bool Insert(string _TableName)
        {
            try
            {
                string str = "INSERT INTO " + _TableName + " (";
                string str2 = "";
                string str3 = "";
                for (int i = 0; i <= (_Command.Parameters.Count - 1); i++)
                {
                    string str4 = (_Command.Parameters[i].ParameterName + ", ").Remove(0, 1);
                    str2 = str2 + str4;
                    str3 = str3 + _Command.Parameters[i].ParameterName + ", ";
                }
                str = (str + str2.Remove(str2.Length - 2, 2)) + ") VALUES( " + str3.Remove(str3.Length - 2, 2) + ")";
                _Command.CommandText = str;
                _Command.ExecuteNonQuery();
                LastSQL = str;
                _Command.Parameters.Clear();
                return true;
            }
            catch (Exception exception)
            {
                LastError = "error: '" + exception.Message;
                _Command.Parameters.Clear();
                return false;
            }
        }

        public void RollbackTransaction()
        {
            if (_Transaction != null)
            {
                _Transaction.Rollback();
                _Transaction.Dispose();
                _Command.Parameters.Clear();
            }
        }

        public bool SetValue(string TableName, string FieldName, object Value, string WhereCondition)
        {
            string str;
            if (WhereCondition == "")
            {
                str = string.Concat(new[] {"UPDATE ", TableName, " SET ", FieldName, "=", Value});
            }
            else
            {
                str =
                    string.Concat(new[]
                                      {"UPDATE ", TableName, " SET ", FieldName, "=", Value, " WHERE ", WhereCondition});
            }
            _Command.CommandText = str;
            LastSQL = str;
            try
            {
                _Command.ExecuteScalar();
                return true;
            }
            catch (SqlException exception)
            {
                LastError = "error: '" + exception.Message;
                return false;
            }
        }

        public bool Update(string _TableName, string WhereCondition)
        {
            try
            {
                string str = "Update " + _TableName + " set ";
                for (int i = 0; i <= (_Command.Parameters.Count - 1); i++)
                {
                    string str2 = _Command.Parameters[i].ParameterName.Remove(0, 1);
                    string str3 = str;
                    str = str3 + str2 + "=" + _Command.Parameters[i].ParameterName + ", ";
                }
                str = str.Remove(str.Length - 2, 2);
                if (WhereCondition != "")
                {
                    str = str + " Where " + WhereCondition;
                }
                _Command.CommandText = str;
                _Command.ExecuteNonQuery();
                LastSQL = str;
                _Command.Parameters.Clear();
                return true;
            }
            catch (Exception exception)
            {
                LastError = "error: '" + exception.Message;
                return false;
            }
        }

        public bool ValueExists(string TableName, string FieldName, string WhereCondition, string OrderBy)
        {
            string str;
            if (WhereCondition == "")
            {
                str = "SELECT TOP 1 " + FieldName + " FROM " + TableName;
            }
            else
            {
                str = "SELECT TOP 1 " + FieldName + " FROM " + TableName + " where " + WhereCondition;
            }
            if (OrderBy.Trim() != "")
            {
                str = str + " Order by " + OrderBy;
            }
            _Command.CommandText = str;
            try
            {
                object obj2 = _Command.ExecuteScalar();
                LastSQL = str;
                if (obj2 == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                return false;
            }
        }

        public string DBPathWithName
        {
            get { return _DBPathWithName; }
            set { _DBPathWithName = value; }
        }

        protected bool Disposed
        {
            get { return _disposed; }
            set { _disposed = value; }
        }

        public string LastError
        {
            get { return _LastError; }
            set { _LastError = value; }
        }

        public string LastSQL
        {
            get { return _LastSQL; }
            set { _LastSQL = value; }
        }

        public MySqlCommand MyCommand
        {
            get { return _Command; }
            set { _Command = value; }
        }

        public MySqlCommand MyCommand2
        {
            get { return _Command2; }
            set { _Command2 = value; }
        }

        public MySqlConnection MyConnection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }

        //public SqlConnection MyConnection1
        //{
        //    get
        //    {
        //        return this._Connection1;
        //    }
        //    set
        //    {
        //        this._Connection1 = value;
        //    }
        //}

        //public SqlConnection MyConnection2
        //{
        //    get
        //    {
        //        return this._Connection2;
        //    }
        //    set
        //    {
        //        this._Connection2 = value;
        //    }
        //}

        //public SqlConnection MyConnection3
        //{
        //    get
        //    {
        //        return this._Connection3;
        //    }
        //    set
        //    {
        //        this._Connection3 = value;
        //    }
        //}

        //public SqlConnection MyConnection4
        //{
        //    get
        //    {
        //        return this._Connection4;
        //    }
        //    set
        //    {
        //        this._Connection4 = value;
        //    }
        //}

        public SqlConnectionStringBuilder MyConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        public MySqlParameter MyParameters
        {
            get { return _Parameter; }
            set { _Parameter = value; }
        }

        public MySqlTransaction MyTransaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }
        }
       
    }

    public enum MyDBTypes
    {
        Bit,
        BigInt,
        Char,
        DateTime,
        Int,
        Image,
        Numeric,
        Nvarchar,
        SmallInt,
        TinyInt,
        Varchar
    }
}