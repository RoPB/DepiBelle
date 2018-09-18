using System;
namespace DepiBelle.Utilities
{
    public static class DateConverter
    {
        public static string ShortDate(DateTime date){

            return date.ToString("MM-dd-yyyy");
        }
    }
}
