using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Terry.CRM.Web
{
    /// <summary>
    ///Extension Methods have to be implemented as 
    ///static methods and in static classes 
    ///(inside a non-nested, non-generic static class to be more precise).
    ///You can use extension methods to extend a class or interface, but not to override them.
    ///An extension method with the same name and signature as an interface or class method 
    ///will never be called. At compile time, extension methods always have lower priority 
    ///than instance methods defined in the type itself.
    ///Extension methods cannot access private variables in the type they are extending.
    /// </summary>
    public static class StringExtension
    {

        /// <summary>
        /// true, if is valid email address
        /// </summary>
        /// <param name="s">email address to test</param>
        /// <returns>true, if is valid email address</returns>
        public static bool IsValidEmailAddress(this string s)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
        }
        /// <summary>
        /// Checks if url is valid. 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string url)
        {
            string strRegex = "^(https?://)"
        + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
        + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
        + "|" // allows either IP or domain
        + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
        + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
        + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
        + "(:[0-9]{1,5})?" // port number- :80
        + "((/?)|" // a slash isn't required if there is no file name
        + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
            return new Regex(strRegex).IsMatch(url);
        }
        public static string FixSqlString(this string s)
        {
            return s.Replace("'", "''");
        }
    }
}
