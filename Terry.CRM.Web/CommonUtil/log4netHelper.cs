using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;
using System.IO;

namespace Terry.CRM.Web.CommonUtil
{
    public class log4netHelper
    {
        private static ILog logger= LogManager.GetLogger(typeof(log4netHelper));

        private log4netHelper() { }
        private log4netHelper(Type type)
        {
            logger = LogManager.GetLogger(type);
        }
        public static void Info(object message)
        {
            logger.Info(message);
        }
        public static void Error(object message) 
        {
            logger.Error("\r\nClient IP:" + HttpContext.Current.Request.UserHostAddress +
                "\r\nURL:" + HttpContext.Current.Request.Url + "\r\nException:" + message);
        }
        public static void Error(object message, Exception exception)
        {
            logger.Error("\r\nClient IP:" + HttpContext.Current.Request.UserHostAddress +
                "\r\nURL:" + HttpContext.Current.Request.Url + "\r\nException:" + message,
                exception);
        }
    }
}