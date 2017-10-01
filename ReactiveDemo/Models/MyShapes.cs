using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ReactiveDemo.Models
{
    public class MyShapes
    {
        public class Position
        {
            public int X;
            public int Y;

            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public class MyCircle
        {
            public int Size;
            public int Number;

            private TextBlock _textInside;
            public TextBlock TextInside
            {
                get
                {
                    if (_textInside == null)
                    {
                        _textInside = new TextBlock();
                        _textInside.Text = Number.ToString();
                        _textInside.FontSize = 12;
                        _textInside.FontWeight = FontWeights.Bold;
                        _textInside.VerticalAlignment = VerticalAlignment.Center;
                        _textInside.HorizontalAlignment = HorizontalAlignment.Center;
                    }             
                    return _textInside;
                }
                set
                {
                    _textInside = value;
                }
            }
            
            private Ellipse _circle;
            public Ellipse Circle
            {
                get
                {
                    if (_circle == null)
                    {
                        _circle = NewCircle();
                    }
                    return _circle;
                }
                set
                {
                    _circle = value;
                }
            }

            public Position Position;

            public MyCircle(int size, int number, Position pos)
            {
                Size = size;
                Number = number;
                Position = pos;
            }

            public MyCircle()
            {

            }

            private Ellipse NewCircle()
            {
                var circle = new Ellipse();
                circle.Width = circle.Height = Size;

                // Create a blue and a black Brush
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = SetColor(Number);
                SolidColorBrush blackBrush = new SolidColorBrush();
                blackBrush.Color = Colors.Black;

                // Set Ellipse's width and color
                circle.StrokeThickness = 4;
                circle.Stroke = blackBrush;
                // Fill rectangle with blue color
                circle.Fill = blueBrush;
                return circle;
            }

            private Color SetColor(int cIndex)
            {
                switch (cIndex)
                {
                    case 1: return Colors.Red;
                    case 2: return Colors.Orange;
                    case 3: return Colors.Cyan;
                    case 4: return Colors.Yellow;
                    case 5: return Colors.Purple;
                    case 6: return Colors.Green;
                    case 7: return Colors.Gray;
                    case 8: return Colors.Wheat;
                    case 9: return Colors.Brown;
                    case 10: return Colors.Aqua;

                    default: return Colors.White;
                }
            }
        }
    }
}
