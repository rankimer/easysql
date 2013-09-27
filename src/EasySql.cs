#region Select your DB type
//#define SQLSERVER
#define MSACCESS
//#define MYSQL
//#define SQLLITE

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

#region Include DB type Libraries
#if MSACCESS
using System.Data.OleDb;
#endif
#if SQLSERVER
using System.Data.SqlClient;
#endif
#if MYSQL
using System.Data.MySql;
#endif
#if SQLITE
using System.Data.SQLite;
#endif
#endregion

namespace Kimerran.EasySql
{

    public class EasySql
    {

        EasySqlDb db;

        public EasySql(string connectionString)
        {
            DbConnection cn = _CreateConnection();
            DbCommand cm = _CreateCommand();

          

            cn.ConnectionString = connectionString;
            cm.Connection = cn;

             db = new EasySqlDb(cn, cm);


        }



        public DataTable ExecuteQuery(string query)
        {

            return db.ExecuteQuery(query);
        }

        protected class EasySqlDb
        {
            private DbConnection _cn;
            private DbCommand _cm;

            public string ConnectionString { get; set; }
            public EasySqlDb(DbConnection connection, DbCommand command)
            {
                _cn = connection;
                _cm = command;
            }
            public void OpenDbConnection()
            {

            }

            public DataTable ExecuteQuery(string queryString)
            {
                return new DataTable();
            }



        }


        #region DB-type specifics
        private DbCommand _CreateCommand()
        {
#if SQL_SERVER            
            return new SqlConnection();
#elif MSACCESS
            return new OleDbCommand();           
#endif
        }

        private DbConnection _CreateConnection()
        {
#if SQL_SERVER
            
             cn = new SqlConnection();
#elif MSACCESS
            return new OleDbConnection();
#endif
        }
        #endregion

    }


  
}