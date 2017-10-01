using ReactiveUI;
using System.Linq;
using System.Reactive.Linq;

namespace ReactiveDemo.ViewModels
{
    public class ReactiveVisualizationViewModel : ReactiveObject
    {
        public enum ThingsToDo
        {
            Select = 1,
        }

        private int _take;
        public int Take
        {
            get { return _take; }
            set { this.RaiseAndSetIfChanged(ref _take, value); }
        }

        private string _displayTake;
        public string DisplayTake
        {
            get { return _displayTake; }
            set { this.RaiseAndSetIfChanged(ref _displayTake, value); }
        }

        private int _skip;
        public int Skip
        {
            get { return _skip; }
            set { this.RaiseAndSetIfChanged(ref _skip, value); }
        }

        private string _displaySkip;
        public string DisplaySkip
        {
            get { return _displaySkip; }
            set { this.RaiseAndSetIfChanged(ref _displaySkip, value); }
        }

        public ReactiveVisualizationViewModel()
        {
            DisplayTake = "2";
            DisplaySkip = "1";

            this.WhenAnyValue(vm => vm.DisplayTake)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(t => ConvertToInt(t))
                .BindTo(this, vm => vm.Take);

            this.WhenAnyValue(vm => vm.DisplaySkip)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(s => ConvertToInt(s))
                .BindTo(this, vm => vm.Skip);
        }

        private int ConvertToInt(string str)
        {
            var i = 0;
            int.TryParse(str, out i);
            return i;
        }
    }
}
