using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Recursive_subroutine_program
{
    public partial class Form1 : Form
    {
        
        private SeriesCollection Series1;


        public Form1()
        {
            InitializeComponent();

            Series1 = new SeriesCollection
            {
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        
                    },
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
            cartesianChart1.Series = Series1;

            foreach(var series in Series1)
            {
                series.Values.Add(new ScatterPoint(x, y, 1));
            }
           
        }
    }
}
