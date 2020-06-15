using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BA_MobileGPS.Entities
{
    [Serializable]
    public class BaseModel : BindableBase
    {
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", params string[] relatedProperty)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            RaisePropertyChanged(propertyName);

            foreach (var property in relatedProperty)
            {
                RaisePropertyChanged(property);
            }

            return true;
        }
    }
}