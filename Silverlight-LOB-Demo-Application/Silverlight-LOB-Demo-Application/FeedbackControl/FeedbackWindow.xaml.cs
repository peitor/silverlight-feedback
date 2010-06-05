using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageTools.IO.Png;

namespace Silverlight_LOB_Demo_Application.FeedbackControl
{
    public partial class FeedbackWindow
    {
        //TODO: Make this your constant company URL: e.g. http://feedback.ssw.com.au/Upload
        string _baseUrl = "http://{0}/Silverlight-Feedback/UploadHandlers/";

        private const string SysinfoUploadHandler = "SysInfoReceiver.ashx";
        private const string FileUploadHandler = "FileUploadReceiver.ashx";

        // From AdamCogan: Don't tell people if everything is OK
        //private const string ThankYouForYourFeedbackMessage = "Thank you for your feedback!";
        

        private WriteableBitmap _screenShot;
        
        /// <summary>
        /// Sets the screen shot as WriteableBitmap.
        /// </summary>
        /// <value>The screen shot.</value>
        public WriteableBitmap ScreenShot
        {
            set
            {
                _screenShot = value;
                this.image1.Source = value;
            }
        }

        /// <summary>
        /// Allows to add additional info that will be sent to the server.
        /// </summary>
        /// <value>The additional info.</value>
        public string AdditionalInfo
        {
            get;
            set;
        }


        public FeedbackWindow()
        {
            InitializeComponent();
            this.Loaded += Feedback_Loaded;
        }


        void Feedback_Loaded(object sender, RoutedEventArgs e)
        {
            _baseUrl = _baseUrl.Replace("{0}", HtmlPage.Document.DocumentUri.Host);


            //_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´¯`.¸_¸.·´
            // Collect system info
            StringBuilder sbStringBuilder = new StringBuilder();

            sbStringBuilder.AppendLine("OSVersion: " + Environment.OSVersion);
            sbStringBuilder.AppendLine("System start: " + Environment.TickCount.ConvertToNiceTime());
            sbStringBuilder.AppendLine("CLR Version: " + Environment.Version);


            sbStringBuilder.AppendLine("Host.Source: " + Application.Current.Host.Source);

            if (HtmlPage.Document != null && HtmlPage.Document.DocumentUri != null)
            {
                sbStringBuilder.AppendLine("Host: " + HtmlPage.Document.DocumentUri.Host);
                sbStringBuilder.AppendLine("Host.Port: " + HtmlPage.Document.DocumentUri.Port);
            }

            if (HtmlPage.BrowserInformation != null)
            {
                sbStringBuilder.AppendLine("BrowserInformation.Name: " + HtmlPage.BrowserInformation.Name);
                sbStringBuilder.AppendLine("BrowserInformation.BrowserVersion: " + HtmlPage.BrowserInformation.BrowserVersion);
                sbStringBuilder.AppendLine("BrowserInformation.Platform: " + HtmlPage.BrowserInformation.Platform);

                sbStringBuilder.AppendLine("BrowserInformation.ProductName: " + HtmlPage.BrowserInformation.ProductName);
                sbStringBuilder.AppendLine("BrowserInformation.ProductVersion: " + HtmlPage.BrowserInformation.ProductVersion);
                sbStringBuilder.AppendLine("BrowserInformation.UserAgent: " + HtmlPage.BrowserInformation.UserAgent);

                sbStringBuilder.AppendLine("BrowserInformation.CookiesEnabled: " + HtmlPage.BrowserInformation.CookiesEnabled);
            }


            //sbStringBuilder.AppendLine("IsRunningOutOfBrowser: " + System.Windows.Application.Current.IsRunningOutOfBrowser);
            //sbStringBuilder.AppendLine("RootVisual.Clip.Bounds.Width: " + System.Windows.Application.Current.RootVisual.Clip.Bounds.Width);
            //sbStringBuilder.AppendLine("RootVisual.Clip.Bounds.Height: " + System.Windows.Application.Current.RootVisual.Clip.Bounds.Height);


            
            sbStringBuilder.AppendLine("Windowless: " + Application.Current.Host.Settings.Windowless);
            sbStringBuilder.AppendLine("MaxFrameRate: " + Application.Current.Host.Settings.MaxFrameRate);


            //System.Windows.Analytics  
            Analytics analytics = new Analytics();
            sbStringBuilder.AppendLine("AverageProcessLoad: " + analytics.AverageProcessLoad);
            sbStringBuilder.AppendLine("AverageProcessorLoad: " + analytics.AverageProcessorLoad);
            sbStringBuilder.AppendLine("GpuCollection.Count: " + analytics.GpuCollection.Count);
            foreach (var singleGpu in analytics.GpuCollection)
            {
                sbStringBuilder.AppendLine(
                    string.Format(" ID:{0}, {1}, {2} ", singleGpu.DeviceId, singleGpu.DriverVersion, singleGpu.VendorId));
            }

            textBlockSysInfo.Text = AdditionalInfo + Environment.NewLine + sbStringBuilder;

        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (checkBoxAttachScreenShot.IsChecked.HasValue && checkBoxAttachScreenShot.IsChecked.Value)
            {
                ImageTools.Image image = ImageTools.ImageExtensions.ToImage(_screenShot);

                using (MemoryStream writestream = new MemoryStream())
                {

                    PngEncoder encoder = new PngEncoder();
                    encoder.Encode(image, writestream);

                    byte[] bytes = writestream.ToArray();

                    MemoryStream readStream = new MemoryStream(bytes);

                    UploadFile(readStream);
                }

            }

            // always try to use {0} as newline

            string text = string.Format("Name: {1}{0}Contact: {2}{0}Comment: {3}{0}{0}{0}",
                Environment.NewLine,
                this.textBoxName.Text,
                this.textBoxContactInfo.Text,
                this.textBoxComments.Text);

            if (checkBoxAttachSysinfo.IsChecked.HasValue && checkBoxAttachSysinfo.IsChecked.Value)
            {
                text = string.Format("{1}{0}SysInfo:{0}{2}", Environment.NewLine, text, textBlockSysInfo.Text);
            }

            UploadSysInfo(text);

            this.DialogResult = true;
        }

        private void UploadSysInfo(string text)
        {
            UriBuilder ub = new UriBuilder(_baseUrl + SysinfoUploadHandler);

            UnicodeEncoding encoding = new UnicodeEncoding();

            byte[] bytes = encoding.GetBytes(text);

            MemoryStream readStream = new MemoryStream(bytes);

            WebClient webClient = new WebClient();
            webClient.OpenWriteCompleted +=
                (o, args) =>
                {
                    PushDataForSysInfo(readStream, args.Result);
                    args.Result.Close();
                    readStream.Close();
                };
            webClient.OpenWriteAsync(ub.Uri);

        }

        private void UploadFile(Stream data)
        {
            UriBuilder ub = new UriBuilder(_baseUrl + FileUploadHandler);

            WebClient webClient = new WebClient();
            webClient.OpenWriteCompleted +=
                (o, args) =>
                {
                    PushData(data, args.Result);
                    args.Result.Close();
                    data.Close();
                };
            webClient.OpenWriteAsync(ub.Uri);

        }

        private static void PushData(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        private static void PushDataForSysInfo(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
            // From AdamCogan: Don't tell people if everything is OK
            //MessageBox.Show(ThankYouForYourFeedbackMessage);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
            }

        }
    }
}
