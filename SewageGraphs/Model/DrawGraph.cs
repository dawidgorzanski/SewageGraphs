using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SewageGraphs.Model
{
    public class DrawGraph
    {
        private Canvas _canvas;
        public Graph CurrentGraph { get; set; }
        public int NodeRadius { get; set; }

        public DrawGraph(Canvas canvas, Graph graph)
        {
            this.CurrentGraph = graph;
            this._canvas = canvas;
            this.NodeRadius = 10;
        }

        //rysowanie poziomów
        private void DrawLevels()
        {
            for(int i = 0; i < CurrentGraph.Levels; ++i)
            {
                Rectangle rec = new Rectangle();
                rec.Height = _canvas.ActualHeight - 40;
                rec.Width = (_canvas.ActualWidth - 20) / CurrentGraph.Levels;
                rec.StrokeThickness = 2;
                rec.Stroke = Brushes.LightGray;

                Canvas.SetTop(rec, 20);
                Canvas.SetLeft(rec, i * rec.Width + 10);
                _canvas.Children.Add(rec);

                Label label = new Label();
                label.Content = "POZIOM" + i;
                label.Foreground = Brushes.Gray;
                label.FontSize = 16;
                Canvas.SetTop(label, -5);
                Canvas.SetLeft(label, i * rec.Width + rec.Width / 2.5);
                _canvas.Children.Add(label);
            }
        }

        private void CreateLabeledEllipse(Node node, double x, double y, out Ellipse ellipse, out Label label)
        {
            ellipse = new Ellipse();
            ellipse.Height = NodeRadius;
            ellipse.Width = NodeRadius;
            ellipse.Fill = Brushes.Red;
            ellipse.Stroke = Brushes.Red;
            Canvas.SetTop(ellipse, y);
            Canvas.SetLeft(ellipse, x);

            label = new Label();
            label.Content = node.ID;
            label.FontSize = 16;
            label.Foreground = Brushes.Black;
            Canvas.SetTop(label, y - 20);
            Canvas.SetLeft(label, x - 15);
        }
        //rysowanie punktów
        private void DrawNodes()
        {
            double width = (_canvas.ActualWidth - 20) / CurrentGraph.Levels;

            Node startNode = CurrentGraph.Nodes.Single(x => x.IsStart == true);          
            double currentX = width / 2 + 10;
            double currentY = _canvas.ActualHeight / 2 + NodeRadius / 2;
            Ellipse ellipse;
            Label label;

            CreateLabeledEllipse(startNode, currentX, currentY, out ellipse, out label);

            _canvas.Children.Add(ellipse);
            _canvas.Children.Add(label);
            startNode.PointOnScreen = new Point(currentX, currentY);

            Node endNode = CurrentGraph.Nodes.Single(x => x.IsEnd == true);
            currentX = width * (CurrentGraph.Levels - 1) + width / 2 + 10;
            currentY = _canvas.ActualHeight / 2;

            CreateLabeledEllipse(endNode, currentX, currentY, out ellipse, out label);

            _canvas.Children.Add(ellipse);
            _canvas.Children.Add(label);
            endNode.PointOnScreen = new Point(currentX, currentY);

            for (int i = 1; i < CurrentGraph.Levels - 1; ++i)
            {
                var onThisLevel = CurrentGraph.Nodes.Where(x => x.Level == i);
                for(int j = 0; j < onThisLevel.Count(); ++j)
                {
                    Node node = onThisLevel.ElementAt(j);
                    currentX = width * i + width / 2 + 10;
                    currentY = _canvas.ActualHeight / onThisLevel.Count() * j + _canvas.ActualHeight / onThisLevel.Count() / 2;

                    CreateLabeledEllipse(node, currentX, currentY, out ellipse, out label);

                    _canvas.Children.Add(ellipse);
                    _canvas.Children.Add(label);
                    node.PointOnScreen = new Point(currentX, currentY);
                }
            }
        }

        //rysowanie pełnego grafu
        public bool Draw()
        {
            if (CurrentGraph == null || CurrentGraph.Nodes.Count == 0)
                return false;

            //Rysowanie punktów
            DrawNodes();

            //Rysowanie linii
            foreach (Connection connection in CurrentGraph.Connections)
            {
                DrawArrow(connection);
            }

            //Rysowanie poziomów
            DrawLevels();

            return true;
        }

        //rysowanie linii od punktu node1 do punktu node2
        private void DrawArrow(Connection connection)
        {
            Point p1 = new Point(connection.Node1.PointOnScreen.X + NodeRadius / 2, connection.Node1.PointOnScreen.Y + NodeRadius / 2);
            Point p2 = new Point(connection.Node2.PointOnScreen.X + NodeRadius / 2, connection.Node2.PointOnScreen.Y + NodeRadius / 2);

            GeometryGroup lineGroup = new GeometryGroup();
            double theta = Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            Point p = new Point(p1.X + ((p2.X - p1.X) / 1.35), p1.Y + ((p2.Y - p1.Y) / 1.35));
            pathFigure.StartPoint = p;

            Point lpoint = new Point(p.X + 6, p.Y + 15);
            Point rpoint = new Point(p.X - 6, p.Y + 15);
            LineSegment seg1 = new LineSegment();
            seg1.Point = lpoint;
            pathFigure.Segments.Add(seg1);

            LineSegment seg2 = new LineSegment();
            seg2.Point = rpoint;
            pathFigure.Segments.Add(seg2);

            LineSegment seg3 = new LineSegment();
            seg3.Point = p;
            pathFigure.Segments.Add(seg3);

            pathGeometry.Figures.Add(pathFigure);

            RotateTransform transform = new RotateTransform();
            transform.Angle = theta + 90;
            transform.CenterX = p.X;
            transform.CenterY = p.Y;
            pathGeometry.Transform = transform;
            lineGroup.Children.Add(pathGeometry);

            LineGeometry connectorGeometry = new LineGeometry();
            connectorGeometry.StartPoint = p1;
            connectorGeometry.EndPoint = p2;
            lineGroup.Children.Add(connectorGeometry);

            Path path = new Path();
            path.Data = lineGroup;
            path.StrokeThickness = 1.0;
            if (connection.IsBestPath)
                path.Stroke = path.Fill = Brushes.Red;
            else
                path.Stroke = path.Fill = Brushes.Blue;

            //Insert() zamiast Add(), aby linie były "pod spodem" - liczy się kolejność dodawania, im dalej na liście tym "wyżej"
            _canvas.Children.Insert(0, path);

            Label label = new Label();
            label.Foreground = Brushes.Blue;
            label.Content = connection.Weight;
            Canvas.SetLeft(label, p.X);
            Canvas.SetTop(label, p.Y);
            _canvas.Children.Add(label);
        }

        public void ClearAll(bool OnlyView = true)
        {
            if (!OnlyView)
                CurrentGraph = null;

            _canvas.Children.Clear();
        }
    }
}
