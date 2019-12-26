using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;

namespace mailkitCore
{
    class Program
    {
        static void Main(string[] args)
        {
           // 对于附件，此处定义了 AttachmentInfo 来作为附件的载体类，其支持两种方式进行附件上传：byte[] 以及Stream，具体的使用例子如下
            //EMailHelper.Host = "smtp.exmail.qq.com";
            //EMailHelper.Port = 465;
            //EMailHelper.UseSsl = true;
            //EMailHelper.UserName = "你要展示的发送方名称";
            //EMailHelper.Password = "你的邮箱密码";
            //EMailHelper.UserAddress = "你的邮箱地址";
            //var subject = "测试多个附件邮件";
            //var content = "Just a test!";
            //var attachs = new List<AttachmentInfo>();
            ////从指定文件夹内读取要发送的附件
            //foreach (var file in Directory.GetFiles("EMailAttach"))
            //{
            //    var att = new AttachmentInfo
            //    {
            //        FileName = Path.GetFileName(file),
            //        //Data = File.ReadAllBytes(file)
            //        Stream = File.OpenRead(file),//Data和Stream两种方式任意用一种即可
            //        ContentTransferEncoding = ContentEncoding.Base64
            //    };
            //    attachs.Add(att);
            //}
            //await EMailHelper.SendEMailAsync(subject, content, new MailboxAddress[] {
            //    new MailboxAddress("接收方邮箱地址")
            //}, attachments: attachs);

//原文链接：https://blog.csdn.net/starfd/article/details/80706227
        }
    }
}
