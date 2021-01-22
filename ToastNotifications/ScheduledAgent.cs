using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;

using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Shell;
using System.Windows.Media;



namespace ToastNotifications
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
       
        public static NetworkStatusHelper NetworkHelper { get; private set; }
        private List<QuakeItem> QuakeItems;
        static ScheduledAgent()
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });

            NetworkHelper = new NetworkStatusHelper();
        }

        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
            
                Debugger.Break();
            }
        }

        public static Color GetColorFromHex(string hexColor)
        {
            return Color.FromArgb(Convert.ToByte(hexColor.Substring(1, 2), 16), Convert.ToByte(hexColor.Substring(3, 2), 16), Convert.ToByte(hexColor.Substring(5, 2), 16), Convert.ToByte(hexColor.Substring(7, 2), 16));
        }
        protected override void OnInvoke(ScheduledTask task)
        {
            ScheduledActionService.LaunchForTest("Sismos Nicaragua", TimeSpan.FromSeconds(10));
            if (NetworkHelper.ConnectionStatus)
            {
                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(@"http://sismosnicaragua.herokuapp.com/");
                    webRequest.BeginGetResponse(GetQuakeDataCallBack, webRequest);
                }
                catch
                {
                    NetworkHelper.UnWatchNetwork();
                    NotifyComplete();
                }
            }
            else
            {
                NetworkHelper.UnWatchNetwork();
               NotifyComplete();
            }
            
        }
        private void GetQuakeDataCallBack(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            if (request != null)
            {
                try
                {
                    using (StreamReader reader = new StreamReader((request.EndGetResponse(result)).GetResponseStream()))
                    {
                        QuakeItems = new List<QuakeItem>(JsonConvert.DeserializeObject<List<QuakeItem>>(reader.ReadToEnd()).TakeWhile(x => DateTime.Now.ToString("dd/MM/yyyy").CompareTo(x.date) == 0));

                        if (QuakeItems.Count > 0)
                        {
                            var main = ShellTile.ActiveTiles.First();
                            ShellToast Notification = new ShellToast()
                            {
                                Title = "Alerta:",
                                Content = "Nuevo sismo ocurrido hoy !",
                                NavigationUri = new Uri("/View/SplashPage.xaml", UriKind.RelativeOrAbsolute)
                            };
                            IconicTileData iconicTile = new IconicTileData()
                            {
                                BackgroundColor = GetColorFromHex("#FFD24726"),
                                Count = QuakeItems.Count,
                                IconImage = new Uri("Assets/Tiles/134x202.png", UriKind.RelativeOrAbsolute),
                                SmallIconImage = new Uri("Assets/Tiles/71x110.png", UriKind.RelativeOrAbsolute),
                                WideContent1 = "incidentes sísmicos ocurridos hoy !",

                            };
                            Notification.Show();
                            main.Update(iconicTile);
                            NetworkHelper.UnWatchNetwork();
                            NotifyComplete();

                        }


                    }

                }

                catch { NotifyComplete(); NetworkHelper.UnWatchNetwork(); }
            }

            else
            {
                NotifyComplete();
                NetworkHelper.UnWatchNetwork();
            }
               
        }
    }
}