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
#if POSTGRESQL
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
        /// <summary>
        /// Instance of EasySqlDb which hides the implementation
        /// </summary>
        private EasySqlDb db;

        /// <summary>
        /// Creates instance of EasySql wrapper
        /// </summary>
        /// <param name="connectionString"></param>
        public EasySql(string connectionString)
        {
            DbConnection cn = _CreateConnection();
            DbCommand cm = _CreateCommand();
            cn.ConnectionString = connectionString;
            cm.Connection = cn;

            db = new EasySqlDb(cn, cm);

        }

        /// <summary>
        /// Returns a boolean whether the connection is successful or not
        /// </summary>
        /// <returns></returns>
        public bool TryOpen()
        {
            try
            {
                db.OpenDbConnection();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Used for SELECT queries and returns a DataTable containing the results
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string query)
        {
            try
            {
                return db.ExecuteQuery(query);
            }
            catch (Exception e)
            {
                
                throw new Exception(e.Message);
            }
         
        }

        /// <summary>
        /// Used for INSERT, UPDATE and DELETE and returns the number of affected rows
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query)
        {
            try
            {
                return db.ExecuteNonQuery(query);
            }
            catch (Exception e)
            {
                
                throw new Exception(e.Message);
            }
        }


        #region EasySqlDb Class
        /// <summary>
        /// Hides the implementation of EasySql
        /// </summary>
        protected class EasySqlDb
        {
            /// <summary>
            /// Internal connection object
            /// </summary>
            private DbConnection _cn;

            /// <summary>
            /// Internal command object
            /// </summary>
            private DbCommand _cm;

            /// <summary>
            /// Creates instance of EasySqlDb
            /// </summary>
            /// <param name="connection"></param>
            /// <param name="command"></param>
            public EasySqlDb(DbConnection connection, DbCommand command)
            {
                _cn = connection;
                _cm = command;
            }

            /// <summary>
            /// Calls Open() method of the connection object
            /// </summary>
            public void OpenDbConnection()
            {
                _cn.Open();
            }

            /// <summary>
            /// Calls Close() method of the connection object
            /// </summary>
            public void CloseDbConnection()
            {
                _cn.Close();
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

        }
        #endregion

        #region DB-type specifics
        /// <summary>
        /// Creates a DbCommand object depending on which database you choose
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
        /// Creates a DbConnection object depending on which database you choose
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
