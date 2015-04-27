using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.Configuration;
using System.Data.Common;

namespace HYUtility
{
    /// <summary> 
    /// SQLite数据库操作帮助类
    /// 提供一系列方便的调用:
    /// Execute,Save,Update,Delete...
    /// 
    /// 不是线程安全的
    /// 
    /// @author pcenshao
    /// </summary>
    public class SQLiteHelper1
    {

        private bool _showSql = true;

        /// <summary>
        /// 是否输出生成的SQL语句
        /// </summary>
        public bool ShowSql
        {
            get
            {
                return this._showSql;
            }
            set
            {
                this._showSql = value;
            }
        }

        private readonly string _dataFile;

        private SQLiteConnection _conn;

        public SQLiteHelper1(string dataFile)
        {
            if (dataFile == null)
                throw new ArgumentNullException("dataFile=null");
            this._dataFile = dataFile;
        }

        /// <summary>
        /// <para>打开SQLiteManager使用的数据库连接</para>
        /// </summary>
        public void Open()
        {
            this._conn = OpenConnection(this._dataFile);
        }

        public void Close()
        {
            if (this._conn != null)
            {
                this._conn.Close();
            }
        }

        /// <summary>
        /// <para>安静地关闭连接,保存不抛出任何异常</para>
        /// </summary>
        public void CloseQuietly()
        {
            if (this._conn != null)
            {
                try
                {
                    this._conn.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// <para>创建一个连接到指定数据文件的SQLiteConnection,并Open</para>
        /// <para>如果文件不存在,创建之</para>
        /// </summary>
        /// <param name="dataFile"></param>
        /// <returns></returns>
        public static SQLiteConnection OpenConnection(string dataFile)
        {
            if (dataFile == null)
                throw new ArgumentNullException("dataFile=null");

            if (!File.Exists(dataFile))
            {
                SQLiteConnection.CreateFile(dataFile);
            }

            SQLiteConnection conn = new SQLiteConnection();
            SQLiteConnectionStringBuilder conStr = new SQLiteConnectionStringBuilder();
            conStr.DataSource = dataFile;
            conn.ConnectionString = conStr.ToString();
            conn.Open();
            return conn;
        }

        /// <summary>
        /// <para>读取或设置SQLiteManager使用的数据库连接</para>
        /// </summary>
        public SQLiteConnection Connection
        {
            get
            {
                return this._conn;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this._conn = value;
            }
        }

        protected void EnsureConnection()
        {
            if (this._conn == null)
            {
                throw new Exception("SQLiteManager.Connection=null");
            }
        }

        public string GetDataFile()
        {
            return this._dataFile;
        }

        /// <summary>
        /// <para>判断表table是否存在</para>
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool TableExists(string table)
        {
            if (table == null)
                throw new ArgumentNullException("table=null");
            this.EnsureConnection();
            // SELECT count(*) FROM sqlite_master WHERE type='table' AND name='test';
            SQLiteCommand cmd = new SQLiteCommand("SELECT count(*) as c FROM sqlite_master WHERE type='table' AND name=@tableName ");
            cmd.Connection = this.Connection;
            cmd.Parameters.Add(new SQLiteParameter("tableName", table));
            SQLiteDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int c = reader.GetInt32(0);
            reader.Close();
            reader.Dispose();
            cmd.Dispose();
            //return false;
            return c == 1;
        }

        /// <summary>
        /// <para>执行SQL,返回受影响的行数</para>
        /// <para>可用于执行表创建语句</para>
        /// <para>paramArr == null 表示无参数</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, SQLiteParameter[] paramArr)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql=null");
            }
            this.EnsureConnection();

            if (this.ShowSql)
            {
                Console.WriteLine("SQL: " + sql);
            }

            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandText = sql;
            if (paramArr != null)
            {
                foreach (SQLiteParameter p in paramArr)
                {
                    cmd.Parameters.Add(p);
                }
            }
            cmd.Connection = this.Connection;
            int c = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return c;
        }

        /// <summary>
        /// <para>执行SQL,返回SQLiteDataReader</para>
        /// <para>返回的Reader为原始状态,须自行调用Read()方法</para>
        /// <para>paramArr=null,则表示无参数</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public SQLiteDataReader ExecuteReader(string sql, SQLiteParameter[] paramArr)
        {
            return (SQLiteDataReader)ExecuteReader(sql, paramArr, (ReaderWrapper)null);
        }

        /// <summary>
        /// <para>执行SQL,如果readerWrapper!=null,那么将调用readerWrapper对SQLiteDataReader进行包装,并返回结果</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramArr">null 表示无参数</param>
        /// <param name="readerWrapper">null 直接返回SQLiteDataReader</param>
        /// <returns></returns>
        public object ExecuteReader(string sql, SQLiteParameter[] paramArr, ReaderWrapper readerWrapper)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql=null");
            }
            this.EnsureConnection();

            SQLiteCommand cmd = new SQLiteCommand(sql, this.Connection);
            if (paramArr != null)
            {
                foreach (SQLiteParameter p in paramArr)
                {
                    cmd.Parameters.Add(p);
                }
            }
            SQLiteDataReader reader = cmd.ExecuteReader();
            object result = null;
            if (readerWrapper != null)
            {
                result = readerWrapper(reader);
            }
            else
            {
                result = reader;
            }
            reader.Close();
            reader.Dispose();
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// <para>执行SQL,返回结果集,使用RowWrapper对每一行进行包装</para>
        /// <para>如果结果集为空,那么返回空List (List.Count=0)</para>
        /// <para>rowWrapper = null时,使用WrapRowToDictionary</para>
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramArr"></param>
        /// <param name="rowWrapper"></param>
        /// <returns></returns>
        public List<object> ExecuteRow(string sql, SQLiteParameter[] paramArr, RowWrapper rowWrapper)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql=null");
            }
            this.EnsureConnection();

