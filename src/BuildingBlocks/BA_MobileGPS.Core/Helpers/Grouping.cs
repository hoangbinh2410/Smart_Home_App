using System.Collections.ObjectModel;
using System.Linq;

namespace BA_MobileGPS.Core
{
    public class Grouping<S, T> : ObservableCollection<T>
    {
        private readonly S _key;

        public Grouping(IGrouping<S, T> group)
            : base(group)
        {
            _key = group.Key;
        }

        public S Key
        {
            get { return _key; }
        }

        public static ObservableCollection<T> All { private set; get; }
    }
}