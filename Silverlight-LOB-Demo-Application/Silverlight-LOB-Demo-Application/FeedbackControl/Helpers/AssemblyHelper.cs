using System.Reflection;

namespace Silverlight_LOB_Demo_Application.FeedbackControl.Helpers
{
    public static class AssemblyHelper
    {
        public static string GetVersionNumber(Assembly runningAssembly)
        {
            var returnValue = "";

            if (runningAssembly != null && runningAssembly.FullName != null)
            {
                string version = runningAssembly.FullName.Split(',')[1];
                returnValue = version.Split('=')[1];
            }

            return returnValue;
        }

    }
}
