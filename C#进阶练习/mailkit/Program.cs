using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net;


namespace mailkit
{
    class Program
    {
        /// <summary>
        /// 邮件Demo
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
               var email=new EMailHelper("2205611523@qq.com", "qpkmxykcrihddjbb");
            //email.SendEmail("对方的邮箱", "对方邮箱名称也可以是邮箱", "标题", "内容", "<b>这是html标记的文本</b>", "附加地址");
            email.SendEmail("320906876@qq.com", "320906876@qq.com", "测试", "测试一下", "<b>这是html标记的文本</b>", "");

        }
    }
}
