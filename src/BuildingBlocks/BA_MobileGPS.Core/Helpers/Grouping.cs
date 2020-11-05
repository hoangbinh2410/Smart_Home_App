using System.Collections.Generic;
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

    public class ObservableGroupCollection<K, T> : ObservableCollection<T>
    {
        // NB: This is the GroupDisplayBinding above for displaying the header
        public K Key { get; private set; }

        public ObservableCollection<T> All { private set; get; }

        public ObservableGroupCollection(K key, List<T> items)
        {
            Key = key;
            All = new ObservableCollection<T>(items);
        }
    }
}