using System;

using BatteryHnd;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.Background;
using System.Diagnostics;

namespace Battery
{

    public sealed partial class MainPage : Page
    {

        private DispatcherTimer AnimatiorPulsingTimer;
        private DispatcherTimer AnimatiorArcTimer;
        private DispatcherTimer BatteryStatusTimer;
        private int PulsingStatus, PulsingResolution, PulsingTime_ms;
        private Point center;
        private BatteryHandler bHandler;

        //private int temp = 0;
        public MainPage()
        {

            this.InitializeComponent();
            Register_BG_TasK();
            center.X = 1280 / 2;
            center.Y = 720 / 2;
            bHandler = new BatteryHandler();
            bHandler.syncBatteryData();
            setPercentage(bHandler.getBatteryData());
            setStatus(bHandler.getBatteryStatus());

            AnimatiorPulsingTimer_Init();

            //AnimatiorArcTimer_Init();
            //AnimatiorArcTimer_Start();

            BatteryStatusTimer_Init();
            BatteryStatusTimer_Start();
            AnimatiorPulsingTimer_Start();
        }
        private void AnimatiorPulsingTimer_Init()
        {
            PulsingResolution = 150;
            PulsingTime_ms = 300;
            AnimatiorPulsingTimer = new DispatcherTimer();
            AnimatiorPulsingTimer.Tick += AnimatiorPulsingTimer_Tick;
            AnimatiorPulsingTimer.Interval = new TimeSpan(0, 0, 0, 0, PulsingTime_ms / PulsingResolution);
            PulsingStatus = 0;
        }
        private void AnimatiorPulsingTimer_Start()
        {
            AnimatiorPulsingTimer.Start();
        }
        private void AnimatiorPulsingTimer_Tick(object sender, object e)
        {
            PulsingStatus++;
            double ratio = AnimationEffectTools.EaseInSine((double)PulsingStatus / PulsingResolution);
            double radius = 285 + 335 * ratio;
            Thickness Margin;
            Margin.Left = center.X - radius;
            Margin.Top = center.Y - radius;
            Margin.Right = center.X + radius;
            Margin.Bottom = center.Y + radius;
            PulsingEllipse.Width = 2 * radius;
            PulsingEllipse.Height = 2 * radius;
            PulsingEllipse.Margin = Margin;
            PulsingEllipse.Opacity = 1 - ratio;

            if (PulsingStatus == PulsingResolution - 1)
            {
                PulsingStatus = 0;

            }

        }
        private void AnimatiorArcTimer_Init()
        {
            AnimatiorArcTimer = new DispatcherTimer();
            AnimatiorArcTimer.Tick += AnimatiorArcTimer_Tick;
            AnimatiorArcTimer.Interval = new TimeSpan(0, 0, 2);



        }
        private void AnimatiorArcTimer_Start()
        {
            AnimatiorArcTimer.Start();

        }
        private void AnimatiorArcTimer_Tick(object sender, object e)
        {

        }
        private void BatteryStatusTimer_Init()
        {
            BatteryStatusTimer = new DispatcherTimer();
            BatteryStatusTimer.Tick += BatteryStatusTimer_Tick;
            BatteryStatusTimer.Interval = new TimeSpan(0, 0, 0, 5); // 10 sec period ki kell majd tolni a megfelelőre!!!

        }
        private void BatteryStatusTimer_Start()
        {
            BatteryStatusTimer.Start();
        }


        private void BatteryStatusTimer_Tick(object sender, object e)
        {
            bHandler.syncBatteryData();

            setPercentage(bHandler.getBatteryData());
            setStatus(bHandler.getBatteryStatus());
            UpdateTile();
            // XmlDocument ToastMessage = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            //ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(ToastMessage));



        }

