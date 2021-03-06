﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeiXin.Attributes;
using WeiXin.Config;
using WeiXin.Messages;
using WeiXin.SendCustomerServiceMessage;
using WeiXin.UserManager;
using WeiXin.Utilitys;

namespace WeiXin.ProcessMessages
{
    public static class ProcessMessageManager
    {
        public const string NotProcessMsg = "呃，不明白，您的问题难倒我了。";
        public const string NotImplementedMsg = "研发小弟正在疯狂的开发中，精彩即将呈现，敬请期待[鼓掌][鼓掌][鼓掌]";
        public const string ConfigurationErrorMsg = "唉，粗心大意啊，服务器配置出错了。放心，我们会马上修复的。";
        public const string SystemUpdateingMsg = "为了给您提供更好的服务体验，我们会不定期进行系统升级，给您造成的不便我们深表歉意。";
        readonly static Dictionary<MessageType, ProcessMessage> Processs;
        readonly static Dictionary<string, Func<Message, string>> MessageFuncs;

        static ProcessMessageManager()
        {
            MessageFuncs = new Dictionary<string, Func<Message, string>>();
            MessageFuncs.Add("1", Action1);
            MessageFuncs.Add("2", Action2);
            MessageFuncs.Add("3", Action3);
            MessageFuncs.Add("4", Action4);
            MessageFuncs.Add("5", Action5);
            MessageFuncs.Add("6", Action6);
            MessageFuncs.Add("7", Action7);
            MessageFuncs.Add("8", Action8);
            MessageFuncs.Add("9", Action9);
            MessageFuncs.Add("12", Action12);

            Processs = new Dictionary<MessageType, ProcessMessage>();
            Processs.Add(MessageType.Event, new ProcessEventMessage());
            Processs.Add(MessageType.Image, new ProcessImageMessage());
            Processs.Add(MessageType.Link, new ProcessLinkMessage());
            Processs.Add(MessageType.Location, new ProcessLocationMessage());
            Processs.Add(MessageType.Text, new ProcessTextMessage());
            Processs.Add(MessageType.Video, new ProcessVideoMessage());
            Processs.Add(MessageType.Voice, new ProcessVoiceMessage());
        }

        public static string InvokMessageFunc(Message receiveMsg, string funcKey)
        {
            if (MessageFuncs.ContainsKey(funcKey))
            {
                return MessageFuncs[funcKey](receiveMsg);
            }
            else
            {
                return NotProcess(receiveMsg.Xml);
            }
        }

        public static string Process(Message receiveMsg, MessageType msgType)
        {
            string replyMsg = string.Empty;
            // TODO:这里需要根据 receiveMsg.MsgId 去重
            if (ConfigProperty.IsConfigurationOk)
            {
                if (ConfigProperty.WeiXin_UpdateEnable)
                {
                    replyMsg = MessageManager.CreateTextMessageXml(receiveMsg.Xml, SystemUpdateingMsg);
                }
                else
                {
                    var process = Processs[msgType];
                    if (process != null)
                    {
                        replyMsg = process.Process(receiveMsg);
                    }
                    else
                    {
                        LogHelper.Log("方法[WeiXin.ProcessMessages.ProcessMessageManager.Process]创建消息处理接口失败，原因是没有找到对应的消息类型接口。");
                    }
                }
            }
            else
            {
                replyMsg = MessageManager.CreateTextMessageXml(receiveMsg.Xml, ConfigurationErrorMsg);
            }
            return replyMsg;
        }

        public static string NotProcess(string xml)
        {
            return MessageManager.CreateTextMessageXml(xml, NotProcessMsg);
        }

