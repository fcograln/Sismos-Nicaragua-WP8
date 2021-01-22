using Microsoft.Phone.Shell;
using SismoNica_WP8._0.Model;
using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SismoNica_WP8._0.ViewModel
{
    public class QuakeViewModel
    {
        public IsolatedStorageSettings IsolatedStorage { get; private set; }
        public QuakeModel QuakeAPIModel;
        public ObservableCollection<QuakeItem> AllQuakeObjects { get; private set; }
        public Action QuakeDataDone { get; set; }
         
        public QuakeViewModel()
        {
            QuakeAPIModel = new QuakeModel();
            IsolatedStorage = IsolatedStorageSettings.ApplicationSettings;

            if (IsolatedStorage.Count == 0 || IsolatedStorage.Contains("iDataItemList"))
            {
                IsolatedStorage.Clear();
                IsolatedStorage.Add("AllQuakeItems", new ObservableCollection<QuakeItem>());
                IsolatedStorage.Save();
            }
        }


        public static Color GetColorFromHex(string hexColor)
        {
            return Color.FromArgb(Convert.ToByte(hexColor.Substring(1, 2), 16), Convert.ToByte(hexColor.Substring(3, 2), 16), Convert.ToByte(hexColor.Substring(5, 2), 16), Convert.ToByte(hexColor.Substring(7, 2), 16));
        }
        public void RefreshQuakeData()
        {
            if (App.NetworkHelper.ConnectionStatus)
            {
                QuakeAPIModel.GetQuakeDataCompleted += QuakeModel_GetQuakeDataCompleted;
                QuakeAPIModel.GetQuakeData();
            }

            else
            {
                MessageBox.Show("Para descargar la información más actualizada, debe conectarse a Internet.", "¡ATENCIÓN!", MessageBoxButton.OK);

                if (QuakeDataDone != null)
                    QuakeDataDone.Invoke();
            }
        }
        
        private void QuakeModel_GetQuakeDataCompleted(object sender, QuakeEventArgs e)
        {
            QuakeAPIModel.GetQuakeDataCompleted -= QuakeModel_GetQuakeDataCompleted;

            if (e != null)
            {
                IsolatedStorage["AllQuakeItems"] = e.Results;
                IsolatedStorage.Save();
            }

            else
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Para descargar la información más actualizada, debe conectarse a Internet.\nRevise que su conexión no este dando problemas", "¡ATENCIÓN!", MessageBoxButton.OK);
                });


            AllQuakeObjects = (ObservableCollection<QuakeItem>)IsolatedStorage["AllQuakeItems"];
            if (QuakeDataDone != null)
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    QuakeDataDone.Invoke();
                });
        }
    }
}