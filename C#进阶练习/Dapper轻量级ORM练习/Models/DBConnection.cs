using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper轻量级ORM练习.Models
{
    public class DBConnection
    {
        /// <summary>
        /// 数据库连接类型
        /// </summary>
        public string DbType { get; set; }
        /// <summary>
        /// 连接名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