        public void setPercentage(int percentage)
        {
            try
            {
                Point ArcEndCoords;
                ArcEndCoords.X = (double)center.X - 280 * Math.Sin(Math.PI * 2 * (double)percentage / 100);
                ArcEndCoords.Y = (double)center.Y - 280 * Math.Cos(Math.PI * 2 * (double)percentage / 100);

                BatteryPercentageValue.Text = percentage + "%";

                // a körvonal ne menjen össze és illeszkedjen  a körre


                if (percentage == 100)
                {
                    FullArc.Visibility = Visibility.Visible;
                }
                else if (percentage > 50)
                {
                    FullArc.Visibility = Visibility.Collapsed;
                    ArcEndPoint.IsLargeArc = true;
                }
                else
                {
                    FullArc.Visibility = Visibility.Collapsed;
                    ArcEndPoint.IsLargeArc = false;
                }
                ArcEndPoint.Point = ArcEndCoords;

                //szín állítása
                if (percentage < 20)
                {
                    BG_Panel.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xc0, 0x27, 0x2c));
                }
                else if (percentage < 50)
                {
                    BG_Panel.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xf6, 0x92, 0x23));
                }
                else if (percentage == 100)
                {
                    BG_Panel.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x07, 0x71, 0xba));
                }
                else
                {
                    BG_Panel.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x36, 0xb4, 0x4c));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }


        }

        public void setStatus(string status)
        {
            try
            {
                if (status.CompareTo("Discharging") == 0)
                {
                    StatusTextBlock.Text = status;
                }
                else
                {
                    StatusTextBlock.Text = status;
                }
                if (status.CompareTo("Fully Charged") == 0 || status.CompareTo("Charging") == 0)
                {
                    AC_pic.Visibility = Visibility.Visible;
                }
                else
                {
                    AC_pic.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

        }

        public void UpdateTile()
        {

            // Get an XML DOM version of a specific template by using getTemplateContent.
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Image);
            XmlNodeList bgImage = tileXml.GetElementsByTagName("image");
            string ImageSrc = "";

            // Choose the suitable image for tile
            if (bHandler.getBatteryStatus().StartsWith("C") || bHandler.getBatteryStatus().StartsWith("F"))
            {
                ImageSrc = "VisualElements/Battery_c/battery_tile_c_" + bHandler.getBatteryData().ToString("000") + ".jpg";
            }
            else
            {
                ImageSrc = "VisualElements/Battery_d/battery_tile_d_" + bHandler.getBatteryData().ToString("000") + ".jpg";
            }

            bgImage[0].Attributes[1].InnerText = ImageSrc;


            // Hogy néz ki az xml template????
            // StorageFile Xml;
            // Xml = await ApplicationData.Current.LocalFolder.GetFileAsync("tile.xml");
            //await tileXml.SaveToFileAsync(Xml);

            // Create the notification from the XML.
            TileNotification tileNotification = new TileNotification(tileXml);
            // Send the notification to the calling app's tile.
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);


        }

        private async void Register_BG_TasK()
        {

            try
            {
                var access = await BackgroundExecutionManager.RequestAccessAsync();
                while (access == BackgroundAccessStatus.Denied)
                {
                    access = await BackgroundExecutionManager.RequestAccessAsync();
                }
               
                foreach (var task_ in BackgroundTaskRegistration.AllTasks)
                {
                    if (task_.Value.Name == "BGTask")
                    {
                        Debug.WriteLine("BGTask is already running!");
                        task_.Value.Unregister(true);
                        Debug.WriteLine("KILLED!");
                    }
                }

                var task = new BackgroundTaskBuilder
                {
                    Name = "BGTask",
                    TaskEntryPoint = "Tasks.SampleBackgroundTask"
                };

                var trigger = new ApplicationTrigger();

                task.SetTrigger(trigger);


                task.Register();

                await trigger.RequestAsync();


                //Debug.WriteLine("Running Tasks : ");
                //foreach (var task_ in BackgroundTaskRegistration.AllTasks)
                //{
                //    Debug.WriteLine(task_.Value.Name);
                //}


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);


            }

        }

    }
}
