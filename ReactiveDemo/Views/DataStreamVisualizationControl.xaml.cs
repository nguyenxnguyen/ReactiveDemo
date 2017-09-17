using ReactiveDemo.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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

namespace ReactiveDemo.Views
{
    /// <summary>
    /// Interaction logic for DataStreamVisualizationControl.xaml
    /// </summary>
    public partial class DataStreamVisualizationControl : UserControl
    {
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
            var myApp = ((App)Application.Current);
            myApp.DrawBallEventSpotted = DrawBallEventSpotted;
            myApp.EraseBallEventSpotted = EraseBallEventSpotted;
        }

        public async Task AddAnEllipse(int number)
        {                     
            var circleSize = 30;
            var circleNumber = number;
            
            for (int i = 0; i < MaxCol; i ++)
            {
                Position circlePos = new Position(i, 1);
                MyCircle myCircle = new MyCircle(circleSize, circleNumber, circlePos);
                // Add Ellipse to the Grid.
                var index1 = CircleGrid.Children.Add(myCircle.Circle);
                Grid.SetColumn(myCircle.Circle, myCircle.Position.X);
                Grid.SetRow(myCircle.Circle, myCircle.Position.Y);
                var index2 = CircleGrid.Children.Add(myCircle.TextInside);
                Grid.SetColumn(myCircle.TextInside, myCircle.Position.X);
                Grid.SetRow(myCircle.TextInside, myCircle.Position.Y);

                // for observing
                var tup = new Tuple<int, int>(number, i);
                drawBallEventSpotted.OnNext(tup);

                var td = number * 100;
                await Task.Delay(td);
                CircleGrid.Children.Remove(myCircle.Circle);
                CircleGrid.Children.Remove(myCircle.TextInside);
                eraseBallEventSpotted.OnNext(number);
            }           
        }

        public async Task<bool> AddAnEllipseAsync()
        {
            List<Task> TaskList = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                TaskList.Add(AddAnEllipse(i));
            }
            await Task.WhenAll(TaskList.ToArray());
            return true;
        }

        private async void ButtonRunClick(object sender, RoutedEventArgs e)
        {
            runButton.IsEnabled = false;
            Stop = false;
            while (!Stop)
            {
                var i = await AddAnEllipseAsync();
            }                       
        }

        private void ButtonStopClick(object sender, RoutedEventArgs e)
        {
            Stop = true;
            runButton.IsEnabled = true;
        }
    }
}
