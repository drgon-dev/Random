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
                OutputBox.Text += $"Вероятность занятости: {((double)sim.totalWaitingTime / (double)sim.queueCapacity) / (double)Constants.simulationTime:F2}\n\n";
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
                Text = "Результаты симуляций",
                Width = 1000,
                Height = 800
            };

            Chart chart = new Chart
            {
                Dock = DockStyle.Fill
            };

            // Добавляем области для разных графиков
            ChartArea area1 = new ChartArea("Пропуская способность");
            ChartArea area2 = new ChartArea("Среднее время ожидания");
            ChartArea area3 = new ChartArea("Вероятность обслуживания");
            ChartArea area4 = new ChartArea("Вероятность отказа");
            ChartArea area5 = new ChartArea("Вероятность занятости");
            chart.ChartAreas.Add(area1);
            chart.ChartAreas.Add(area2);
            chart.ChartAreas.Add(area3);
            chart.ChartAreas.Add(area4);
            chart.ChartAreas.Add(area5);

            chart.Legends.Add(new Legend());

            // График пропускной способности
            Series queueSeries = new Series("Пропуская способность")
            {
                ChartType = Constants.chartView,
                ChartArea = "Пропуская способность",
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                queueSeries.Points.AddXY(i, sim.counter1[i]);
            }

            // График среднего времени ожидания
            Series waitSeries = new Series("Среднее время ожидания")
            {
                ChartType = Constants.chartView,
                ChartArea = "Среднее время ожидания"
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                waitSeries.Points.AddXY(i, sim.counter2[i]);
            }

            //График вероятности обслуживания
            Series servingSeries = new Series("Вероятность обcлуживания")
            {
                ChartType = Constants.chartView,
                ChartArea = "Вероятность обслуживания",
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                servingSeries.Points.AddXY(i, sim.counter3[i]);
            }

            // График вероятности отказа
            Series denialSeries = new Series("Вероятность отказа")
            {
                ChartType = Constants.chartView,
                ChartArea = "Вероятность отказа",
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                denialSeries.Points.AddXY(i, sim.counter4[i]);
            }

            // График вероятности занятости
            Series usedSeries = new Series("Вероятность занятости")
            {
                ChartType = Constants.chartView,
                ChartArea = "Вероятность занятости",
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                usedSeries.Points.AddXY(i, sim.counter5[i]);
            }

            chart.Series.Add(queueSeries);
            chart.Series.Add(waitSeries);
            chart.Series.Add(servingSeries);
            chart.Series.Add(denialSeries);
            chart.Series.Add(usedSeries);

            chartForm.Controls.Add(chart);
            Task.Run(() => Application.Run(chartForm));
        }

    }

}