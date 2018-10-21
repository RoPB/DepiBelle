using System;
namespace DepiBelleDepi.Utilities
{
    public static class DateConverter
    {
        public static string ShortDate(DateTime date){

            return date.ToString("MM-dd-yyyy");
        }
    }
}