            SQLiteCommand cmd = new SQLiteCommand(sql, this.Connection);
            if (paramArr != null)
            {
                foreach (SQLiteParameter p in paramArr)
                {
                    cmd.Parameters.Add(p);
                }
            }

            if (rowWrapper == null)
            {
                rowWrapper = new RowWrapper(SQLiteHelper1.WrapRowToDictionary);
            }

            SQLiteDataReader reader = cmd.ExecuteReader();
            List<object> result = new List<object>();
            if (reader.HasRows)
            {
                int rowNum = 0;
                while (reader.Read())
                {
                    object row = rowWrapper(rowNum, reader);
                    result.Add(row);
                    rowNum++;
                }
            }
            reader.Close();
            reader.Dispose();
            cmd.Dispose();
            return result;
        }

        public static object WrapRowToDictionary(int rowNum, SQLiteDataReader reader)
        {
            int fc = reader.FieldCount;
            Dictionary<string, object> row = new Dictionary<string, object>();
            for (int i = 0; i < fc; i++)
            {
                string fieldName = reader.GetName(i);
                object value = reader.GetValue(i);
                row.Add(fieldName, value);
            }
            return row;
        }

        /// <summary>
        /// <para>执行insert into语句</para>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Save(string table, Dictionary<string, object> entity)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table=null");
            }
            this.EnsureConnection();
            string sql = BuildInsert(table, entity);
            return this.ExecuteNonQuery(sql, BuildParamArray(entity));
        }

        private static SQLiteParameter[] BuildParamArray(Dictionary<string, object> entity)
        {
            List<SQLiteParameter> list = new List<SQLiteParameter>();
            foreach (string key in entity.Keys)
            {
                list.Add(new SQLiteParameter(key, entity[key]));
            }
            if (list.Count == 0)
                return null;
            return list.ToArray();
        }

        private static string BuildInsert(string table, Dictionary<string, object> entity)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("insert into ").Append(table);
            buf.Append(" (");
            foreach (string key in entity.Keys)
            {
                buf.Append(key).Append(",");
            }
            buf.Remove(buf.Length - 1, 1); // 移除最后一个,
            buf.Append(") ");
            buf.Append("values(");
            foreach (string key in entity.Keys)
            {
                buf.Append("@").Append(key).Append(","); // 创建一个参数
            }
            buf.Remove(buf.Length - 1, 1);
            buf.Append(") ");

            return buf.ToString();
        }

        private static string BuildUpdate(string table, Dictionary<string, object> entity)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("update ").Append(table).Append(" set ");
            foreach (string key in entity.Keys)
            {
                buf.Append(key).Append("=").Append("@").Append(key).Append(",");
            }
            buf.Remove(buf.Length - 1, 1);
            buf.Append(" ");
            return buf.ToString();
        }

        /// <summary>
        /// <para>执行update语句</para>
        /// <para>where参数不必要包含'where'关键字</para>
        /// 
        /// <para>如果where=null,那么忽略whereParams</para>
        /// <para>如果where!=null,whereParams=null,where部分无参数</para>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="entity"></param>
        /// <param name="where"></param>
        /// <param name="whereParams"></param>
        /// <returns></returns>
        public int Update(string table, Dictionary<string, object> entity, string where, SQLiteParameter[] whereParams)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table=null");
            }
            this.EnsureConnection();
            string sql = BuildUpdate(table, entity);
            SQLiteParameter[] arr = BuildParamArray(entity);
            if (where != null)
            {
                sql += " where " + where;
                if (whereParams != null)
                {
                    SQLiteParameter[] newArr = new SQLiteParameter[arr.Length + whereParams.Length];
                    Array.Copy(arr, newArr, arr.Length);
                    Array.Copy(whereParams, 0, newArr, arr.Length, whereParams.Length);

                    arr = newArr;
                }
            }
            return this.ExecuteNonQuery(sql, arr);
        }

        /// <summary>
        /// <para>查询一行记录,无结果时返回null</para>
        /// <para>conditionCol = null时将忽略条件,直接执行select * from table </para>
        /// </summary>
        /// <param name="table"></param>
        /// <param name="conditionCol"></param>
        /// <param name="conditionVal"></param>
        /// <returns></returns>
        public Dictionary<string, object> QueryOne(string table, string conditionCol, object conditionVal)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table=null");
            }
            this.EnsureConnection();

            string sql = "select * from " + table;
            if (conditionCol != null)
            {
                sql += " where " + conditionCol + "=@" + conditionCol;
            }
            if (this.ShowSql)
            {
                Console.WriteLine("SQL: " + sql);
            }

            List<object> list = this.ExecuteRow(sql, new SQLiteParameter[] { 
                new SQLiteParameter(conditionCol,conditionVal)
            }, null);
            if (list.Count == 0)
                return null;
            return (Dictionary<string, object>)list[0];
        }

        /// <summary>
        /// 执行delete from table 语句
        /// where不必包含'where'关键字
        /// where=null时将忽略whereParams
        /// </summary>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="whereParams"></param>
        /// <returns></returns>
        public int Delete(string table, string where, SQLiteParameter[] whereParams)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table=null");
            }
            this.EnsureConnection();
            string sql = "delete from " + table + " ";
            if (where != null)
            {
                sql += "where " + where;
            }

            return this.ExecuteNonQuery(sql, whereParams);
        }
    }

    /// <summary>
    /// 在SQLiteManager.Execute方法中回调,将SQLiteDataReader包装成object 
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public delegate object ReaderWrapper(SQLiteDataReader reader);

    /// <summary>
    /// 将SQLiteDataReader的行包装成object
    /// </summary>
    /// <param name="rowNum"></param>
    /// <param name="reader"></param>
    /// <returns></returns>
    public delegate object RowWrapper(int rowNum, SQLiteDataReader reader);







    ////使用TestDrivern.NET
    //using System;
    //using System.Collections.Generic;
    //using System.Linq;
    //using System.Text;

    //using System.Data.SQLite;

    //namespace SQLite {

    //    class Test {

    //        private SQLiteHelper _mgr;

    //        public Test() {
    //            this._mgr = new SQLiteHelper("sqlite.db");
    //            this._mgr.Open();
    //        }

    //        public void TestTableExists() {
    //            Console.WriteLine("表test是否存在: " + this._mgr.TableExists("test")); 
    //        }

    //        public void TestExecuteRow() {
    //            List<object> list = this._mgr.ExecuteRow("select * from test", null, null);
    //            foreach (object o in list) {
    //                Dictionary<string, object> d = (Dictionary<string, object>) o;
    //                foreach (string k in d.Keys) {
    //                    Console.Write(k + "=" + d[k] + ","); 
    //                }
    //                Console.WriteLine();
    //            }
    //        }

    //        public void TestSave() {
    //            Dictionary<string, object> entity = new Dictionary<string, object>();
    //            entity.Add("username", "u1");
    //            entity.Add("password", "p1");
    //            this._mgr.Save("test", entity);
    //        }

    //        public void TestUpdate() {
    //            Dictionary<string, object> entity = new Dictionary<string, object>();
    //            entity.Add("username", "u1");
    //            entity.Add("password", "123456");

    //            int c = this._mgr.Update("test", entity, "username=@username", new System.Data.SQLite.SQLiteParameter[] { 
    //                new SQLiteParameter("username","u1")
    //            });
    //            Console.WriteLine(c); 
    //        }

    //        public void TestQueryOne() {
    //             Dictionary<string, object> entity = this._mgr.QueryOne("test", "username", "a");
    //             foreach (string k in entity.Keys) {
    //                 Console.Write(k + "=" + entity[k] + ","); 
    //             }
    //        }

    //        public void TestDelete() {
    //            int  c = this._mgr.Delete("test", "username=@username", new SQLiteParameter[] {
    //                new SQLiteParameter("username","a")
    //            });
    //            Console.WriteLine("c=" + c); 
    //        }

    //        public static void Test0() { 
    //            Test t = new Test(); 
    //            t.TestTableExists(); 
    //            t.TestExecuteRow(); 

    //            //t.TestSave(); 
    //            //t.TestUpdate();  
    //           // t.TestQueryOne(); 
    //            t.TestDelete(); 
    //        }

    //    }


    /// <summary>
    /// SQLiteHelper is a utility class similar to "SQLHelper" in MS
    /// Data Access Application Block and follows similar pattern.
    /// </summary>
    public class SQLiteHelper2
    {
        /// <summary>
        /// Creates a new <see cref="SQLiteHelper"/> instance. The ctor is marked private since all members are static.
        /// </summary>
        private SQLiteHelper2()
        {
        }
        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="connection">Connection.</param>
        /// <param name="commandText">Command text.</param>
        /// <param name="commandParameters">Command parameters.</param>
        /// <returns>SQLite Command</returns>
        public static SQLiteCommand CreateCommand(SQLiteConnection connection, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteCommand cmd = new SQLiteCommand(commandText, connection);
            if (commandParameters.Length > 0)
            {
                foreach (SQLiteParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
            return cmd;
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="commandText">Command text.</param>
        /// <param name="commandParameters">Command parameters.</param>
        /// <returns>SQLite Command</returns>
        public static SQLiteCommand CreateCommand(string connectionString, string commandText, params SQLiteParameter[] commandParameters)
        {
            SQLiteConnection cn = new SQLiteConnection(connectionString);

            SQLiteCommand cmd = new SQLiteCommand(commandText, cn);

            if (commandParameters.Length > 0)
            {
                foreach (SQLiteParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
            return cmd;
        }
        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="parameterValue">Parameter value.</param>
        /// <returns>SQLiteParameter</returns>
        public static SQLiteParameter CreateParameter(string parameterName, System.Data.DbType parameterType, object parameterValue)
        {
            SQLiteParameter parameter = new SQLiteParameter();
            parameter.DbType = parameterType;
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            return parameter;
        }

        /// <summary>
        /// Shortcut method to execute dataset from SQL Statement and object[] arrray of parameter values
        /// </summary>
        /// <param name="connectionString">SQLite Connection string</param>
        /// <param name="commandText">SQL Statement with embedded "@param" style parameter names</param>
        /// <param name="paramList">object[] array of parameter values</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string connectionString, string commandText, object[] paramList)
        {
            SQLiteConnection cn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = cn.CreateCommand();


            cmd.CommandText = commandText;
            if (paramList != null)
            {
                AttachParameters(cmd, commandText, paramList);
            }
            DataSet ds = new DataSet();
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();
            cn.Close();
            return ds;
        }
        /// <summary>
        /// Shortcut method to execute dataset from SQL Statement and object[] arrray of  parameter values
        /// </summary>
        /// <param name="cn">Connection.</param>
        /// <param name="commandText">Command text.</param>
        /// <param name="paramList">Param list.</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(SQLiteConnection cn, string commandText, object[] paramList)
        {

            SQLiteCommand cmd = cn.CreateCommand();


            cmd.CommandText = commandText;
            if (paramList != null)
            {
                AttachParameters(cmd, commandText, paramList);
            }
            DataSet ds = new DataSet();
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(ds);
            da.Dispose();
            cmd.Dispose();
            cn.Close();
            return ds;
        }
        /// <summary>
        /// Executes the dataset from a populated Command object.
        /// </summary>
        /// <param name="cmd">Fully populated SQLiteCommand</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataset(SQLiteCommand cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            DataSet ds = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(ds);
            da.Dispose();
            cmd.Connection.Close();
            cmd.Dispose();
            return ds;
        }

        /// <summary>
        /// Executes the dataset in a SQLite Transaction
        /// </summary>
        /// <param name="transaction">SQLiteTransaction. Transaction consists of Connection, Transaction,  /// and Command, all of which must be created prior to making this method call. </param>
        /// <param name="commandText">Command text.</param>
        /// <param name="commandParameters">Sqlite Command parameters.</param>
        /// <returns>DataSet</returns>
        /// <remarks>user must examine Transaction Object and handle transaction.connection .Close, etc.</remarks>
        public static DataSet ExecuteDataset(SQLiteTransaction transaction, string commandText, params SQLiteParameter[] commandParameters)
        {

            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rolled back or committed, please provide an open transaction.", "transaction");
            IDbCommand cmd = transaction.Connection.CreateCommand();
            cmd.CommandText = commandText;
            foreach (SQLiteParameter parm in commandParameters)
            {
                cmd.Parameters.Add(parm);
            }
            if (transaction.Connection.State == ConnectionState.Closed)
                transaction.Connection.Open();
            DataSet ds = ExecuteDataset((SQLiteCommand)cmd);
            return ds;
        }

        /// <summary>
        /// Executes the dataset with Transaction and object array of parameter values.
        /// </summary>
        /// <param name="transaction">SQLiteTransaction. Transaction consists of Connection, Transaction,    /// and Command, all of which must be created prior to making this method call. </param>
        /// <param name="commandText">Command text.</param>
        /// <param name="commandParameters">object[] array of parameter values.</param>
        /// <returns>DataSet</returns>
        /// <remarks>user must examine Transaction Object and handle transaction.connection .Close, etc.</remarks>
        public static DataSet ExecuteDataset(SQLiteTransaction transaction, string commandText, object[] commandParameters)
        {

            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rolled back or committed,                                                          please provide an open transaction.", "transaction");
            IDbCommand cmd = transaction.Connection.CreateCommand();
            cmd.CommandText = commandText;
            AttachParameters((SQLiteCommand)cmd, cmd.CommandText, commandParameters);
            if (transaction.Connection.State == ConnectionState.Closed)
                transaction.Connection.Open();

            DataSet ds = ExecuteDataset((SQLiteCommand)cmd);
            return ds;
        }

        #region UpdateDataset
        /// <summary>
        /// Executes the respective command for each inserted, updated, or deleted row in the DataSet.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
        /// </remarks>
        /// <param name="insertCommand">A valid SQL statement  to insert new records into the data source</param>
        /// <param name="deleteCommand">A valid SQL statement to delete records from the data source</param>
        /// <param name="updateCommand">A valid SQL statement used to update records in the data source</param>
        /// <param name="dataSet">The DataSet used to update the data source</param>
        /// <param name="tableName">The DataTable used to update the data source.</param>
        public static void UpdateDataset(SQLiteCommand insertCommand, SQLiteCommand deleteCommand, SQLiteCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null) throw new ArgumentNullException("insertCommand");
            if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
            if (updateCommand == null) throw new ArgumentNullException("updateCommand");
            if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("tableName");

            // Create a SQLiteDataAdapter, and dispose of it after we are done
            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter())
            {
                // Set the data adapter commands
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // Update the dataset changes in the data source
                dataAdapter.Update(dataSet, tableName);

                // Commit all the changes made to the DataSet
                dataSet.AcceptChanges();
            }
        }
        #endregion




        /// <summary>
        /// ShortCut method to return IDataReader
        /// NOTE: You should explicitly close the Command.connection you passed in as
        /// well as call Dispose on the Command  after reader is closed.
        /// We do this because IDataReader has no underlying Connection Property.
        /// </summary>
        /// <param name="cmd">SQLiteCommand Object</param>
        /// <param name="commandText">SQL Statement with optional embedded "@param" style parameters</param>
        /// <param name="paramList">object[] array of parameter values</param>
        /// <returns>IDataReader</returns>
        public static IDataReader ExecuteReader(SQLiteCommand cmd, string commandText, object[] paramList)
        {
            if (cmd.Connection == null)
                throw new ArgumentException("Command must have live connection attached.", "cmd");
            cmd.CommandText = commandText;
            AttachParameters(cmd, commandText, paramList);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return rdr;
        }

        /// <summary>
        /// Shortcut to ExecuteNonQuery with SqlStatement and object[] param values
        /// </summary>
        /// <param name="connectionString">SQLite Connection String</param>
        /// <param name="commandText">Sql Statement with embedded "@param" style parameters</param>
        /// <param name="paramList">object[] array of parameter values</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string commandText, params object[] paramList)
        {
            SQLiteConnection cn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = cn.CreateCommand();
            cmd.CommandText = commandText;
            AttachParameters(cmd, commandText, paramList);
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            cn.Close();

            return result;
        }



        public static int ExecuteNonQuery(SQLiteConnection cn, string commandText, params  object[] paramList)
        {

            SQLiteCommand cmd = cn.CreateCommand();
            cmd.CommandText = commandText;
            AttachParameters(cmd, commandText, paramList);
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            cn.Close();

            return result;
        }

        /// <summary>
        /// Executes  non-query sql Statment with Transaction
        /// </summary>
        /// <param name="transaction">SQLiteTransaction. Transaction consists of Connection, Transaction,   /// and Command, all of which must be created prior to making this method call. </param>
        /// <param name="commandText">Command text.</param>
        /// <param name="paramList">Param list.</param>
        /// <returns>Integer</returns>
        /// <remarks>user must examine Transaction Object and handle transaction.connection .Close, etc.</remarks>
        public static int ExecuteNonQuery(SQLiteTransaction transaction, string commandText, params  object[] paramList)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rolled back or committed,                                                        please provide an open transaction.", "transaction");
            IDbCommand cmd = transaction.Connection.CreateCommand();
            cmd.CommandText = commandText;
            AttachParameters((SQLiteCommand)cmd, cmd.CommandText, paramList);
            if (transaction.Connection.State == ConnectionState.Closed)
                transaction.Connection.Open();
            int result = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return result;
        }


        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="cmd">CMD.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbCommand cmd)
        {
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            int result = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Shortcut to ExecuteScalar with Sql Statement embedded params and object[] param values
        /// </summary>
        /// <param name="connectionString">SQLite Connection String</param>
        /// <param name="commandText">SQL statment with embedded "@param" style parameters</param>
        /// <param name="paramList">object[] array of param values</param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, string commandText, params  object[] paramList)
        {
            SQLiteConnection cn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = cn.CreateCommand();
            cmd.CommandText = commandText;
            AttachParameters(cmd, commandText, paramList);
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            object result = cmd.ExecuteScalar();
            cmd.Dispose();
            cn.Close();

            return result;
        }

        /// <summary>
        /// Execute XmlReader with complete Command
        /// </summary>
        /// <param name="command">SQLite Command</param>
        /// <returns>XmlReader</returns>
        public static XmlReader ExecuteXmlReader(IDbCommand command)
        { // open the connection if necessary, but make sure we 
            // know to close it when we�re done.
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }

            // get a data adapter  
            SQLiteDataAdapter da = new SQLiteDataAdapter((SQLiteCommand)command);
            DataSet ds = new DataSet();
            // fill the data set, and return the schema information
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.Fill(ds);
            // convert our dataset to XML
            StringReader stream = new StringReader(ds.GetXml());
            command.Connection.Close();
            // convert our stream of text to an XmlReader
            return new XmlTextReader(stream);
        }



        /// <summary>
        /// Parses parameter names from SQL Statement, assigns values from object array ,   /// and returns fully populated ParameterCollection.
        /// </summary>
        /// <param name="commandText">Sql Statement with "@param" style embedded parameters</param>
        /// <param name="paramList">object[] array of parameter values</param>
        /// <returns>SQLiteParameterCollection</returns>
        /// <remarks>Status experimental. Regex appears to be handling most issues. Note that parameter object array must be in same ///order as parameter names appear in SQL statement.</remarks>
        private static SQLiteParameterCollection AttachParameters(SQLiteCommand cmd, string commandText, params  object[] paramList)
        {
            if (paramList == null || paramList.Length == 0) return null;

            SQLiteParameterCollection coll = cmd.Parameters;
            string parmString = commandText.Substring(commandText.IndexOf("@"));
            // pre-process the string so always at least 1 space after a comma.
            parmString = parmString.Replace(",", " ,");
            // get the named parameters into a match collection
            string pattern = @"(@)\S*(.*?)\b";
            Regex ex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = ex.Matches(parmString);
            string[] paramNames = new string[mc.Count];
            int i = 0;
            foreach (Match m in mc)
            {
                paramNames[i] = m.Value;
                i++;
            }

            // now let's type the parameters
            int j = 0;
            Type t = null;
            foreach (object o in paramList)
            {
                t = o.GetType();

                SQLiteParameter parm = new SQLiteParameter();
                switch (t.ToString())
                {

                    case ("DBNull"):
                    case ("Char"):
                    case ("SByte"):
                    case ("UInt16"):
                    case ("UInt32"):
                    case ("UInt64"):
                        throw new SystemException("Invalid data type");


                    case ("System.String"):
                        parm.DbType = DbType.String;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (string)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Byte[]"):
                        parm.DbType = DbType.Binary;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (byte[])paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Int32"):
                        parm.DbType = DbType.Int32;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (int)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.Boolean"):
                        parm.DbType = DbType.Boolean;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (bool)paramList[j];
                        coll.Add(parm);
                        break;

                    case ("System.DateTime"):
                        parm.DbType = DbType.DateTime;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDateTime(paramList[j]);
                        coll.Add(parm);
                        break;

                    case ("System.Double"):
                        parm.DbType = DbType.Double;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDouble(paramList[j]);
                        coll.Add(parm);
                        break;

                    case ("System.Decimal"):
                        parm.DbType = DbType.Decimal;
                        parm.ParameterName = paramNames[j];
                        parm.Value = Convert.ToDecimal(paramList[j]);
                        break;

                    case ("System.Guid"):
                        parm.DbType = DbType.Guid;
                        parm.ParameterName = paramNames[j];
                        parm.Value = (System.Guid)(paramList[j]);
                        break;

                    case ("System.Object"):

                        parm.DbType = DbType.Object;
                        parm.ParameterName = paramNames[j];
                        parm.Value = paramList[j];
                        coll.Add(parm);
                        break;

                    default:
                        throw new SystemException("Value is of unknown data type");

                } // end switch

                j++;
            }
            return coll;
        }

        /// <summary>
        /// Executes non query typed params from a DataRow
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="dataRow">Data row.</param>
        /// <returns>Integer result code</returns>
        public static int ExecuteNonQueryTypedParams(IDbCommand command, DataRow dataRow)
        {
            int retVal = 0;

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Set the parameters values
                AssignParameterValues(command.Parameters, dataRow);

                retVal = ExecuteNonQuery(command);
            }
            else
            {
                retVal = ExecuteNonQuery(command);
            }

            return retVal;
        }

        /// <summary>
        /// This method assigns dataRow column values to an IDataParameterCollection
        /// </summary>
        /// <param name="commandParameters">The IDataParameterCollection to be assigned values</param>
        /// <param name="dataRow">The dataRow used to hold the command's parameter values</param>
        /// <exception cref="System.InvalidOperationException">Thrown if any of the parameter names are invalid.</exception>
        protected internal static void AssignParameterValues(IDataParameterCollection commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
            {
                // Do nothing if we get no data
                return;
            }

            DataColumnCollection columns = dataRow.Table.Columns;

            int i = 0;
            // Set the parameters values
            foreach (IDataParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if (commandParameter.ParameterName == null ||
                 commandParameter.ParameterName.Length <= 1)
                    throw new InvalidOperationException(string.Format(
                           "Please provide a valid parameter name on the parameter #{0},                            the ParameterName property has the following value: '{1}'.",
                     i, commandParameter.ParameterName));

                if (columns.Contains(commandParameter.ParameterName))
                    commandParameter.Value = dataRow[commandParameter.ParameterName];
                else if (columns.Contains(commandParameter.ParameterName.Substring(1)))
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];

                i++;
            }
        }

        /// <summary>
        /// This method assigns dataRow column values to an array of IDataParameters
        /// </summary>
        /// <param name="commandParameters">Array of IDataParameters to be assigned values</param>
        /// <param name="dataRow">The dataRow used to hold the stored procedure's parameter values</param>
        /// <exception cref="System.InvalidOperationException">Thrown if any of the parameter names are invalid.</exception>
        protected void AssignParameterValues(IDataParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // Do nothing if we get no data
                return;
            }

            DataColumnCollection columns = dataRow.Table.Columns;

            int i = 0;
            // Set the parameters values
            foreach (IDataParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if (commandParameter.ParameterName == null ||
                 commandParameter.ParameterName.Length <= 1)
                    throw new InvalidOperationException(string.Format(
                     "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                     i, commandParameter.ParameterName));

                if (columns.Contains(commandParameter.ParameterName))
                    commandParameter.Value = dataRow[commandParameter.ParameterName];
                else if (columns.Contains(commandParameter.ParameterName.Substring(1)))
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];

                i++;
            }
        }

        /// <summary>
        /// This method assigns an array of values to an array of IDataParameters
        /// </summary>
        /// <param name="commandParameters">Array of IDataParameters to be assigned values</param>
        /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
        /// <exception cref="System.ArgumentException">Thrown if an incorrect number of parameters are passed.</exception>
        protected void AssignParameterValues(IDataParameter[] commandParameters, params  object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the IDataParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length, k = 0; i < j; i++)
            {
                if (commandParameters[i].Direction != ParameterDirection.ReturnValue)
                {
                    // If the current array value derives from IDataParameter, then assign its Value property
                    if (parameterValues[k] is IDataParameter)
                    {
                        IDataParameter paramInstance;
                        paramInstance = (IDataParameter)parameterValues[k];
                        if (paramInstance.Direction == ParameterDirection.ReturnValue)
                        {
                            paramInstance = (IDataParameter)parameterValues[++k];
                        }
                        if (paramInstance.Value == null)
                        {
                            commandParameters[i].Value = DBNull.Value;
                        }
                        else
                        {
                            commandParameters[i].Value = paramInstance.Value;
                        }
                    }
                    else if (parameterValues[k] == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = parameterValues[k];
                    }
                    k++;
                }
            }
        }
    }


    /// <summary>
    /// SQLiteHelper
    /// </summary>
    public class SQLiteHelper3 : System.IDisposable
    {
        private SQLiteConnection _SQLiteConn = null;
        private SQLiteTransaction _SQLiteTrans = null;
        private bool _IsRunTrans = false;
        private string _SQLiteConnString = null;
        private bool _disposed = false;
        private bool _autocommit = false;
        #region 构造/析构函数
        /// <summary>
        /// 初始化 SQLiteHelper
        /// </summary>
        public SQLiteHelper3()
            : this(ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString)
        {
        }

        /// <summary>
        /// 初始化 SQLiteHelper
        /// </summary>
        /// <param name="connectionstring">数据库连接字符串</param>
        public SQLiteHelper3(string connectionstring)
        {
            this._SQLiteConnString = connectionstring;
            this._SQLiteConn = new SQLiteConnection(this._SQLiteConnString);
            this._SQLiteConn.Commit += new SQLiteCommitHandler(_SQLiteConn_Commit);
            this._SQLiteConn.RollBack += new EventHandler(_SQLiteConn_RollBack);
        }

        /// <summary>
        /// SQLiteHelper 析构函数
        /// </summary>
        ~SQLiteHelper3()
        {
            this.Dispose(false);
        }

        #endregion
        #region 方法
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        private void Open()
        {
            if (this._SQLiteConn.State == ConnectionState.Closed)
            {
                this._SQLiteConn.Open();
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void Close()
        {
            if (this._SQLiteConn.State != ConnectionState.Closed)
            {
                if (this._IsRunTrans && this._autocommit)
                {
                    this.Commit();
                }
                this._SQLiteConn.Close();
            }
        }
        /// <summary>
        /// 开始数据库事务
        /// </summary>
        public void BeginTransaction()
        {
            this._SQLiteConn.BeginTransaction();
            this._IsRunTrans = true;
        }
        /// <summary>
        /// 开始数据库事务
        /// </summary>
        /// <param name="isoLevel">事务锁级别</param>
        public void BeginTransaction(IsolationLevel isoLevel)
        {
            this._SQLiteConn.BeginTransaction(isoLevel);
            this._IsRunTrans = true;
        }
        /// <summary>
        /// 提交当前挂起的事务
        /// </summary>
        public void Commit()
        {
            if (this._IsRunTrans)
            {
                this._SQLiteTrans.Commit();
                this._IsRunTrans = false;
            }
        }
        /// <summary>
        /// 回滚当前挂起的事务
        /// </summary>
        public void Rollback()
        {
            if (this._IsRunTrans)
            {
                this._SQLiteTrans.Rollback();
                this._IsRunTrans = false;
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public int Execute(string command)
        {
            int result = -1;
            this.Open();
            using (SQLiteCommand sqlitecmd = new SQLiteCommand(command))
            {
                result = sqlitecmd.ExecuteNonQuery();
            }
            this.Close();
            return result;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="parameter">参数组</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public int Execute(string command, SQLiteParameter[] parameter)
        {
            int result = -1;
            this.Open();
            using (SQLiteCommand sqlitecmd = new SQLiteCommand(command))
            {
                sqlitecmd.Parameters.AddRange(parameter);
                result = sqlitecmd.ExecuteNonQuery();
            }
            this.Close();
            return result;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回第一行第一列值</returns>
        public object ExecuteScalar(string command)
        {
            object result = null;
            this.Open();
            using (SQLiteCommand sqlitecmd = new SQLiteCommand(command))
            {
                result = sqlitecmd.ExecuteScalar();
            }
            this.Close();
            return result;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <param name="parmeter">参数组</param>
        /// <returns>返回受影响行数 [SELECT 不会返回影响行]</returns>
        public object ExecuteScalar(string command, SQLiteParameter[] parmeter)
        {
            object result = null;
            this.Open();
            using (SQLiteCommand sqlitecmd = new SQLiteCommand(command))
            {
                sqlitecmd.Parameters.AddRange(parmeter);
                result = sqlitecmd.ExecuteScalar();
            }
            this.Close();
            return result;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="command">SQL语句</param>
        /// <returns>返回DataSet数据集</returns>
        public DataSet GetDs(string command)
        {
            return this.GetDs(command, string.Empty);
        }
        public DataSet GetDs(string command, string tablename)
        {
            DataSet ds = new DataSet();
            this.Open();
            using (SQLiteCommand sqlitecmd = new SQLiteCommand(command, this._SQLiteConn))
            {
                using (SQLiteDataAdapter sqliteadapter = new SQLiteDataAdapter(sqlitecmd))
                {
                    if (string.Empty.Equals(tablename))
                    {
                        sqliteadapter.Fill(ds);
                    }
                    else
                    {
                        sqliteadapter.Fill(ds, tablename);
                    }
                }
            }
            this.Close();
            return ds;
        }

        public DataSet GetDs(string command, out SQLiteCommand SqlItecmd)
        {
            return this.GetDs(command, string.Empty, out SqlItecmd);
        }

        public DataSet GetDs(string command, string tablename, out SQLiteCommand SqlItecmd)
        {
            DataSet ds = new DataSet();
            this.Open();
            SQLiteCommand sqlcmd = new SQLiteCommand(command, this._SQLiteConn);
            using (SQLiteDataAdapter sqladapter = new SQLiteDataAdapter(sqlcmd))
            {
                sqladapter.Fill(ds);
            }
            SqlItecmd = sqlcmd;
            this.Close();
            return ds;
        }

        public int Update(DataSet ds, ref SQLiteCommand SqlItecmd)
        {
            return this.Update(ds, string.Empty, ref SqlItecmd);
        }

        public int Update(DataSet ds, string tablename, ref SQLiteCommand SqlItecmd)
        {
            int result = -1;
            this.Open();
            using (SQLiteDataAdapter sqladapter = new SQLiteDataAdapter(SqlItecmd))
            {
                using (SQLiteCommandBuilder sqlcommandbuilder = new SQLiteCommandBuilder(sqladapter))
                {
                    if (string.Empty.Equals(tablename))
                    {
                        result = sqladapter.Update(ds);
                    }
                    else
                    {
                        result = sqladapter.Update(ds, tablename);
                    }
                }
            }
            this.Close();
            return result;
        }
        /// <summary>
        /// 释放该实例的托管资源
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // 定义释放非托管资源
                }
                this._disposed = true;
            }
        }
        #endregion
        #region 属性
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this._SQLiteConnString;
            }
        }
        /// <summary>
        /// 设置是否自动提交事务
        /// </summary>
        public bool AutoCommit
        {
            get
            {
                return this._autocommit;
            }
            set
            {
                this._autocommit = value;
            }
        }
        #endregion
        #region 事件
        void _SQLiteConn_RollBack(object sender, EventArgs e)
        {
            this._IsRunTrans = false;
        }
        void _SQLiteConn_Commit(object sender, CommitEventArgs e)
        {
            this._IsRunTrans = false;
        }
        #endregion
    }


    /// <summary>
    /// 本类为SQLite数据库帮助类 
    /// </summary>
    public  class SQLiteHelper
    {

        public SQLiteHelper()
        { 
        
        }

        public SQLiteHelper(string connection)
        {
            Conn = connection;
        }

        //数据库连接字符串
        public   string Conn = @"Data Source=D:\DRSAPPS\crm.db;";

        //轻量级数据库SQLite的连接字符串写法："Data Source=D:\database\test.s3db"
        //轻量级数据库SQLite的加密后的连接字符串写法："Data Source=Maximus.db;Version=3;Password=myPassword;"\

        #region ExecuteNonQuery
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>所受影响的行数</returns>
        public  int ExecuteNonQuery(SQLiteCommand cmd)
        {
            int result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, cmd.CommandType, cmd.CommandText);
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>所受影响的行数</returns>
        public  int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            int result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText);
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>所受影响的行数</returns>
        public int ExecuteNonQuery(string commandText )
        {
            int result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, CommandType.Text, commandText);
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        public bool ExecuteTrans(string[] sqls)
        {
            
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            
            
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                SQLiteTransaction trans = null;
                
               
                try
                {
                    trans = con.BeginTransaction(IsolationLevel.ReadCommitted);
                    foreach (string sql in sqls)
                    {
                        SQLiteCommand cmd = new SQLiteCommand();
                        cmd.CommandText =sql;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        cmd.Transaction = trans;
                        //PrepareCommand(cmd, con, ref trans, true, CommandType.Text, sql);
                        if (cmd.ExecuteNonQuery() < 0)
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                    
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>所受影响的行数</returns>
        public  int ExecuteNonQuery(string commandText, CommandType commandType, params SQLiteParameter[] cmdParms)
        {
            int result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText);
                try
                {
                    result = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public  object ExecuteScalar(SQLiteCommand cmd)
        {
            object result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, cmd.CommandType, cmd.CommandText);
                try
                {
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public  object ExecuteScalar(string commandText, CommandType commandType)
        {
            object result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText);
                try
                {
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        public object ExecuteScalar(string commandText )
        {
            object result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, CommandType.Text, commandText);
                try
                {
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public  object ExecuteScalar(string commandText, CommandType commandType, params SQLiteParameter[] cmdParms)
        {
            object result = 0;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnection con = new SQLiteConnection(Conn))
            {
                SQLiteTransaction trans = null;
                PrepareCommand(cmd, con, ref trans, true, commandType, commandText);
                try
                {
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>SqlDataReader对象</returns>
        public  DbDataReader ExecuteReader(SQLiteCommand cmd)
        {
            DbDataReader reader = null;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");

            SQLiteConnection con = new SQLiteConnection(Conn);
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, cmd.CommandType, cmd.CommandText);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>SqlDataReader对象</returns>
        public  DbDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            DbDataReader reader = null;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection con = new SQLiteConnection(Conn);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>SqlDataReader对象</returns>
        public  DbDataReader ExecuteReader(string commandText, CommandType commandType, params SQLiteParameter[] cmdParms)
        {
            DbDataReader reader = null;
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteConnection con = new SQLiteConnection(Conn);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText, cmdParms);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reader;
        }
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <returns>DataSet对象</returns>
        public  DataSet ExecuteDataSet(SQLiteCommand cmd)
        {
            DataSet ds = new DataSet();
            SQLiteConnection con = new SQLiteConnection(Conn);
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, cmd.CommandType, cmd.CommandText);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>DataSet对象</returns>
        public  DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            SQLiteConnection con = new SQLiteConnection(Conn);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return ds;
        }


        public DataTable ExecuteDataTable(string commandText )
        {
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            SQLiteConnection con = new SQLiteConnection(Conn);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, CommandType.Text, commandText);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
            //return ds;
        }

        /// <summary>
        /// 执行数据库查询，返回DataSet对象
        /// </summary>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>DataSet对象</returns>
        public  DataSet ExecuteDataSet(string commandText, CommandType commandType, params SQLiteParameter[] cmdParms)
        {
            if (Conn == null || Conn.Length == 0)
                throw new ArgumentNullException("Conn");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            SQLiteConnection con = new SQLiteConnection(Conn);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            PrepareCommand(cmd, con, ref trans, false, commandType, commandText, cmdParms);
            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return ds;
        }
        #endregion

        /// <summary>
        /// 通用分页查询方法
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="strColumns">查询字段名</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <param name="pageSize">每页数据数量</param>
        /// <param name="currentIndex">当前页数</param>
        /// <param name="recordOut">数据总量</param>
        /// <returns>DataTable数据表</returns>
        public  DataTable SelectPaging(string tableName, string strColumns, string strWhere, string strOrder, int pageSize, int currentIndex, out int recordOut)
        {
            DataTable dt = new DataTable();
            recordOut = Convert.ToInt32(ExecuteScalar("select count(*) from " + tableName, CommandType.Text));
            string pagingTemplate = "select {0} from {1} where {2} order by {3} limit {4} offset {5} ";
            int offsetCount = (currentIndex - 1) * pageSize;
            string commandText = String.Format(pagingTemplate, strColumns, tableName, strWhere, strOrder, pageSize.ToString(), offsetCount.ToString());
            using (DbDataReader reader = ExecuteReader(commandText, CommandType.Text))
            {
                if (reader != null)
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        /// <summary>
        /// 预处理Command对象,数据库链接,事务,需要执行的对象,参数等的初始化
        /// </summary>
        /// <param name="cmd">Command对象</param>
        /// <param name="conn">Connection对象</param>
        /// <param name="trans">Transcation对象</param>
        /// <param name="useTrans">是否使用事务</param>
        /// <param name="cmdType">SQL字符串执行类型</param>
        /// <param name="cmdText">SQL Text</param>
        /// <param name="cmdParms">SQLiteParameters to use in the command</param>
        private  void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, ref SQLiteTransaction trans, bool useTrans, CommandType cmdType, string cmdText, params SQLiteParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (useTrans)
            {
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = trans;
            }


            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }

}
