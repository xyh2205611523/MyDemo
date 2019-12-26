using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 相关博客顺序
/// https://blog.csdn.net/sD7O95O/article/details/89334103
/// https://blog.csdn.net/weixin_39813211/article/details/98855019
/// https://blog.csdn.net/starfd/article/details/80706227?tdsourcetag=s_pctim_aiomsg
/// https://blog.csdn.net/weixin_30768175/article/details/96967045
/// https://www.cnblogs.com/qingspace/p/3732677.html
/// </summary>
namespace mailkitCore
{
    /// <summary>
    /// 基于MailKit的邮件帮助类
    /// </summary>
    public static class EMailHelper
    {
        /// <summary>
        /// 邮件服务器Host
        /// </summary>
        public static string Host { get; set; }
        /// <summary>
        /// 邮件服务器Port
        /// </summary>
        public static int Port { get; set; }
        /// <summary>
        /// 邮件服务器是否是ssl
        /// </summary>
        public static bool UseSsl { get; set; }
        /// <summary>
        /// 发送邮件的账号友善名称
        /// </summary>
        public static string UserName { get; set; }
        /// <summary>
        /// 发送邮件的账号地址
        /// </summary>
        public static string UserAddress { get; set; }
        /// <summary>
        /// 发现邮件所需的账号密码
        /// </summary>
        public static string Password { get; set; }

        /// <summary>
        /// 发送电子邮件，默认发送方为<see cref="UserAddress"/>
        /// </summary>
        /// <param name="subject">邮件主题</param>
        /// <param name="content">邮件内容主题</param>
        /// <param name="toAddress">接收方信息</param>
        /// <param name="textFormat">内容主题模式，默认TextFormat.Text</param>
        /// <param name="attachments">附件</param>
        /// <param name="dispose">是否自动释放附件所用Stream</param>
        /// <returns></returns>
        public static async Task SendEMailAsync(string subject, string content, IEnumerable<MailboxAddress> toAddress, TextFormat textFormat = TextFormat.Text, IEnumerable<AttachmentInfo> attachments = null, bool dispose = true)
        {
            await SendEMailAsync(subject, content, new MailboxAddress[] { new MailboxAddress(UserName, UserAddress) }, toAddress, textFormat, attachments, dispose).ConfigureAwait(false);
        }

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="subject">邮件主题</param>
        /// <param name="content">邮件内容主题</param>
        /// <param name="fromAddress">发送方信息</param>
        /// <param name="toAddress">接收方信息</param>
        /// <param name="textFormat">内容主题模式，默认TextFormat.Text</param>
        /// <param name="attachments">附件</param>
        /// <param name="dispose">是否自动释放附件所用Stream</param>
        /// <returns></returns>
        public static async Task SendEMailAsync(string subject, string content, MailboxAddress fromAddress, IEnumerable<MailboxAddress> toAddress, TextFormat textFormat = TextFormat.Text, IEnumerable<AttachmentInfo> attachments = null, bool dispose = true)
        {
            await SendEMailAsync(subject, content, new MailboxAddress[] { fromAddress }, toAddress, textFormat, attachments, dispose).ConfigureAwait(false);
        }

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="subject">邮件主题</param>
        /// <param name="content">邮件内容主题</param>
        /// <param name="fromAddress">发送方信息</param>
        /// <param name="toAddress">接收方信息</param>
        /// <param name="textFormat">内容主题模式，默认TextFormat.Text</param>
        /// <param name="attachments">附件</param>
        /// <param name="dispose">是否自动释放附件所用Stream</param>
        /// <returns></returns>
        public static async Task SendEMailAsync(string subject, string content, IEnumerable<MailboxAddress> fromAddress, IEnumerable<MailboxAddress> toAddress, TextFormat textFormat = TextFormat.Text, IEnumerable<AttachmentInfo> attachments = null, bool dispose = true)
        {
            var message = new MimeMessage();//创建一个mime消息,MimeMessage是MailKit里代表一封电子邮件的对象，它和.NET自带的MailMessage类型非常类似。比如添加主题和发件人：
                                            // From是一个集合类型，要通过Add方法来添加：
                                            //message.From.Add(new MailboxAddress("发件人姓名", "发件人邮箱账号名"));

            //发送方
            message.From.AddRange(fromAddress);//AddRange：添加地址集合。多发。add:添加指定的地址。单发
            //接收方
            message.To.AddRange(toAddress);//AddRange：添加地址集合。多发。add:添加指定的地址。单发
            message.Subject = subject;//邮件标题
            var body = new TextPart(textFormat)
            {
                Text = content
            };
            MimeEntity entity = body;//Multipart继承了他
            if (attachments != null)
            {
                var mult = new Multipart("mixed")//创建附件信息
                {
                    body
                };
                foreach (var att in attachments)
                {
                    if (att.Stream != null)
                    {
                        var attachment = string.IsNullOrWhiteSpace(att.ContentType) ? new MimePart() : new MimePart(att.ContentType);
                        attachment.Content = new MimeContent(att.Stream);
                        attachment.ContentDisposition = new ContentDisposition(ContentDisposition.Attachment);
                        attachment.ContentTransferEncoding = att.ContentTransferEncoding;
                        attachment.FileName = ConvertHeaderToBase64(att.FileName, Encoding.UTF8);//解决附件中文名问题
                        mult.Add(attachment);
                    }
                }
                entity = mult;
            }
            message.Body = entity;//邮件正文（Body属性）支持多种格式，最常用的是纯文本和HTML。需要用TextPart类来安排，TextPart的构造函数里可以指定正文格式，例如HTML：
            message.Date = DateTime.Now;

             //密送
            //message.Bcc.Add(new MailboxAddress("huang", "123456789@qq.com"));
            //抄送
            //message.Cc.Add(new MailboxAddress("huang", "123456789@qq.com"));

            using (var client = new SmtpClient())
            {

                //smtp.MessageSent += (sender, args) => { // args.Response };//MessageSent事件里可以通过args参数，获得服务器的响应信息，以便于记录Log。//MessageSent事件将在每次成功发送消息时发出。

                //接受所有SSL证书（如果服务器支持SistTTLS）
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    //创建连接
                    await client.ConnectAsync(Host, Port, UseSsl).ConfigureAwait(false);//异步地建立到指定邮件服务器的连接。
               // await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);//连接outlook.com的服务器需要设置为SecureSocketOptions.StartTls，不然会拒绝连接。对于其他服务器，可以试试 SecureSocketOptions.Auto
                await client.AuthenticateAsync(UserAddress, Password).ConfigureAwait(false);//使用指定的用户名和密码进行异步身份验证
                await client.SendAsync(message).ConfigureAwait(false);//发送信息
                await client.DisconnectAsync(true).ConfigureAwait(false);//异步断开服务。
                if (dispose && attachments != null)
                {
                    foreach (var att in attachments)
                    {
                        att.Dispose();//释放非托管资源
                    }
                }             
            }
        }
        private static string ConvertToBase64(string inputStr, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(inputStr));
        }
        private static string ConvertHeaderToBase64(string inputStr, Encoding encoding)
        {//https://www.cnblogs.com/qingspace/p/3732677.html
            var encode = !string.IsNullOrEmpty(inputStr) && inputStr.Any(c => c > 127);
            if (encode)
            {
                return "=?" + encoding.WebName + "?B?" + ConvertToBase64(inputStr, encoding) + "?=";
            }
            return inputStr;
        }



