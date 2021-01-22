using System.Collections.ObjectModel;

namespace SismoNica_WP8._0.Model
{
    public class QuakeEventArgs
    {
        public ObservableCollection<QuakeItem> Results { get; private set; }

        public QuakeEventArgs(ObservableCollection<QuakeItem> results)
        {
            this.Results = results;
        }
    }
}