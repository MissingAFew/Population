using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Population
{
    public partial class Graph : Form
    {
        public Graph()
        {
            InitializeComponent();
        }

        private void Graph_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.chart1.Series["Population"].Points.Clear();
            this.chart2.Series["BirthDeath"].Points.Clear();

            LifespanData data = new LifespanData(50, 15, 1900, 2000, 100);

            for(int i = 0; i < data.population.Count; ++i)
            {
                this.chart1.Series["Population"].Points.AddXY(i + data.RangeStart, data.population[i]);
            }

            this.chart2.ChartAreas[0].AxisX.Minimum = data.RangeStart;
            this.chart2.ChartAreas[0].AxisX.Maximum = data.RangeEnd;

            this.chart2.ChartAreas[0].AxisY.Minimum = data.RangeStart;
            this.chart2.ChartAreas[0].AxisY.Maximum = data.RangeEnd;

            for (int i = 0; i < data.data.Count; ++i)
            {
                this.chart2.Series["BirthDeath"].Points.AddXY(data.data[i].birthDate, data.data[i].deathDate);
            }
        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
