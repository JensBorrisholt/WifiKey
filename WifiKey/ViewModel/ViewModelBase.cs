using System;
using System.ComponentModel;

namespace WifiKey.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private readonly object _notifyingObjectIsChangedSyncRoot = new object();
        private bool _notifyingObjectIsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModelBase()
        {
            PropertyChanged += OnNotifiedOfPropertyChanged;
        }

        public bool IsChanged
        {
            get
            {
                lock (_notifyingObjectIsChangedSyncRoot)
                    return _notifyingObjectIsChanged;
            }

            protected set
            {
                lock (_notifyingObjectIsChangedSyncRoot)
                {
                    if (Equals(_notifyingObjectIsChanged, value))
                        return;

                    _notifyingObjectIsChanged = value;
                    OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        protected virtual void OnPropertyChanged(params string[] argsname)
        {
            foreach (var name in argsname)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnNotifiedOfPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && !string.Equals(e.PropertyName, nameof(IsChanged), StringComparison.Ordinal))
                IsChanged = true;
        }
    }
}
