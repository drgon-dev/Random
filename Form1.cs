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
                OutputBox.Text += $"��������� �����: {sim.carsServed}\n";
                OutputBox.Text += $"����� �������� ����: {sim.carsMissed}\n";
                OutputBox.Text += $"���������� �����������: {(double)sim.carsServed / (double)Constants.simulationTime:F2} ����/���\n";
                OutputBox.Text += $"������� ����� ��������: {(double)sim.totalWaitingTime / (double)sim.carsServed:F2} �����\n";
                OutputBox.Text += $"����������� ������������: {(double)sim.carsServed / (double)sim.carsAmount:F2}\n";
                OutputBox.Text += $"����������� ������: {(double)sim.carsMissed / (double)sim.carsAmount:F2}\n";
                OutputBox.Text += $"����������� ���������: {(double)sim.totalWaitingTime / (double)Constants.simulationTime:F2}\n\n";
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

            // ��������� ������� ��� ������ ��������
            ChartArea area1 = new ChartArea("��������� �����������");
            ChartArea area2 = new ChartArea("������� ����� ��������");
            ChartArea area3 = new ChartArea("����������� ������������");
            chart.ChartAreas.Add(area1);
            chart.ChartAreas.Add(area2);
            chart.ChartAreas.Add(area3);

            // ������ ���������� �����������
            Series queueSeries = new Series("��������� �����������")
            {
                ChartType = SeriesChartType.Spline,
                ChartArea = "��������� �����������"
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                queueSeries.Points.AddXY(i, sim.counter1[i]);
            }

            // ������ 
            Series waitSeries = new Series("������� ����� ��������")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "������� ����� ��������"
            };
            for (int i = 0; i < Constants.queueVar; i++)
            {
                waitSeries.Points.AddXY(i, sim.counter2[i]);
            }

            Series servingSeries = new Series("����������� ��c���������")
            {
                ChartType = SeriesChartType.Line,
                ChartArea = "����������� ������������"
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