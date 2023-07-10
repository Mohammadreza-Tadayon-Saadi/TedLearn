using System.Globalization;

namespace Core.Convertors;

public static class DateConvertors
{
    public static string MiladiToShamsi(this DateTime date)
    {
        var persian = new PersianCalendar();
        return persian.GetYear(date) + "/" +
               persian.GetMonth(date).ToString("00") + "/" +
               persian.GetDayOfMonth(date).ToString("00");
    }

    public static DateTime ShamsiToMiladi(string persianDate)
    {
        PersianCalendar pc = new PersianCalendar();

        int year = Convert.ToInt32(persianDate.Split('/')[0]);
        int month = Convert.ToInt32(persianDate.Split('/')[1]);
        int day = Convert.ToInt32(persianDate.Split('/')[2]);

        return new DateTime(year, month, day, pc);
    }
}
