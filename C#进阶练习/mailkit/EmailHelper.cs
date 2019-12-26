using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string _STEPNAME = "smtp.qq.com";//SMTP服务器地址，实际上就是代收发服务器地址，是由邮箱服务商提供的。常用SMTP地址
                                                 //1、QQ邮箱（mail.qq.com）
                                                 //POP3服务器地址：pop.qq.com（端口：110）
                                                 //SMTP服务器地址：smtp.qq.com（端口：25）
                                                 //2、搜狐邮箱（sohu.com）:
                                                 //POP3服务器地址:pop3.sohu.com（端口：110）
                                                 //SMTP服务器地址:smtp.sohu.com（端口：25）
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

        /// <summary>
        /// 邮箱配置
        /// </summary>
        /// <param name="email"></param>
        /// <param name="smtp"></param>
        public EMailHelper(string email, string smtp)
        {
            this._USEREMAIL = email;
            this._PASSWORD = smtp;
        }

        public EMailHelper(string email, string emailBlonger, string smtp)
        {
            this._USEREMAIL = email;
            this._PASSWORD = smtp;
            this._EMAILBLONGER = emailBlonger;
        }
        /// <summary>
        /// 邮箱发送类
        /// </summary>
        /// <param name="toEmaill">发送方邮箱</param>
        /// <param name="toEmailBlonger">发送方名称</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="text">发送的文字内容</param>
        /// <param name="html">发送的html内容</param>
        /// <param name="path">发送的附件,找不到的就自动过滤</param>
        /// <returns></returns>
        public string SendEmail(string toEmaill, string toEmailBlonger, string subject, string text, string html, string path)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                //发送方
                message.From.Add(new MailboxAddress(this._EMAILBLONGER, this._USEREMAIL));//这个地方是个坑，_EMAILBLONGER又值就会报错：mail from address must be same as authorization user（来自地址的邮件必须与授权用户相同）解决：MailboxAddress('邮箱名称','授权码') 如果你填的邮箱名称不为你的发送邮箱名称也会报上面的错，邮箱名称可以为空，
                //接受方
                message.To.Add(new MailboxAddress(toEmailBlonger, toEmaill));
                //标题
                message.Subject = subject;
                //创建附件
                var multipart = new Multipart("mixed");//创建一个新的MimeKit。具有指定子类型的Multipart。mixed:mime中如果一封邮件中含有附件，那邮件的Content - Type域中必须定义multipart / mixed类型
                #region 概述
                //MIME邮件中各种不同类型的内容是分段存储的，各个段的排列方式、位置信息都通过Content-Type域的multipart类型来定义。multipart类型主要有三种子类型：mixed、alternative、related。
                //                ●  multipart / mixed类型

                //    如果一封邮件中含有附件，那邮件的Content - Type域中必须定义multipart / mixed类型，邮件通过multipart / mixed类型中定义的boundary标识将附件内容同邮件其它内容分成不同的段。基本格式如下：

                //Content - Type： multipart / mixed;

                //                boundary = "{分段标识}"

                //    ●  multipart / alternative类型

                //    MIME邮件可以传送超文本内容，但出于兼容性的考虑，一般在发送超文本格式内容的同时会同时发送一个纯文本内容的副本，如果邮件中同时存在纯文本和超文本内容，则邮件需要在Content - Type域中定义multipart / alternative类型，邮件通过其boundary中的分段标识将纯文本、超文本和邮件的其它内容分成不同的段。基本格式如下：

                //Content - Type： multipart / alternative;

                //                boundary = "{分段标识}"

                //    ●  multipart / related类型

                //    MIME邮件中除了可以携带各种附件外，还可以将其它内容以内嵌资源的方式存储在邮件中。比如我们在发送html格式的邮件内容时，可能使用图像作为html的背景，html文本会被存储在alternative段中，而作为背景的图像则会存储在multipart / related类型定义的段中。
                #endregion
                if (!string.IsNullOrEmpty(text))
                {
                    var plain = new TextPart(TextFormat.Plain)//TextFormat:文本格式的枚举。Plain:纯文本格式。
                    {
                        Text = text //Text:纯文本格式的别名。
                    };
                    multipart.Add(plain);
                }
                //html内容
                if (!string.IsNullOrEmpty(html))
                {
                    var Html = new TextPart(TextFormat.Html)//HTML文本格式。
                    {
                        Text = html
                    };
                    multipart.Add(Html);
                }
                if (!string.IsNullOrEmpty(path))
                {
                    var pathList = path.Split(';');
                    foreach (var p in pathList)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(p.Trim()))
                            {
                                var attimg = new MimePart()
                                {//"image", "png"方法里带参数的话
                                    ContentObject = new ContentObject(File.OpenRead(p), ContentEncoding.Default),
                                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                    ContentTransferEncoding = ContentEncoding.Base64,
                                    FileName = Path.GetFileName(path)
                                };
                                multipart.Add(attimg);
                            }
                        }
                        catch (FileNotFoundException ex)
                        {
                            //找不到文件就不提交附件了
                        }
                    }
                }
                //赋值邮件内容
                message.Body = multipart;
                //开始发送
                using (var client = new SmtpClient())
                {
                    client.Connect(this._STEPNAME, this._STEPPORT, false);
                    client.Authenticate(this._USEREMAIL, this._PASSWORD);
                    client.Send(message);
                    client.Disconnect(true);
                }
                return "邮件发送成功";
            }
            catch (Exception ex)
            {
                return "邮箱发送失败";
            }
        }

        /// <summary>
        /// 邮箱发送类，不用输入用户昵称的
        /// </summary>
        /// <param name="toEmaill">发送方邮箱</param>
        /// <param name="toEmailBlonger">发送方名称</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="text">发送的文字内容</param>
        /// <param name="html">发送的html内容</param>
        /// <param name="path">发送的附件，多附件用;隔开</param>
        /// <returns></returns>
        public void SendEmail(string toEmaill, string subject, string text, string html, string path)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                //发送方
                message.From.Add(new MailboxAddress(this._USEREMAIL));
                //接受方
                message.To.Add(new MailboxAddress(toEmaill));
                //标题
                message.Subject = subject;
                //创建附件
                var multipart = new Multipart("mixed");
                //文字内容
                if (!string.IsNullOrEmpty(text))
                {
                    var plain = new TextPart(TextFormat.Plain)
                    {
                        Text = text
                    };
                    multipart.Add(plain);
                }
                //html内容
                if (!string.IsNullOrEmpty(html))
                {
                    var Html = new TextPart(TextFormat.Html)
                    {
                        Text = html
                    };
                    multipart.Add(Html);
                }
                if (!string.IsNullOrEmpty(path))
                {//修改为多附件，
                    var pathList = path.Split(';');
                    foreach (var p in pathList)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(p.Trim()))
                            {
                                var attimg = new MimePart()
                                {//"image", "png"方法里带参数的话
                                    ContentObject = new ContentObject(File.OpenRead(p), ContentEncoding.Default),
                                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                    ContentTransferEncoding = ContentEncoding.Base64,
                                    FileName = Path.GetFileName(path)
                                };
                                multipart.Add(attimg);
                            }
                        }
                        catch (FileNotFoundException ex)
                        {
                            //找不到文件就不提交附件了
                        }
                    }
                }
                //赋值邮件内容
                message.Body = multipart;
                //开始发送
                using (var client = new SmtpClient())
                {
                    client.Connect(this._STEPNAME, this._STEPPORT, false);
                    client.Authenticate(this._USEREMAIL, this._PASSWORD);
                    client.Send(message);
                    client.Disconnect(true);
                }
                // return "邮件发送成功";
            }
            catch (Exception ex)
            {
                // return "邮箱发送失败";
            }
        }
    }





    
}