        /// <summary>
        /// 发送邮件  不知道多个发件人有什么用
        /// 参考：https://blog.csdn.net/sD7O95O/article/details/89334103
        /// </summary>
        /// <param name="model"></param>
        public static void SendMailKitEmail(IEnumerable<MailboxAddress> toAddress)
        {
            var messageToSend = new MimeMessage()
            {
                Sender = new MailboxAddress(UserName, UserAddress),
                Subject = "",
            };
            //支持多个发件人
            messageToSend.From.AddRange(toAddress);
            using (var smtp = new SmtpClient())
            {
                smtp.MessageSent += async (sender, args) =>
                {
                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await smtp.ConnectAsync(Host, Port, UseSsl);
                    await smtp.AuthenticateAsync(UserAddress,Password);
                    await smtp.SendAsync(messageToSend);
                    await smtp.DisconnectAsync(true);
                };
            }
        }





    }
    /// <summary>
    /// 附件信息
    /// </summary>
    public class AttachmentInfo : IDisposable
    {
        /// <summary>
        /// 附件类型，比如application/pdf
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件传输编码方式，默认ContentEncoding.Default
        /// </summary>
        public ContentEncoding ContentTransferEncoding { get; set; } = ContentEncoding.Default;
        /// <summary>
        /// 文件数组
        /// </summary>
        public byte[] Data { get; set; }
        private Stream stream;
        /// <summary>
        /// 文件数据流，获取数据时优先采用此部分
        /// </summary>
        public Stream Stream
        {
            get
            {
                if (this.stream == null && this.Data != null)
                {
                    stream = new MemoryStream(this.Data);
                }
                return this.stream;
            }
            set { this.stream = value; }
        }
        /// <summary>
        /// 释放Stream
        /// </summary>
        public void Dispose()
        {
            if (this.stream != null)
            {
                this.stream.Dispose();
            }
        }
    }
}
