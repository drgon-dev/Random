using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            GasStationSimulation sim = new GasStationSimulation(false, 0);
            for (int i = 0; i < Constants.queueVar; i++)
            {
                sim.RunSimulation(i);
                OutputBox.Text += $"Обслужено машин: {sim.carsServed}\n";
                OutputBox.Text += $"Машин проехало мимо: {sim.carsMissed}\n";
                OutputBox.Text += $"Пропускная способность: {(double)sim.carsServed / (double)Constants.simulationTime:F2} авто/час\n";
                OutputBox.Text += $"Среднее время ожидания: {(double)sim.totalWaitingTime / (double)sim.carsServed:F2} часов\n";
                OutputBox.Text += $"Вероятность обслуживания: {(double)sim.carsServed / (double)sim.carsAmount:F2}\n";
                OutputBox.Text += $"Вероятность отказа: {(double)sim.carsMissed / (double)sim.carsAmount:F2}\n";
                OutputBox.Text += $"Вероятность занятости: {(double)sim.totalWaitingTime / (double)Constants.simulationTime:F2}\n\n";
                sim.Reset();
                sim.queueCapacity++;
            }
            ShowCharts(ref sim);
        }

        public void ShowCharts(ref GasStationSimulation sim)
        {
            if (sim == null)
            {
                MessageBox.Show("Bruh");
                return;
            }

            Form chartForm = new Form
            {
                Text = "Gas Station Simulation Results",
                Width = 800,
                Height = 600
            };

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill
            };

            // Добавляем области для разных графиков
            ChartArea area1 = new ChartArea("Пропуская способность");
            ChartArea area2 = new ChartArea("Среднее время ожидания");
            ChartArea area3 = new ChartArea("Вероятность обслуживания");
            chart.ChartAreas.Add(area1);
            chart.ChartAreas.Add(area2);
            chart.ChartAreas.Add(area3);

            // График пропускной способности
            Series queueSeries = new Series("Пропуская способность")
            {
                ChartType = SeriesChartType.Spline,
                ChartArea = "Пропуская способность"
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                queueSeries.Points.AddXY(i, sim.counter1[i]);
            }

            // График 
            Series waitSeries = new Series("Среднее время ожидания")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Среднее время ожидания"
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                waitSeries.Points.AddXY(i, sim.counter2[i]);
            }

            Series servingSeries = new Series("Вероятность обcлуживания")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "Вероятность обслуживания"
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                servingSeries.Points.AddXY(i, sim.counter3[i]);
            }

            chart.Series.Add(queueSeries);
            chart.Series.Add(waitSeries);
            chart.Series.Add(servingSeries);

            chartForm.Controls.Add(chart);
            Task.Run(() => Application.Run(chartForm));
        }

    }

}