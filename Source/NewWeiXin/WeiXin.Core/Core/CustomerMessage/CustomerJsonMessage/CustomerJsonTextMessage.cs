﻿using System.Web;

namespace WeiXin.Core.Messages
{
    /// <summary>
    /// 客服文本消息
    /// </summary>
    public class CustomerJsonTextMessage : CustomerServiceMessage
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
        public override string MsgType
        {
            get
            {
                return "text";
            }
        }

        public override string GetJson()
        {
            string content = string.Format("\"content\":\"{0}\"", HttpUtility.UrlDecode(this.Content));
            return base.Json(content);
        }
    }
}
