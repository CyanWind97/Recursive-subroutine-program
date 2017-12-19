using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Recursive_subroutine_program
{
    public partial class Form1 : Form
    {
        
        private readonly SeriesCollection _series1;


        public Form1()
        {
            InitializeComponent();

            _series1 = new SeriesCollection
            {
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>(),
                    MinPointShapeDiameter = 1,
                    MaxPointShapeDiameter = 10
                    //PointGeometry = DefaultGeometries.Cross
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = false
                }
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = false
                }
            });
        }


        public void DrawPoint(double x, double y)
        {
            cartesianChart1.Series = _series1;

            foreach(var series in _series1)
            {
                series.Values.Add(new ScatterPoint(x, y, 1));
            }
           
        }
    }
}
