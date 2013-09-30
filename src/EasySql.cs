
/*
The MIT License (MIT)
Copyright (c) 2013 kimerran

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#region Select your DB type
#define SQLSERVER
//#define MSACCESS
//#define MYSQL
//#define SQLLITE
//#define ORACLE
//#define POSTGRESQL
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
#elif SQLSERVER
using System.Data.SqlClient;
#elif MYSQL
using System.Data.MySql;
#elif SQLITE
using System.Data.SQLite;
#elif ORACLE
using Oracle
#elif POSTGRESQL
using Npgsql;
#endif
#endregion

namespace Kimerran.EasySql
{
    /// <summary>
    /// EasySql is a simple wrapper to common databases used for a .NET application
    /// </summary>
    public class EasySql 
    {
       
        #region private members
        /// <summary>
        /// Internal connection object
        /// </summary>
        private DbConnection _cn;

        /// <summary>
        /// Internal command object
        /// </summary>
        private DbCommand _cm;

        /// <summary>
        /// Calls Open() method of the connection object
        /// </summary>
        private void OpenDbConnection()
        {
            _cn.Open();
        }

        /// <summary>
        /// Calls Close() method of the connection object
        /// </summary>
        private void CloseDbConnection()
        {
            _cn.Close();
        }
	    #endregion

        #region constructor
        /// <summary>
        /// Creates instance of EasySql
        /// </summary>
        /// <param name="connectionString">Valid connection string for the selected database type</param>
        public EasySql(string connectionString)
        {
            _cn = _CreateConnection();
            _cn.ConnectionString = connectionString;

            _cm = _CreateCommand();
            _cm.Connection =_cn;
           
        }
	    #endregion
        
        #region public members

        /// <summary>
        /// Gets or sets the command timeout value of the command
        /// </summary>
        public int  CommandTimeOut { 
            get 
            {
                if (_cm != null) 
                {
                    return _cm.CommandTimeout;
                }
                return 0;
            }
            set
            {
                if (_cm != null)
                {
                    _cm.CommandTimeout = value;
                }

            }
        }
       
		/// <summary>
        /// Implementation of ExecuteQuery used for SELECT queries
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string queryString)
        {
            this.OpenDbConnection();
            DataTable retDt = new DataTable();
            DbDataReader dr;

            _cm.CommandText = queryString;
            _cm.CommandType = CommandType.Text;

            dr = _cm.ExecuteReader();
         
            retDt.Load(dr);

            this.CloseDbConnection();

            return retDt;
        }

        /// <summary>
        /// Implementation of ExecuteNonQuery used for INSERT, UPDATE and DELETE queries
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string queryString)
        {
            this.OpenDbConnection();
            int retRowsAffected;

            _cm.CommandText = queryString;
            _cm.CommandType = CommandType.Text;

            retRowsAffected = _cm.ExecuteNonQuery();

            this.CloseDbConnection();

            return retRowsAffected;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedureName"></param>
        [System.Obsolete("Feature not yet implemented")]
        public DataTable? ExecuteStoredProcedure(string storedProcedureName)
        {
            //return null;
            throw new Exception("Feature not yet implemented");
        }

        
        /// <summary>
        /// Dispose the EasySql instance
        /// </summary>
        public void Dispose()
        {
            _cn.Dispose();
            _cm.Dispose();
        }

	    #endregion
        

        #region db-type specific
        /// <summary>
        /// Creates a command object for the selected database type
        /// </summary>
        /// <returns></returns>
        private DbCommand _CreateCommand()
        {
#if SQLSERVER
            return new SqlCommand();
#elif MSACCESS
            return new OleDbCommand();
#endif
        }

        /// <summary>
        /// Creates a connection object for the selected database type
        /// </summary>
        /// <returns></returns>
        private DbConnection _CreateConnection()
        {   
#if SQLSERVER
            return new SqlConnection();
#elif MSACCESS
            return new OleDbConnection();
#endif
        }
        #endregion

        
           
    }



       
}




