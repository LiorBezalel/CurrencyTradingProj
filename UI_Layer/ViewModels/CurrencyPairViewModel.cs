using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UI_Layer;

namespace TradingUI.ViewModels
{
    class CurrencyPairViewModel
    {
        private readonly Simulator simulator;
        public ObservableCollection<CurrencyPairModel> CurrencyPairs { get; private set; } = new();

        public CurrencyPairViewModel(String connectionString)
        {
            simulator = new Simulator(connectionString);
            Task.Run(InitializeAndStart);
        }
        private async Task InitializeAndStart()
        {
            await simulator.Initializer();
            simulator.Simulate();
            while (true)
            {
                var pairs = await simulator.GetLatestCurrencyPairs();

                App.Current.Dispatcher.Invoke(() =>
                {
                    CurrencyPairs.Clear();
                    foreach (var pair in pairs)
                        CurrencyPairs.Add(pair);
                });

                await Task.Delay(2000);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
