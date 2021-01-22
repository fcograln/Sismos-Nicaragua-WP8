using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;

namespace SismoNica_WP8._0.Model
{
    public class QuakeModel
    {
        public event EventHandler<QuakeEventArgs> GetQuakeDataCompleted;
        
        public void GetQuakeData()
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(@"http://sismosnicaragua.herokuapp.com/");
                webRequest.BeginGetResponse(GetQuakeDataCallBack, webRequest);
            }
            catch
            {
                GetQuakeDataCompleted(this, null);
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
                        GetQuakeDataCompleted(this, new QuakeEventArgs(JsonConvert.DeserializeObject<ObservableCollection<QuakeItem>>(reader.ReadToEnd())));
                }

                catch { GetQuakeDataCompleted(this, null); }
            }

            else
                GetQuakeDataCompleted(this, null);
        }
    }
}