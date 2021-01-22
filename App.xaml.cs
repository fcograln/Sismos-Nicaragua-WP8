using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using SismoNica_WP8._0.Common;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using Telerik.Windows.Controls;

namespace SismoNica_WP8._0
{
    public partial class App : Application
    {
        public static PhoneApplicationFrame RootFrame { get; private set; }
        private bool phoneApplicationInitialized = false;
        public static NetworkStatusHelper NetworkHelper { get; private set; }
        public RadDiagnostics diagnostics;

        public App()
        {
            UnhandledException += Application_UnhandledException;

            InitializeComponent();

            InitializePhoneApplication();

            InitializeMyAppComponents();
            diagnostics = new RadDiagnostics();
            diagnostics.EmailTo = "fcograln@outlook.com";
            diagnostics.Init();
        }

        private void InitializeMyAppComponents()
        {
            NetworkHelper = new NetworkStatusHelper();
        }


        private void Start()
        {
            PeriodicTask BackgroundAgent = ScheduledActionService.Find("Sismos Nicaragua") as PeriodicTask;
            if (BackgroundAgent != null)
            {
                Stop();
            }
            else
            {
                BackgroundAgent = new PeriodicTask("Sismos Nicaragua");
                BackgroundAgent.Description = "Reporte de incidentes sísmicos ocurridos";
            }
            try
            {
                ScheduledActionService.Add(BackgroundAgent);
                ScheduledActionService.LaunchForTest("Sismos Nicaragua", TimeSpan.FromSeconds(10));
                BackgroundAgent = null;


            }
            catch (Exception)
            {


            }
        }
        private void Stop()
        {
            try
            {
                ScheduledActionService.Remove("Cines Siglo Nuevo");
            }

            catch (Exception) { }
        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            ApplicationUsageHelper.Init("1.0");
            Stop();
            //var channel = HttpNotificationChannel.Find("SismosNicaragua");
            //if (channel == null)
            //{
            //    channel = new HttpNotificationChannel("SismosNicaragua");
            //    channel.Open();
            //    channel.BindToShellToast();
            //}


            //channel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(async (o, args) =>
            //{
            //    var hub = new NotificationHub("SismosNicaragua", "Endpoint=sb://sismosnicaragua.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=5wcScuhb+wji538tsHy1pocd6FHcO0xpuzOdtjJoH8Y=");
            //    await hub.RegisterNativeAsync(args.ChannelUri.ToString());
            //});
            //channel.ShellToastNotificationReceived += (o, args) => VibrationDevice.GetDefault().Vibrate(TimeSpan.FromMilliseconds(300));


        }


        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            Stop();
            if (!e.IsApplicationInstancePreserved)
                InitializeMyAppComponents();

            else
                NetworkHelper.WatchNetwork();

        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            NetworkHelper.UnWatchNetwork();
            Start();
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            NetworkHelper.UnWatchNetwork();
            Start();
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NetworkHelper.UnWatchNetwork();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            NetworkHelper.UnWatchNetwork();
        }

        #region Phone application initialization

        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            RootFrame = new TransitionFrame() { Background = new SolidColorBrush(Colors.Transparent) };
            RootFrame.Navigated += CompleteInitializePhoneApplication;
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            RootFrame.Navigated += CheckForResetNavigation;

            phoneApplicationInitialized = true;
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            RootFrame.Navigated -= ClearBackStackAfterReset;

            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            while (RootFrame.RemoveBackEntry() != null)
            {
                ;
            }
        }

        #endregion

    }
}