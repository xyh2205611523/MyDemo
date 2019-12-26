using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mailkit
{
    /// <summary>
    /// 基于MailKit的邮件帮助类
    /// </summary>
    public class EMailHelper
    {
        /// <summary>
        /// 服务邮箱
        /// </summary>
        private string _STEPNAME = "smtp.qq.com";
        /// <summary>
        /// 服务邮箱端口
        /// </summary>
        private int _STEPPORT = 587;
        /// <summary>
        /// 发送方邮箱
        /// </summary>
        private string _USEREMAIL;
        /// <summary>
        /// 发送方邮箱Smtp授权码
        /// </summary>
        private string _PASSWORD;
        /// <summary>
        /// 发送方邮箱归属人，昵称
        /// </summary>
        private string _EMAILBLONGER;
        private string email;
        private string emailBlonger;
        private string smtp;
 


    }
}
