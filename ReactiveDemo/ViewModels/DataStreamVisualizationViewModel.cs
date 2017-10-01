using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using static ReactiveDemo.Models.MyShapes;

namespace ReactiveDemo.ViewModels
{
    public class DataStreamVisualizationViewModel : ReactiveObject
    {
        private const int MaxCol = 20;

        private bool _running;
        public bool Running
        {
            get { return _running; }
            set { this.RaiseAndSetIfChanged(ref _running, value); }
        }

        private MyCircle _newBall;
        public MyCircle NewBall
        {
            get { return _newBall; }
            set { this.RaiseAndSetIfChanged(ref _newBall, value); }
        }

        private MyCircle _oldBall;
        public MyCircle OldBall
        {
            get { return _oldBall; }
            set { this.RaiseAndSetIfChanged(ref _oldBall, value); }
        }

        private List<MyCircle> _currentBalls = new List<MyCircle>();
        public List<MyCircle> CurrentBalls
        {
            get { return _currentBalls; }
            set { this.RaiseAndSetIfChanged(ref _currentBalls, value); }
        }

        public ReactiveCommand<Unit, Unit> RunCommand;
        public ReactiveCommand<Unit, Unit> StopCommand;

        public DataStreamVisualizationViewModel()
        {
            RunCommand = ReactiveCommand.CreateFromTask(Run);
            StopCommand = ReactiveCommand.Create(Stop);
        }

        private async Task AddAnEllipse(int number)
        {
            var circleSize = 30;
            var circleNumber = number;

            for (int i = 0; i < MaxCol; i++)
            {
                Position circlePos = new Position(i, 1);
                MyCircle myCircle = new MyCircle(circleSize, circleNumber, circlePos);
                // Add Ellipse to the Grid.
                NewBall = myCircle;
                CurrentBalls.Add(NewBall);
                var td = number * 100;
                await Task.Delay(td);
                OldBall = myCircle;
                CurrentBalls.Remove(OldBall);
            }
        }

        private async Task AddAllEllipses()
        {
            Running = true;
            while (Running)
            {
                var taskList = new List<Task>();
                for (int i = 0; i < 10; i++)
                {
                    taskList.Add(AddAnEllipse(i));
                }
                await Task.WhenAll(taskList.ToArray());
            }        
        }

        private async Task Run()
        {
            Running = true;
            await AddAllEllipses();
        }

        private void Stop()
        {
            Running = false;
            foreach(var ball in CurrentBalls)
            {
                OldBall = ball;
            }
        }
       
    }
}
