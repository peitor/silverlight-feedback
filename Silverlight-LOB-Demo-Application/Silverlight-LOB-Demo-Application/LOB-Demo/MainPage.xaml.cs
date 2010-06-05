using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Silverlight_LOB_Demo_Application.FeedbackControl;
using Silverlight_LOB_Demo_Application.FeedbackControl.Helpers;

namespace Silverlight_LOB_Demo_Application.LOB_Demo
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonFeedback_Click(object sender, RoutedEventArgs e)
        {
            // TODO Add additional info to send
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("UserName: " + "Peter Gfader");
            stringBuilder.AppendLine("Roles: " + "Administrator");
            stringBuilder.AppendLine("LOB Silverlight version: " + AssemblyHelper.GetVersionNumber(Assembly.GetExecutingAssembly()));
            
            stringBuilder.AppendLine("----");
            //strinBuilder.Append(debugOutput.Text);
            //strinBuilder.AppendLine("----");
            

            // Show window and we are done
            FeedbackWindow feedback = new FeedbackWindow();
            feedback.AdditionalInfo = stringBuilder.ToString();
            feedback.ScreenShot = new WriteableBitmap(this.LayoutRoot, null);
            feedback.Show();


        }
    }
}
