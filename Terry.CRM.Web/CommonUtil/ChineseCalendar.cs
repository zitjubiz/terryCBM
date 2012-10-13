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
using System.Globalization;
using System.Diagnostics;

namespace Terry.CRM.Web.CommonUtil
{
    public class ChineseCalendar
    {
        static ChineseLunisolarCalendar cCalendar = new ChineseLunisolarCalendar();

        /// <summary>
        /// 根据公历获取农历日期
        /// 传入日期：2010-3-4
        /// 返回农历：庚寅[虎]年正月十九
        /// </summary>
        /// <param name="datetime">公历日期</param>
        /// <returns></returns>
        public static string GetChineseDateTime(DateTime datetime)
        {
            int lyear = cCalendar.GetYear(datetime);
            int lmonth = cCalendar.GetMonth(datetime);
            int lday = cCalendar.GetDayOfMonth(datetime);
            int leapMonth = cCalendar.GetLeapMonth(lyear);

            bool isleap = false;

            if (leapMonth == lmonth)
            {
                //闰月
                isleap = true;
            }

            return string.Concat(GetLunisolarYear(datetime.Year), "年", isleap ? "闰" : string.Empty, GetLunisolarMonth(lyear,lmonth), "月", GetLunisolarDay(lday));
        }

        public static string GetChineseShortDay(DateTime datetime)
        {
            int lyear = cCalendar.GetYear(datetime);
            int lmonth = cCalendar.GetMonth(datetime);
            int lday = cCalendar.GetDayOfMonth(datetime);

            string luniMonth = GetLunisolarMonth(lyear,lmonth);
            string luniDay = GetLunisolarDay(lday);

            Debug.WriteLine(lmonth + " " + lday);
            string Festival = GetLunarFesitaval(datetime);
            if (Festival != "")
                return Festival;
            string TwentyFourDay =GetChineseTwentyFourDay(datetime);
            if (TwentyFourDay != "")
                return TwentyFourDay;

            if (luniDay == "初一")
                return luniMonth;
            else
                return luniDay;
        }

        #region 农历年

        /// <summary>
        /// 十天干
        /// </summary>
        private static string[] tiangan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

        /// <summary>
        /// 十二地支
        /// </summary>
        private static string[] dizhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        /// <summary>
        /// 十二生肖
        /// </summary>
        private static string[] shengxiao = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

        /// <summary>
        /// 返回农历天干地支年 
        /// </summary>
        /// <param name="year">公历年</param>
        /// <returns></returns>
        public static string GetLunisolarYear(int year)
        {
            if (year > 3)
            {
                int tgIndex = (year - 4) % 10;
                int dzIndex = (year - 4) % 12;

                return string.Concat(tiangan[tgIndex], dizhi[dzIndex], "[", shengxiao[dzIndex], "]");

            }

            throw new ArgumentOutOfRangeException("无效的年份!");
        }
        /// <summary>
        /// 返回生肖
        /// </summary>
        /// <param name="datetime">公历日期</param>
        /// <returns></returns>
        public static string GetShengXiao(DateTime datetime)
        {
            return shengxiao[cCalendar.GetTerrestrialBranch(cCalendar.GetSexagenaryYear(datetime)) - 1];
        }
        #endregion

        #region 农历月

        /// <summary>
        /// 农历月
        /// </summary>
        private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };
        /// <summary>
        /// 返回农历月(month是1-13的一个数,假如是)
        /// </summary>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public static string GetLunisolarMonth(int year,int month)
        {
            bool isleap = false;

            //获取闰月， 0 则表示没有闰月
            int leapMonth = cCalendar.GetLeapMonth(year);

            if (leapMonth > 0)
            {
                if (leapMonth == month)
                {
                    //闰月
                    isleap = true;
                    month--;
                }
                else if (month > leapMonth)
                {
                    month--;
                }
            }

            if (month < 13 && month > 0)
            {
                return (isleap ? "闰" : string.Empty) + months[month - 1] + "月";
            }

            throw new ArgumentOutOfRangeException("无效的月份!");
        }
        #endregion

        #region 农历日

        /// <summary>
        /// 
        /// </summary>
        private static string[] days1 = { "初", "十", "廿", "三" };

        /// <summary>
        /// 日
        /// </summary>
        private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

