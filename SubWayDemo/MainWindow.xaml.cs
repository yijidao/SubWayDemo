using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Windows.Threading;

namespace SubWayDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ReadXML();
            ShowSubWay();
            AddCar();
            //2020.71" y="939.38
            AddDashLine(new Point(2251.2, 939.38), new Point(2040.71, 939.38));
            //x = "2020.71" y = "824.59"
            AddDashLine(new Point(2251.2, 939.38), new Point(2040.71, 824.59));
            //x = "1753.3" y = "768.3"
            AddDashLine(new Point(2251.2, 939.38), new Point(1773.3, 768.3));
            //sx = "2888.2" sy = "876.98"
            AddDashLine(new Point(2251.2, 939.38), new Point(2908.2, 876.98));
            //x = "2412.4" y = "524.81"
            AddDashLine(new Point(2251.2, 939.38), new Point(2432.4, 524.81));
            //sx = "1113.83" sy = "481"
            AddTransferDashLine(new Point(935, 450), new Point(1113.83, 481));
            //sx = "1249.55" sy = "323.96"
            AddTransferDashLine(new Point(935, 450), new Point(1249.55, 323.96));
            //sx = "931.31" sy = "907.79"
            AddTransferDashLine(new Point(935, 450), new Point(931.31, 907.79));
            //sx = "1061.31" sy = "907.79"
            AddTransferDashLine(new Point(935, 450), new Point(1061.31, 907.79));

            //sx = "1191.31" sy = "907.79"
            AddTransferDashLine(new Point(935, 450), new Point(1191.31, 907.79));
            //sx = "1282.31" sy = "907.79"

            AddTransferDashLine(new Point(935, 450), new Point(1282.31, 907.79));

            //sx = "2031.11" sy = "1722.5"
            AddTransferDashLine(new Point(3235, 1550), new Point(2031.11, 1722.5));

            //sx = "2648.35" sy = "1503.58"
            AddTransferDashLine(new Point(3235, 1550), new Point(2648.35, 1503.58));

            //sx = "2894.83" sy = "1355.77"
            AddTransferDashLine(new Point(3235, 1550), new Point(2894.83, 1355.77));

            //sx = "3076.7" sy = "1218.49"
            AddTransferDashLine(new Point(3235, 1550), new Point(3076.7, 1218.49));

            //sx = "3366.34" sy = "1012.1800000000001"
            AddTransferDashLine(new Point(3235, 1550), new Point(3366.34, 1012.1800000000001));

            RunDashLine();

        }
        string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "sw.xml";
        XmlDocument doc = new XmlDocument();
        private List<DrawInfo> drInList = new List<DrawInfo>();

        public List<Path> AllPaths { get; set; } = new List<Path>();

        public List<DrawInfo> DrInList
        {
            get
            {
                return drInList;
            }

            set
            {
                drInList = value;
            }
        }
        private void ReadXML()
        {
            doc.Load(xmlPath);
            XmlNodeList xmlNodeList = doc.DocumentElement.GetElementsByTagName("g");
            foreach (XmlNode xn in xmlNodeList)
            {
                foreach (XmlNode item in xn.ChildNodes)
                {
                    DrawInfo drawInfo = null;
                    if (item.Name == "path")
                    {
                        drawInfo = new DrawInfo()
                        {
                            Type = "path",
                            PathData = item.Attributes["d"].InnerText,
                            ForColor = item.Attributes["stroke"].InnerText,
                            Tooltip = item.Attributes["lb"].InnerText,
                        };
                    }
                    if (item.Name == "text")
                    {
                        drawInfo = new DrawInfo()
                        {
                            Type = "text",
                            ForColor = item.Attributes["fill"].InnerText,
                            X = double.Parse(item.Attributes["x"].InnerText),
                            Y = double.Parse(item.Attributes["y"].InnerText),
                            Tooltip = item.InnerText,
                            FontWeight = item.Attributes["font-weight"].InnerText
                        };
                    }
                    if (item.Name == "ellipse")
                    {
                        drawInfo = new DrawInfo()
                        {
                            Type = "ellipse",
                            ForColor = item.Attributes["stroke"].InnerText,
                            X = double.Parse(item.Attributes["cx"].InnerText) - 6.5,
                            Y = double.Parse(item.Attributes["cy"].InnerText) - 6.5,
                            Tooltip = item.Attributes["name"].InnerText,
                        };
                    }
                    if (item.Name == "image")
                    {
                        drawInfo = new DrawInfo()
                        {
                            Type = "image",
                            X = double.Parse(item.Attributes["x"].InnerText),
                            Y = double.Parse(item.Attributes["y"].InnerText),
                            Id = item.Attributes["id"].InnerText,
                            Tooltip = item.Attributes["name"].InnerText
                        };
                    }
                    if (drawInfo != null)
                        DrInList.Add(drawInfo);
                }
            }
        }
        BrushConverter brushConverter = new BrushConverter();
        private void ShowSubWay()
        {
            foreach (DrawInfo item in DrInList)
            {
                if (item.Type == "path")
                {
                    Style style = win.FindResource("pathStyle") as Style;
                    Path path = new Path()
                    {
                        Style = style,
                        Data = Geometry.Parse(item.PathData),
                        Stroke = (Brush)brushConverter.ConvertFromString(item.ForColor),
                        ToolTip = item.Tooltip
                    };
                    path.MouseMove += Path_MouseMove;
                    path.MouseLeave += Path_MouseLeave;
                    grid.Children.Add(path);
                    AllPaths.Add(path);
                }
                if (item.Type == "text")
                {
                    Style style = win.FindResource("textblockStyle") as Style;
                    TextBlock textBlock = new TextBlock()
                    {
                        Style = style,
                        Foreground = (Brush)brushConverter.ConvertFromString(item.ForColor),
                        Margin = new Thickness() { Left = item.X, Top = item.Y },
                        Text = item.Tooltip,
                        FontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(item.FontWeight),
                    };
                    grid.Children.Add(textBlock);
                }
                if (item.Type == "ellipse")
                {
                    Style style = win.FindResource("ellipseStyle") as Style;
                    Ellipse ellipse = new Ellipse()
                    {
                        Style = style,
                        Stroke = (Brush)brushConverter.ConvertFromString(item.ForColor),
                        Margin = new Thickness() { Left = item.X, Top = item.Y },
                        ToolTip = item.Tooltip
                    };
                    ellipse.MouseMove += Ellipse_MouseMove;
                    ellipse.MouseLeave += Ellipse_MouseLeave;
                    grid.Children.Add(ellipse);
                }
                if (item.Type == "image")
                {
                    Style style = null;
                    if (item.Id == "svgjsImage12298" || item.Id == "svgjsImage12302")
                        style = win.FindResource("imgAir") as Style;
                    else
                        style = win.FindResource("imgTran") as Style;
                    Image img = new Image()
                    {
                        Style = style,
                        Margin = new Thickness() { Left = item.X, Top = item.Y },
                        ToolTip = item.Tooltip
                    };
                    img.MouseMove += Img_MouseMove;
                    img.MouseLeave += Img_MouseLeave;
                    grid.Children.Add(img);
                }
            }
        }

        private Image CreateCar(PathGeometry path)
        {
            var car = new Image();

            var drawing = new DrawingImage();
            var geometryDrawing = new GeometryDrawing { Brush = Brushes.LightSteelBlue };
            geometryDrawing.Pen = new Pen { Thickness = 1, Brush = Brushes.Black };

            var geometryGroup = new GeometryGroup();
            geometryGroup.Children.Add(new EllipseGeometry { Center = new Point(7, 10), RadiusX = 3, RadiusY = 3 });
            geometryGroup.Children.Add(new EllipseGeometry { Center = new Point(21, 10), RadiusX = 3, RadiusY = 3 });

            var combinedGeometry = new CombinedGeometry
            {
                Geometry1 = new RectangleGeometry { Rect = new Rect(0, 0, 28, 10) },
                Geometry2 = geometryGroup
            };
            geometryDrawing.Geometry = combinedGeometry;
            drawing.Drawing = geometryDrawing;
            car.Source = drawing;

            var trigger = new EventTrigger { RoutedEvent = LoadedEvent };

            var storyBoard = new Storyboard();
            storyBoard.Children.Add(new DoubleAnimationUsingPath
            {
                Source = PathAnimationSource.Y,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new TimeSpan(0, 0, 40),
                PathGeometry = path
            });

            storyBoard.Children.Add(new DoubleAnimationUsingPath
            {
                Source = PathAnimationSource.X,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new TimeSpan(0, 0, 40),
                PathGeometry = path
            });

            Storyboard.SetTargetProperty(storyBoard.Children[0], new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTargetProperty(storyBoard.Children[1], new PropertyPath("(Canvas.Left)"));

            Timeline.SetDesiredFrameRate(storyBoard, 3);

            var beginStoryBoard = new BeginStoryboard { Storyboard = storyBoard };

            trigger.Actions.Add(beginStoryBoard);

            car.Triggers.Add(trigger);
            return car;
        }

        private async void AddCar()
        {

            for (int i = 0; i < 5; i++)
            {
                foreach (var path in AllPaths)
                {
                    var sg = path.Data as StreamGeometry;
                    Dispatcher.Invoke(() =>
                    {
                        grid.Children.Add(CreateCar(sg.GetFlattenedPathGeometry()));
                    });

                }
                await Task.Delay(8000);
            }



            //StreamGeometry s = new StreamGeometry();
            //var sg = AllPaths.FirstOrDefault().Data as StreamGeometry;
            //var pg = sg.GetFlattenedPathGeometry();

            //xAnimation.PathGeometry = pg;
            //yAnimation.PathGeometry = pg;

        }

        public List<Path> AllDashLines { get; set; } = new List<Path>();

        private void AddDashLine(Point point1, Point point2)
        {
            var pathFigure = new PathFigure { StartPoint = point1 };
            pathFigure.Segments.Add(new BezierSegment
            {
                Point1 = new Point(point1.X - 100, point2.Y - 100),
                Point2 = point2,
                Point3 = point2
            }); ;

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            var path = new Path
            {
                Data = pathGeometry,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };
            path.StrokeDashArray.Add(14);
            path.StrokeDashArray.Add(6);

            grid.Children.Add(path);

            AllDashLines.Add(path);
        }

        private async void RunDashLine()
        {
            while (true)
            {
                await Task.Delay(180);
                foreach (var path in AllDashLines)
                {
                    path.StrokeDashOffset -= 4;
                }
            }
        }

        private void AddTransferDashLine(Point startPoint, Point point)
        {
            var pathGeometry = new PathGeometry();

            var pathFigure = new PathFigure { StartPoint = startPoint };
            pathFigure.Segments.Add(new LineSegment { Point = point });

            pathGeometry.Figures.Add(pathFigure);

            var path = new Path
            {
                Data = pathGeometry,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };

            path.StrokeDashArray.Add(14);
            path.StrokeDashArray.Add(6);

            grid.Children.Add(path);

            AllDashLines.Add(path);
        }

        private void Img_MouseLeave(object sender, MouseEventArgs e)
        {
            popEllipse.IsOpen = false;
        }

        private void Img_MouseMove(object sender, MouseEventArgs e)
        {
            Image img = (Image)sender;
            tbEllipse.Text = img.ToolTip.ToString();

            Point p = e.GetPosition(mainGrid);
            popEllipse.IsOpen = true;
            popEllipse.HorizontalOffset = p.X - 60;
            popEllipse.VerticalOffset = p.Y - mainGrid.ActualHeight - 40;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            popEllipse.IsOpen = false;
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            Ellipse ell = (Ellipse)sender;
            tbEllipse.Text = ell.ToolTip.ToString();

            Point p = e.GetPosition(mainGrid);
            popEllipse.IsOpen = true;
            popEllipse.HorizontalOffset = p.X - 60;
            popEllipse.VerticalOffset = p.Y - mainGrid.ActualHeight - 40;
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            Path path = (Path)sender;
            grpopPath.Background = path.Stroke;
            tbPath.Text = path.ToolTip.ToString();


            Point p = e.GetPosition(mainGrid);
            popPath.IsOpen = true;
            popPath.HorizontalOffset = p.X - 60;
            popPath.VerticalOffset = p.Y - mainGrid.ActualHeight - 40;
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            popPath.IsOpen = false;
        }
        private bool mouseDown;
        private Point mouseXY;
        private double min = 1, max = 5;
        private void ContentControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var con = sender as Canvas;
            if (con == null)
            {
                return;
            }
            con.CaptureMouse();

            mouseDown = true;
            mouseXY = e.GetPosition(con);
        }

        private void ContentControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var con = sender as Canvas;
            if (con == null)
            {
                return;
            }
            con.ReleaseMouseCapture();
            mouseDown = false;
        }

        private void ContentControl_MouseMove(object sender, MouseEventArgs e)
        {
            var con = sender as Canvas;
            if (con == null)
            {
                return;
            }
            if (mouseDown)
            {
                Domousemove(con, e);
            }
        }

        private void ContentControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var con = sender as Canvas;
            if (con == null)
            {
                return;
            }
            var point = e.GetPosition(con);
            var group = grid.FindResource("tfg") as TransformGroup;
            var delta = e.Delta * 0.001;
            DowheelZoom(group, point, delta);
        }

        private void Domousemove(Canvas con, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }
            var group = grid.FindResource("tfg") as TransformGroup;
            var transform = group.Children[1] as TranslateTransform;
            var position = e.GetPosition(con);
            transform.X -= mouseXY.X - position.X;
            transform.Y -= mouseXY.Y - position.Y;
            mouseXY = position;
        }

        private void DowheelZoom(TransformGroup group, Point point, double delta)
        {
            var pointToContent = group.Inverse.Transform(point);
            var scale = group.Children[0] as ScaleTransform;
            if (scale.ScaleX + delta < min) return;
            if (scale.ScaleY + delta > max) return;
            scale.ScaleX += delta;
            scale.ScaleY += delta;
            var translate = group.Children[1] as TranslateTransform;
            translate.X = -1 * ((pointToContent.X * scale.ScaleX) - point.X);
            translate.Y = -1 * ((pointToContent.Y * scale.ScaleY) - point.Y);
        }
    }
}
