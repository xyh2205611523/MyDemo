using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.Common;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace Dapper轻量级ORM练习.Models
{
    public class DBhelper
    {

        //连接字符串
         static string strConn = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("DBConnection").Get<DBConnection>().ConnectionString;

        private static IDbConnection openConn(){
            
            IDbConnection conn = new OracleConnection(strConn);
            conn.Open();
            return conn;
        }
        //用lambda形式返回值
        private static Func<DbConnection> openConn1 = () =>
          {
              //OracleConnection:core的话要引用Oracle.ManagedDataAccess.Core，Oracle.ManagedDataAccess是framework用的，不然会报错
              DbConnection conn = new OracleConnection(strConn);
              conn.Open();
              return conn;
          };

        #region  Execute 增删改
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool add(string sql, DynamicParameters param)
        {
            using (IDbConnection conn = openConn())
            {
                //Execute是对IDbConnection的扩展方法，所以可以直接在IDbConnection对象 conn中直接点出Execute方法
                if (conn.Execute(sql, param) < 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool delete(string sql, DynamicParameters param)
        {
            using (IDbConnection conn = openConn())
            {
                if (conn.Execute(sql, param) < 0)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool update(string sql, DynamicParameters param)
        {
            using (IDbConnection conn = openConn())
            {
                if (conn.Execute(sql, param) < 0)
                {
                    return false;
                }
                return true;
            }
        }

        #endregion

        #region Query 查询
        public static List<T> query<T>(string sql, DynamicParameters param)
        {
            using (IDbConnection conn = openConn())
            {
              var result= conn.Query<T>(sql, param).ToList();
                return result;
            }
        }

        #endregion
    }

}
