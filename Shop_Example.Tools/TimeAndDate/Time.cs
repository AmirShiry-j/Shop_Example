using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Example.Tools.TimeAndDate
{
    public class Time
    {
        public string GetCurrentTime(DateTime timeCreate)
        {
            int tempTime;

            tempTime = (DateTime.Now - timeCreate).Days;
            if (tempTime >= 365)
            {
                return tempTime / 365 + "سال پیش";
            }

            tempTime = (DateTime.Now - timeCreate).Days;
            if (tempTime >= 30)
            {
                return tempTime / 30 + " ماه پیش";
            }

            if (tempTime != 0)
            {
                return tempTime + " روز پیش";
            }

            tempTime = (DateTime.Now - timeCreate).Hours;
            if (tempTime != 0)
            {
                return tempTime + " ساعت پیش";
            }

            tempTime = (DateTime.Now - timeCreate).Minutes;
            if (tempTime != 0)
            {
                return tempTime + " دقیقه پیش";
            }

            tempTime = (DateTime.Now - timeCreate).Seconds;
            return tempTime + " ثانیه پیش";
        }
        public string ToShamsi(DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            string day = pc.GetDayOfMonth(value).ToString();
            string month = pc.GetMonth(value).ToString();
            string year = pc.GetYear(value).ToString();

            switch (month)
            {
                case "1":
                    month = "فروردین";
                    break;
                case "2":
                    month = "اردیبهشت";
                    break;
                case "3":
                    month = "خرداد";
                    break;
                case "4":
                    month = "تیر";
                    break;
                case "5":
                    month = "مرداد";
                    break;
                case "6":
                    month = "شهریور";
                    break;
                case "7":
                    month = "مهر";
                    break;
                case "8":
                    month = "آبان";
                    break;
                case "9":
                    month = "آذر";
                    break;
                case "10":
                    month = "دی";
                    break;
                case "11":
                    month = "بهمن";
                    break;
                case "12":
                    month = "اسفند";
                    break;
            }

            return $"{day} {month} {year}";
        }
    }
}
//در تاریخ 19 فروردین 1399