        /// <summary>
        /// 验证此消息类型是否需要回复
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public static bool IsReply(MessageType msgType)
        {
            ReplyMessageAttribute replyMessageAttribute = ReflectionHelper.GetCustomAttribute<ReplyMessageAttribute>(msgType);
            return replyMessageAttribute.IsReply;
        }
        private static string Action1(Message receiveMsg)
        {
            return MessageManager.CreateTextMessageXml(receiveMsg.Xml, "被动文本消息");
        }
        private static string Action2(Message receiveMsg)
        {
            var article = new List<WeiXin.Messages.Article>();
            article.Add(new WeiXin.Messages.Article { Title = string.Format(MessageManager.MsgTag, "被动单图文消息"), Description = string.Format(MessageManager.MsgTag, "被动单图文消息，此处省略一万字。。。"), PicUrl = string.Format(MessageManager.MsgTag, "http://h.hiphotos.baidu.com/image/pic/item/c9fcc3cec3fdfc037d970d53d63f8794a5c2266a.jpg"), Url = string.Format(MessageManager.MsgTag, "http://www.wangwenzhuang.com/") });
            return MessageManager.CreateNewsMessageXml(receiveMsg.Xml, article);
        }
        private static string Action3(Message receiveMsg)
        {
            var article = new List<WeiXin.Messages.Article>();
            article.Add(new WeiXin.Messages.Article { Title = string.Format(MessageManager.MsgTag, "被动多图文消息1"), Description = string.Format(MessageManager.MsgTag, "被动多图文消息，此处省略一万字。。。"), PicUrl = string.Format(MessageManager.MsgTag, "http://h.hiphotos.baidu.com/image/pic/item/c9fcc3cec3fdfc037d970d53d63f8794a5c2266a.jpg"), Url = string.Format(MessageManager.MsgTag, "http://www.wangwenzhuang.com/") });
            article.Add(new WeiXin.Messages.Article { Title = string.Format(MessageManager.MsgTag, "被动多图文消息2"), Description = string.Format(MessageManager.MsgTag, "被动多图文消息，此处省略一万字。。。"), PicUrl = string.Format(MessageManager.MsgTag, "http://g.hiphotos.baidu.com/image/pic/item/55e736d12f2eb93895023c7fd7628535e4dd6fcb.jpg"), Url = string.Format(MessageManager.MsgTag, "http://www.wangwenzhuang.com/") });
            article.Add(new WeiXin.Messages.Article { Title = string.Format(MessageManager.MsgTag, "被动多图文消息3"), Description = string.Format(MessageManager.MsgTag, "被动多图文消息，此处省略一万字。。。"), PicUrl = string.Format(MessageManager.MsgTag, "http://e.hiphotos.baidu.com/image/pic/item/63d0f703918fa0ec8426f0f7249759ee3c6ddb63.jpg"), Url = string.Format(MessageManager.MsgTag, "http://www.wangwenzhuang.com/") });
            return MessageManager.CreateNewsMessageXml(receiveMsg.Xml, article);
        }
        private static string Action4(Message receiveMsg)
        {
            Task t = new Task(() =>
            {
                var msg = new CustomerServiceTextMessage { Touser = receiveMsg.FromUserName, Content = "客服文本消息" };
                SendCustomerServiceMessageManager.SendTextMessage(msg);
            });
            t.Start();
            return string.Empty;
        }
        private static string Action5(Message receiveMsg)
        {
            Task t = new Task(() =>
            {
                var title = "客服单图文消息";
                var discription = "被动单图文消息，此处省略一万字。。。";
                var url = "http://www.wangwenzhuang.com/";
                var articles = new List<WeiXin.SendCustomerServiceMessage.Article>();
                articles.Add(new WeiXin.SendCustomerServiceMessage.Article { Title = title, Description = discription, PicUrl = "http://h.hiphotos.baidu.com/image/pic/item/c9fcc3cec3fdfc037d970d53d63f8794a5c2266a.jpg", Url = url });
                var msg = new CustomerServiceNewsMessage { Touser = receiveMsg.FromUserName, Articles = articles };
                SendCustomerServiceMessageManager.SendNewsMessage(msg);
            });
            t.Start();
            return string.Empty;
        }
        private static string Action6(Message receiveMsg)
        {
            Task t = new Task(() =>
            {
                var discription = "被动单图文消息，此处省略一万字。。。";
                var url = "http://www.wangwenzhuang.com/";
                var articles = new List<WeiXin.SendCustomerServiceMessage.Article>();
                articles.Add(new WeiXin.SendCustomerServiceMessage.Article { Title = "客服多图文消息1", Description = discription, PicUrl = "http://h.hiphotos.baidu.com/image/pic/item/c9fcc3cec3fdfc037d970d53d63f8794a5c2266a.jpg", Url = url });
                articles.Add(new WeiXin.SendCustomerServiceMessage.Article { Title = "客服多图文消息2", Description = discription, PicUrl = "http://g.hiphotos.baidu.com/image/pic/item/55e736d12f2eb93895023c7fd7628535e4dd6fcb.jpg", Url = url });
                articles.Add(new WeiXin.SendCustomerServiceMessage.Article { Title = "客服多图文消息3", Description = discription, PicUrl = "http://e.hiphotos.baidu.com/image/pic/item/63d0f703918fa0ec8426f0f7249759ee3c6ddb63.jpg", Url = url });
                var msg = new CustomerServiceNewsMessage { Touser = receiveMsg.FromUserName, Articles = articles };
                SendCustomerServiceMessageManager.SendNewsMessage(msg);
            });
            t.Start();
            return string.Empty;
        }
        private static string Action7(Message receiveMsg)
        {
            return MessageManager.CreateTextMessageXml(receiveMsg.Xml, "请说一段语音发来。");
        }
        private static string Action8(Message receiveMsg)
        {
            return MessageManager.CreateTextMessageXml(receiveMsg.Xml, string.Format("<a href=\"http://58.215.139.6/testwebsite/test/Index/{0}\">查看OpenId</a>", receiveMsg.FromUserName));
        }
        private static string Action9(Message receiveMsg)
        {
            return MessageManager.CreateTextMessageXml(receiveMsg.Xml, string.Format("OAuth2.0授权分两种，第一种获取获取 OpenId，不弹出授权界面；第二种弹出授权界面，不但能获取 OpenId，还可以获取用户的信息。\r\n<a href=\"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=http%3a%2f%2f58.215.139.6%2ftestwebsite%2foauth2%2fIndex&response_type=code&scope=snsapi_base&state=0#wechat_redirect\">第一种</a>\r\n<a href=\"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=http%3a%2f%2f58.215.139.6%2ftestwebsite%2foauth2%2fIndex&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect\">第二种</a>", ConfigProperty.WeiXin_AppId));
        }
        private static string Action12(Message receiveMsg)
        {
            Task t = new Task(() =>
            {
                // 获取已关注列表
                var openIds = WeiXinUserManager.GetUserList();
                if (openIds != null && openIds.Count > 0)
                {
                    var title = "已关注用户";
                    var discription = string.Empty;
                    var url = "http://www.wangwenzhuang.com/";
                    // 获取已关注列表每个人的基本信息
                    foreach (var opendId in openIds)
                    {
                        var userInfo = WeiXinUserManager.GetUserInfo(opendId);
                        discription += string.Format("subscribe：{0}\r\n", userInfo.Subscribe);
                        discription += string.Format("openid：{0}\r\n", userInfo.OpenId);
                        discription += string.Format("nickname：{0}\r\n", userInfo.NickName);
                        discription += string.Format("sex：{0}\r\n", userInfo.Sex);
                        discription += string.Format("language：{0}\r\n", userInfo.Language);
                        discription += string.Format("city：{0}\r\n", userInfo.City);
                        discription += string.Format("province：{0}\r\n", userInfo.Province);
                        discription += string.Format("country：{0}\r\n", userInfo.Country);
                        discription += string.Format("headimgurl：{0}\r\n", userInfo.HeadImgUrl);
                        discription += string.Format("subscribe_time：{0}\r\n", userInfo.SubscribeTime);
                        discription += string.Format("remark：{0}", userInfo.Remark);
                        discription += "\r\n\r\n";
                    }
                    var articles = new List<WeiXin.SendCustomerServiceMessage.Article>();
                    articles.Add(new WeiXin.SendCustomerServiceMessage.Article { Title = title, Description = discription, Url = url });
                    var msg = new CustomerServiceNewsMessage { Touser = receiveMsg.FromUserName, Articles = articles };
                    SendCustomerServiceMessageManager.SendNewsMessage(msg);
                }
            });
            t.Start();
            return string.Empty;
        }
    }
}