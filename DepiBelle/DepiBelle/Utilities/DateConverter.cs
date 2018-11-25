using System;
namespace DepiBelle.Utilities
{
    public static class DateConverter
    {
        public static string ShortDate(DateTime date){

            return date.ToString("MM-dd-yyyy");
        }

        public static string ShortTime(TimeSpan timeSpan)
        {

            return timeSpan.ToString(@"hh\:mm");
        }
    }
}
