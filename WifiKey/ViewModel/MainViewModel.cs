using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WifiKey.Network;
using WifiKey.Splash;

namespace WifiKey.ViewModel
{
    public sealed class MainViewModel : ViewModelBase
    {
        private ICommand _removeItem;
        private ICommand _copyPassword;
        private WifiNetwork _selectedItem;

        public MainViewModel()
        {
            Networks = new WifiNetworkList();
            Networks.OnNetworkNameFound += (sender, networkName) => MessageListener.Instance.ReceiveMessage($"Getting Password for {networkName}");
            Networks.OnEndScanning += (s, e) => Splasher.CloseSplash();
            Networks.FillList();
            Networks.CollectionChanged += (sender, args) => SelectedItem = Networks.Count > 0 ? Networks[0] : null;
        }

        public WifiNetworkList Networks { get; }

        public WifiNetwork SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand RemoveItem => _removeItem ?? (_removeItem = new RelayCommand(() => Networks?.RemoveProfile(SelectedItem)));

        public ICommand CopyPassword => _copyPassword ?? (_copyPassword = new RelayCommand(DoCopyPassword));

        private void DoCopyPassword()
        {
            if (SelectedItem == null)
                return;

            try
            {
                Clipboard.Clear();
                Clipboard.SetDataObject(SelectedItem.Password, true);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
