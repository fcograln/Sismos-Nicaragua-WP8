using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ToastNotifications
{
    [DataContract]
    public class QuakeItem
    {
        private string _magnitude;
        private string _latitude;
        private string _longitude;
        private string _date;


        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string date
        {
            get { return _date; }

            set
            {
                _date = Convert.ToDateTime(value).ToString("dd/MM/yyyy");
            }
        }
        [DataMember]
        public string time { get; set; }
        [DataMember]
        public string longitude
        {
            get { return _longitude; }

            set
            {
                _longitude = Regex.Replace("-" + value, "[^0-9./]", string.Empty);
            }
        }
        [DataMember]
        public string latitude
        {
            get { return _latitude; }

            set
            {
                _latitude = Regex.Replace(value, "[^0-9./]", string.Empty);
            }
        }
        [DataMember]
        public string depth { get; set; }
        [DataMember]
        public string magnitude
        {
            get { return _magnitude; }

            set
            {
                if (value.Length > 1)
                {
                    scale = value;
                    scale = scale.Substring(scale.Length - 2);
                }

                _magnitude = Regex.Replace(value, "[^0-9./]", string.Empty);
            }
        }
       
        [DataMember]
        public string place { get; set; }
        public string scale { get; set; }

        public string description
        {
            get { return string.Format("{0} a {1} de profundidad a las {2} horas del día {3}", place, depth, time, date); }
        }
        public string Mapdescription
        {
            get { return string.Format("Se sintió un sismo de magnitud {0}{1}\nDescripción: {2}", magnitude, scale, description); }
        }

        public string Toast
        {
            get { return string.Format("{0} a {1}{2} a las {3} horas del día {4}", place, magnitude, scale, time, date); }
        }

    }
}