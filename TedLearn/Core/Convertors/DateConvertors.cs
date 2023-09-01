using System.Globalization;

namespace Core.Convertors;

public static class DateConvertors
{
    public static string MiladiToShamsi(this DateTime date , bool withTime = false)
    {
        var persian = new PersianCalendar();

        if (withTime)
            return persian.GetYear(date) + "/" +
                   persian.GetMonth(date).ToString("00") + "/" +
                   persian.GetDayOfMonth(date).ToString("00") + " " +
                   persian.GetHour(date).ToString("00") + ":" +
                   persian.GetMinute(date).ToString("00") + ":" +
                   persian.GetSecond(date).ToString("00");

        return persian.GetYear(date) + "/" +
                   persian.GetMonth(date).ToString("00") + "/" +
                   persian.GetDayOfMonth(date).ToString("00");
    }

    public static DateTime ShamsiToMiladi(string persianDate)
    {
        var pcDateTime = persianDate.Split(' ');
        var date = pcDateTime[0].Split('/');
        var time = pcDateTime[1].Split(':');

        PersianCalendar pc = new PersianCalendar();

        int year = Convert.ToInt32(date[0]);
        int month = Convert.ToInt32(date[1]);
        int day = Convert.ToInt32(date[2]);

        int hour = Convert.ToInt32(time[0]);
        int minute = Convert.ToInt32(time[1]);
        int second = Convert.ToInt32(time[2]);

        return new DateTime(year, month, day, hour, minute, second, pc);
    }
}
