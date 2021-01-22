using Microsoft.Phone.Controls;
using SismoNica_WP8._0.ViewModel;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace SismoNica_WP8._0.View
{
    public partial class SplashPage : PhoneApplicationPage
    {
        private QuakeViewModel tempViewModel;
        public SplashPage()
        {
            InitializeComponent();
            tempViewModel = (QuakeViewModel)Application.Current.Resources["GlobalQuakeViewModel"];
            this.Loaded += SplashPage_Loaded;
        }

        private void SplashPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= SplashPage_Loaded;

            tempViewModel.QuakeDataDone = NavigateToMainPage;
            tempViewModel.RefreshQuakeData();
        }

        private void NavigateToMainPage()
        {
            tempViewModel.QuakeDataDone = null;
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}