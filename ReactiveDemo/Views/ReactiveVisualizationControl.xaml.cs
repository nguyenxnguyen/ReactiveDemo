using ReactiveDemo.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ReactiveDemo.Models.MyShapes;
using static ReactiveDemo.ViewModels.ReactiveVisualizationViewModel;

namespace ReactiveDemo.Views
{
    /// <summary>
    /// Interaction logic for ReactiveVisualizationControl.xaml
    /// </summary>
    public partial class ReactiveVisualizationControl : IViewFor<ReactiveVisualizationViewModel>
    {
        public static readonly DependencyProperty _viewModel =
               DependencyProperty.Register("ViewModel", typeof(ReactiveVisualizationViewModel), typeof(ReactiveVisualizationControl));

        public ReactiveVisualizationViewModel ViewModel
        {
            get { return GetValue(_viewModel) as ReactiveVisualizationViewModel; }
            set { SetValue(_viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as ReactiveVisualizationViewModel; }
        }

        public Dictionary<int, MyCircle> MyDict;

        public List<int> Elements;

        public ReactiveVisualizationControl()
        {
            InitializeComponent();
            MyDict = new Dictionary<int, MyCircle>();
            Elements = new List<int>();

            var myApp = ((App)Application.Current);
            myApp.DrawBallEventSpotted
                .Where(tup => Elements.Contains(tup.Item1))
                .Subscribe(tup =>
                {
                    DrawABall(tup);
                });

            myApp.EraseBallEventSpotted
                .Where(number => Elements.Contains(number))
                .Subscribe(tup =>
                {
                    //EraseABall(tup);
                });

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.DisplayTake, v => v.TakeTextBox).DisposeWith(d);
                this.Bind(ViewModel, vm => vm.DisplaySkip, v => v.SkipTextBox).DisposeWith(d);

                this.WhenAnyValue(v => v.ViewModel.Take, v => v.ViewModel.Skip)
                .Where(ts => ts.Item1 != 0 && ts.Item2 != 0)
                .Subscribe(ts => GetElements(ts.Item1, ts.Item2)).DisposeWith(d);
            });
        }

        private void DoThing(ThingsToDo thing)
        {
            switch (thing)
            {
                case ThingsToDo.Select:
                    break;
            }
        }

        private void GetElements(int take, int skip)
        {
            for(int i = 0; i <= 10; i += skip)
            {
                Elements.Add(i);
                if (Elements.Count() == take)
                {
                    break;
                }
            }
        }

        private void DrawABall(Tuple<int, int> tup)
        {
            var circleSize = 30;
            var circleNumber = tup.Item1;
            var circleColumn = tup.Item2;
            var circleRow = 2;

            Position circlePos = new Position(circleColumn, circleRow);
            MyCircle myCircle = new MyCircle(circleSize, circleNumber, circlePos);

            if (MyDict.ContainsKey(circleNumber) && MyDict[circleNumber] != myCircle)
            {
                if (circleColumn > 19)
                {
                    EraseABall(circleNumber);
                    return;
                }
                EraseABall(circleNumber);
                MyDict[circleNumber] = myCircle;
            }
            else
            {
                MyDict.Add(circleNumber, myCircle);
            }

            // Add Ellipse to the Grid.
            var index1 = CircleGrid.Children.Add(myCircle.Circle);
            Grid.SetColumn(myCircle.Circle, myCircle.Position.X);
            Grid.SetRow(myCircle.Circle, myCircle.Position.Y);
            var index2 = CircleGrid.Children.Add(myCircle.TextInside);
            Grid.SetColumn(myCircle.TextInside, myCircle.Position.X);
            Grid.SetRow(myCircle.TextInside, myCircle.Position.Y);
        }

        private void EraseABall(int circleNumber)
        {
            var oldCircle = MyDict[circleNumber];
            CircleGrid.Children.Remove(oldCircle.Circle);
            CircleGrid.Children.Remove(oldCircle.TextInside);
        }
    }
}
