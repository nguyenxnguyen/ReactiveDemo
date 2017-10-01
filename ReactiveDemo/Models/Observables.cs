using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveDemo.Models
{
    public class Observables
    {
        public IObservable<Tuple<int, int>> DrawBallEventSpotted;
        public IObservable<int> EraseBallEventSpotted;
    }
}
