using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mapa_slepa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        Random rnd = new Random();
        MapPoint activePoint;
        int score = 0;
        int round = 1;
        List<MapPoint> points = new List<MapPoint>()
        {
            new MapPoint() { Name = "Město 1", XPercent = 0.365, YPercent = 0.39, answer = "Praha" }, // Praha
            new MapPoint() { Name = "Město 2", XPercent = 0.685, YPercent = 0.71, answer = "Brno"}, // Brno
            new MapPoint() { Name = "Město 3", XPercent = 0.047, YPercent = 0.32, answer = "Aš" }, // Aš
        };

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawPoints();
            NextRound();
        }

        private void MapImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawPoints();
        }

        void DrawPoints()
        {
            OverlayCanvas.Children.Clear();

            foreach (var point in points)
            {
                double x = MapImage.ActualWidth * point.XPercent;
                double y = MapImage.ActualHeight * point.YPercent;

                Button btn = new Button()
                {
                    Content = point.Name,
                    Tag = point
                };

                btn.Click += Btn_Click;

                Canvas.SetLeft(btn, x);
                Canvas.SetTop(btn, y);

                OverlayCanvas.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MapPoint point = btn.Tag as MapPoint;

            if (point == activePoint)
            {
                score++;
                MessageBox.Show("Správně!");
            }
            else
            {
                MessageBox.Show($"Špatně!");
            }

            round++;
            NextRound();
        }
        private void MapImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MapImage);

            double xPercent = pos.X / MapImage.ActualWidth;
            double yPercent = pos.Y / MapImage.ActualHeight;

            MessageBox.Show($"{xPercent:F4} , {yPercent:F4}");
        }

        void NextRound()
        {
            if (round > points.Count)
            {
                MessageBox.Show($"Konec hry! Skóre: {score}/{points.Count}");
                Close();
                return;
            }

            activePoint = points[rnd.Next(points.Count)];
            Question.Text = $"Najdi město: {activePoint.answer}";
            Points.Text = $"Skóre: {score}";
            Turn.Text = $"Kolo {round}";
        }
    }
}