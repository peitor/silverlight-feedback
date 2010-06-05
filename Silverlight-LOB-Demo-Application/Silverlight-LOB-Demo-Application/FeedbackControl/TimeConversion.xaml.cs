namespace Silverlight_LOB_Demo_Application.FeedbackControl
{
    public static class TimeConversion
    {


        public static string ConvertToNiceTime(this int p)
        {
            string returnValue = "";
            int seconds = p / 1000;

            if (seconds > 60)
            {
                int minutes = seconds / 60;
                seconds = seconds%60;
                
                if (minutes > 60)
                {
                    int hours = minutes / 60;
                    minutes = minutes % 60;

                    if (hours>24)
                    {
                        int days = hours/24;
                        hours = hours%24;
                        returnValue = string.Format("{0} d. ", days);
                    }


                    returnValue = returnValue + string.Format("{0} h. ", hours);
    
                }
                returnValue = returnValue + string.Format("{0} min. ", minutes);
                

            }
            returnValue = returnValue + string.Format("{0} sec.", seconds);
            return returnValue;
        }

    }
}