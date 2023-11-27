using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace AbstractClass
{
    public abstract class DBJob
    {       
        
        public DbConnection Conn;
        public DbDataAdapter DataAdapter;
        //public DataTable DataTable;
        public string ConnString { get; set; }
        public abstract DbConnection GetConnection();
        public abstract DbDataAdapter GetDataAdapter(string sql);
        //добавить датаридер
        //добавить метод HasTable возвращает есть ли такая таблица в БД
        public abstract bool HasTable(string tableName);

    }//end abstract Class

    public class SQLiteDBJob : DBJob
    {
        private SQLiteConnection _conn;
        private SQLiteDataAdapter _dataAdapter;
        public SQLiteDBJob(string constr)  
        {
            ConnString = "DataSource=" + constr;
            _conn = new SQLiteConnection(ConnString);
            Conn = _conn;
        }        

        public override DbConnection GetConnection()
        {
            _conn = new SQLiteConnection(ConnString);
            Conn= _conn;
            return Conn;
        }

        public override SQLiteDataAdapter GetDataAdapter(string sql)
        {
            _dataAdapter = new SQLiteDataAdapter(sql, _conn);
            DataAdapter=_dataAdapter;
            return _dataAdapter;
        }

        public override bool HasTable(string tableName) {

            string sqltext = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name ='" + tableName + "'" +" COLLATE NOCASE;";
            _dataAdapter = new SQLiteDataAdapter(sqltext, _conn);
            DataAdapter = _dataAdapter;       
            return false; 
        }

    }//end Class
    public class MSSqlDBJob : DBJob
    {
        private SqlConnection _conn;
        private SqlDataAdapter _dataAdapter;

        public MSSqlDBJob(string constr)
        {
            //подключиться к локальному серверу
            //string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";

            //подключиться к именованному экземпляру SQL Server
            //string connectionString = "Data Source=MySqlServer\\MSSQL1;";

            ConnString = "Server=localhost;Database=master;Trusted_Connection=True;";
            _conn = new SqlConnection(ConnString);
            Conn = _conn;
        }

        public override DbConnection GetConnection()
        {
            _conn = new SqlConnection(ConnString);
            Conn = _conn;
            return Conn;
        }
        public override SqlDataAdapter GetDataAdapter(string sql)
        {
            _dataAdapter = new SqlDataAdapter(sql, _conn);
            DataAdapter = _dataAdapter;
            return _dataAdapter;
        }

        public override bool HasTable(string tableName) 
        {
            //SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = @db_name AND TABLE_SCHEMA = @schema AND TABLE_NAME = @table;
            //SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'players_statistics1';
            string sqltext = "SHOW TABLES LIKE '" + tableName + "';";

            _dataAdapter = new SqlDataAdapter(sqltext, _conn);
            DataAdapter = _dataAdapter;
            return false;
            
        }//реализовать
    }
}