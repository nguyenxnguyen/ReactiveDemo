using ReactiveDemo.ViewModels;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using static ReactiveDemo.Models.MyShapes;

namespace ReactiveDemo.Views
{
    /// <summary>
    /// Interaction logic for DataStreamVisualizationControl.xaml
    /// </summary>
    public partial class DataStreamVisualizationControl : IViewFor<DataStreamVisualizationViewModel>
    {
        public static readonly DependencyProperty _viewModel =
               DependencyProperty.Register("ViewModel", typeof(DataStreamVisualizationViewModel), typeof(DataStreamVisualizationControl));

        public DataStreamVisualizationViewModel ViewModel
        {
            get { return GetValue(_viewModel) as DataStreamVisualizationViewModel; }
            set { SetValue(_viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value as DataStreamVisualizationViewModel; }
        }
    
        public bool Stop = false;
        public const int MaxCol = 21;

        private Subject<Tuple<int, int>> drawBallEventSpotted = new Subject<Tuple<int, int>>();
        public IObservable<Tuple<int, int>> DrawBallEventSpotted
        {
            get { return this.drawBallEventSpotted.AsObservable(); }
        }

        private Subject<int> eraseBallEventSpotted = new Subject<int>();
        public IObservable<int> EraseBallEventSpotted
        {
            get { return this.eraseBallEventSpotted.AsObservable(); }
        }

        public DataStreamVisualizationControl()
        {
            InitializeComponent();

            ViewModel = new DataStreamVisualizationViewModel();

            var myObservables = ((App)Application.Current).MyObservables;
            myObservables.DrawBallEventSpotted = DrawBallEventSpotted;
            myObservables.EraseBallEventSpotted = EraseBallEventSpotted;

            this.WhenActivated(d => {
                this.BindCommand(ViewModel, vm => vm.RunCommand, v => v.runButton).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.StopCommand, v => v.stopButton).DisposeWith(d);
                this.WhenAnyValue(v => v.ViewModel.NewBall).Where(b => this.ViewModel.Running).Subscribe(DrawABall).DisposeWith(d);
                this.WhenAnyValue(v => v.ViewModel.OldBall).Where(b => b != null).Subscribe(EraseABall).DisposeWith(d);
            });
        }

        private void DrawABall(MyCircle myCircle)
        {
            var index1 = CircleGrid.Children.Add(myCircle.Circle);
            Grid.SetColumn(myCircle.Circle, myCircle.Position.X);
            Grid.SetRow(myCircle.Circle, myCircle.Position.Y);
            var index2 = CircleGrid.Children.Add(myCircle.TextInside);
            Grid.SetColumn(myCircle.TextInside, myCircle.Position.X);
            Grid.SetRow(myCircle.TextInside, myCircle.Position.Y);
            // for observing
            var tup = new Tuple<int, int>(myCircle.Number, myCircle.Position.X);
            drawBallEventSpotted.OnNext(tup);
        }

        private void EraseABall(MyCircle myCircle)
        {
            CircleGrid.Children.Remove(myCircle.Circle);
            CircleGrid.Children.Remove(myCircle.TextInside);

            eraseBallEventSpotted.OnNext(myCircle.Number);
        }
    }
}