        /// <summary>
        /// 返回农历日
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetLunisolarDay(int day)
        {

            if (day > 0 && day < 32)
            {
                if (day != 20 && day != 30)
                {
                    return string.Concat(days1[(day - 1) / 10], days[(day - 1) % 10]);
                }
                else
                {
                    return string.Concat(days[day / 10 - 1], days1[1]);
                }
            }

            throw new ArgumentOutOfRangeException("无效的日!");
        }

        #endregion

        #region 农历节日

        private static string[] lFtv = {

                                     "0101 春节",
                                     "0115 元宵",
                                     "0505 端午",
                                     "0707 七夕",
                                     "0815 中秋",
                                     "0909 重阳",
                                     "1208 腊八",
                                     "1224 小年",
                                     "1230 除夕"
                                      };
        ///<summary>
        ///农历日期返回农历节日
        ///农历节日如遇闰月，以第一个月为休假日,但如果闰12月,除夕则以闰月计
        ///例如2009年闰5月
        ///</summary>
        ///<returns></returns>
        public static string GetLunarFesitaval(DateTime datetime)
        {
            int lunaryear = cCalendar.GetYear(datetime);
            int lunarMonth = cCalendar.GetMonth(datetime);
            int lunarDay = cCalendar.GetDayOfMonth(datetime);

            int leapMonth = cCalendar.GetLeapMonth(lunaryear);

            bool isleap = false;

            if (leapMonth > 0)
            {
                if (leapMonth == lunarMonth)
                {
                    //闰月
                    isleap = true;
                    lunarMonth--;
                }
                else if (lunarMonth > leapMonth)
                {
                    lunarMonth--;
                }
            }

            string str = @"(\d{2})(\d{2})([\s\*])(.+)$"; //匹配的正则表达式
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(str);
            for (int i = 0; i < lFtv.Length; i++)
            {
                string[] s = re.Split(lFtv[i]);
                //农历节日如遇闰月，以第一个月为休假日,闰月的节日不算
                if (!isleap && Convert.ToInt32(s[1]) == lunarMonth && Convert.ToInt32(s[2]) == lunarDay)
                {
                    return s[4];
                }
            }
            return "";
        }


            #region 二十四节气
        /// <summary>
        /// 定气法计算二十四节气,二十四节气是按地球公转来计算的，并非是阴历计算的
        /// </summary>
        /// <remarks>
        /// 节气的定法有两种。古代历法采用的称为"恒气"，即按时间把一年等分为24份，
        /// 每一节气平均得15天有余，所以又称"平气"。现代农历采用的称为"定气"，即
        /// 按地球在轨道上的位置为标准，一周360°，两节气之间相隔15°。由于冬至时地
        /// 球位于近日点附近，运动速度较快，因而太阳在黄道上移动15°的时间不到15天。
        /// 夏至前后的情况正好相反，太阳在黄道上移动较慢，一个节气达16天之多。采用
        /// 定气时可以保证春、秋两分必然在昼夜平分的那两天。
        /// </remarks>
        private static string[] lunarHolidayName = 
                    { 
                    "小寒", "大寒", "立春", "雨水", 
                    "惊蛰", "春分", "清明", "谷雨", 
                    "立夏", "小满", "芒种", "夏至", 
                    "小暑", "大暑", "立秋", "处暑", 
                    "白露", "秋分", "寒露", "霜降", 
                    "立冬", "小雪", "大雪", "冬至"
                    };
        private static int[] sTermInfo = new int[] { 0, 21208, 42467, 63836, 85337,
            107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 
            308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };  
        public static string GetChineseTwentyFourDay(DateTime _date)
        {
            DateTime baseDateAndTime = new DateTime(1900, 1, 6, 2, 5, 0); //#1/6/1900 2:05:00 AM#
            DateTime newDate;
            double num;
            int y;
            string tempStr = "";

            y = _date.Year;

            for (int i = 1; i <= 24; i++)
            {
                num = 525948.76 * (y - 1900) + sTermInfo[i - 1];

                newDate = baseDateAndTime.AddMinutes(num);//按分钟计算
                if (newDate.DayOfYear == _date.DayOfYear)
                {
                    tempStr = lunarHolidayName[i - 1];
                    break;
                }
            }
            return tempStr;

        }
        #endregion
        #endregion


        private void Test()
        {
            System.Diagnostics.Debug.WriteLine(GetChineseShortDay(DateTime.Parse("2010/02/13")));
            System.Diagnostics.Debug.WriteLine(GetChineseShortDay(DateTime.Parse("2011/02/02")));

        }
    }
}
