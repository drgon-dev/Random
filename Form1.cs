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
            for (int i = 0; i < 4; i++)
            {
                GasStationSimulation sim = new GasStationSimulation(true, Convert.ToInt32(queueTextBox.Text));

                OutputBox.Text += $"Обслужено машин: {sim.carsServed}\n";
                OutputBox.Text += $"Машин проехало мимо: {sim.carsMissed}\n";
                OutputBox.Text += $"Пропускная способность: {(double)sim.carsServed / (double)Constants.simulationTime:F2} авто/час\n";
                OutputBox.Text += $"Среднее время ожидания: {(double)sim.totalWaitingTime / (double)sim.carsServed:F2} часов\n";
                OutputBox.Text += $"Вероятность обслуживания: {(double)sim.carsServed / (double)sim.carsAmount:F2}\n";
                OutputBox.Text += $"Вероятность отказа: {(double)sim.carsMissed / (double)sim.carsAmount:F2}\n";
                OutputBox.Text += $"Вероятность занятости: {(double)sim.totalWaitingTime / (double)Constants.simulationTime:F2}\n\n";

                ShowCharts(ref sim);
            }
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
            ChartArea area1 = new ChartArea("QueueLength");
            ChartArea area2 = new ChartArea("WaitingTime");
            chart.ChartAreas.Add(area1);
            chart.ChartAreas.Add(area2);

            // График длины очереди
            Series queueSeries = new Series("Queue Length")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "QueueLength"
            };
            for (int i = 0; i < sim.arrivalTimes.Count && i < sim.queueLengths.Count; i++)
            {
                queueSeries.Points.AddXY(sim.arrivalTimes[i], sim.queueLengths[i]);
            }

            // График времени ожидания
            Series waitSeries = new Series("Waiting Time")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "WaitingTime"
            };
            for (int i = 0; i < sim.arrivalTimes.Count && i < sim.waitingTimes.Count; i++)
            {
                if (sim.waitingTimes[i] >= 0) // Пропускаем машины, которые уехали
                    waitSeries.Points.AddXY(sim.arrivalTimes[i], sim.waitingTimes[i]);
            }

            chart.Series.Add(queueSeries);
            chart.Series.Add(waitSeries);

            chartForm.Controls.Add(chart);
            Task.Run(() => Application.Run(chartForm));
        }

    }

}
