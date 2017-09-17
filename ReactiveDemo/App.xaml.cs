using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IObservable<Tuple<int, int>> DrawBallEventSpotted;
        public IObservable<int> EraseBallEventSpotted;
    }
}
