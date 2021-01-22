using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using SismoNica_WP8._0.Model;
using SismoNica_WP8._0.ViewModel;
using System;
using System.Windows;
using System.Windows.Navigation;
using Telerik.Windows.Controls;

namespace SismoNica_WP8._0.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        private QuakeViewModel tempViewModel;
        
        public MainPage()
        {
            InitializeComponent();

            InteractionEffectManager.AllowedTypes.Add(typeof(RadDataBoundListBoxItem));
            this.MainDataList.SetValue(InteractionEffectManager.IsInteractionEnabledProperty, true);

            tempViewModel = (QuakeViewModel)Application.Current.Resources["GlobalQuakeViewModel"];
           
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();

            this.Loaded -= MainPage_Loaded;
        }

        private void ClickEventHandler(object sender, RoutedEventArgs e)
        {
            if (App.NetworkHelper.ConnectionStatus)
            {
                QuakeItem quakeItem = (sender as MenuItem).DataContext as QuakeItem;

                switch (((MenuItem)sender).Name)
                {
                    case "ShareItem":
                        ShareLinkTask shareStatusTask = new ShareLinkTask
                        {
                            Title = "Sismos Nicaragua para Windows Phone 8:",
                            Message = string.Format("Se sintió un sismo de magnitud {0}{1}\nDescripción: {2}", quakeItem.magnitude, quakeItem.scale, quakeItem.description),
                            LinkUri = new Uri(@"http://www.ineter.gob.ni/", UriKind.RelativeOrAbsolute)
                        };

                        shareStatusTask.Show();
                        shareStatusTask = null;

                        break;

                    case "ItemMenu":
                        EmailComposeTask Email = new EmailComposeTask
                        {
                            Subject = "Sismos Nicaragua para Windows Phone 8:",
                            Body = string.Format("Se sintió un sismo de magnitud {0}{1}\nDescripción: {2}\nMás información: http://www.ineter.gob.ni/", quakeItem.magnitude, quakeItem.scale, quakeItem.description)
                        };

                        Email.Show();
                        Email = null;

                        break;
                }
            }

            else
                MessageBox.Show("Por favor, conectese a Internet para compartir la noticia.\nRevise que su conexión no este dando problemas", "¡ATENCIÓN!", MessageBoxButton.OK);
        }

        private void AppBarClickEventHandler(object sender, EventArgs e)
        {
            switch (sender.GetType().FullName)
            {
                case "Microsoft.Phone.Shell.ApplicationBarMenuItem":
                    if (((ApplicationBarMenuItem)sender).Text == "calificar aplicación")
                    {
                        MarketplaceReviewTask RateApp = new MarketplaceReviewTask();
                        RateApp.Show();
                        RateApp = null;
                    }
                    else
                    {
                        EmailComposeTask emailComposeTask = new EmailComposeTask
                        {
                            Subject = "Sugerencia: Sismos Nicaragua para Windows Phone 8",
                            To = "fcograln@outlook.com"
                        };

                        emailComposeTask.Show();
                        emailComposeTask = null;
                    }

                    break;
            }
        }

        private void RefreshRequestedEventHandler(object sender, EventArgs e)
        {
            if (tempViewModel.QuakeDataDone == null)
                tempViewModel.QuakeDataDone = StopLoading;

            tempViewModel.RefreshQuakeData();
        }

        private void StopLoading()
        {
            this.MainDataList.StopPullToRefreshLoading(true, true);
        }

        private void MainDataList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MainDataList.ItemCount!=0)
            {
               string uri = string.Format("/View/LocationPage.xaml?selectedItem=" +tempViewModel.AllQuakeObjects.IndexOf(this.MainDataList.SelectedItem as QuakeItem));
               
               NavigationService.Navigate(new Uri(uri, UriKind.Relative));
               this.MainDataList.SelectedItem = null;  
            }
        }
       
    }
}