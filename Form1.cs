namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private static Random random = new Random();

        // ��������� ���������
        private const int simulationTime = 1000000; // ����� ����� ���������
        private static int QueueCapacity; // ���������� ���� � �������
        private static int carsAmount; // ���������� ���� �����
        private static int carsServed; // ���������� ����������� �����
        private static int carsMissed; // ���������� �����, ������� �������� ����

        private static double totalWaitingTime; // ����� ����� �������� � �������

        public Form1()
        {
            InitializeComponent();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            InitializeVar();
            // ������� ��� �������� ������� ��������� �������� �� ������ �������
            List<double> pumpEndTimes = new List<double> { 0, 0 };
            Queue<double> waitingQueue = new Queue<double>(); // ������� �����

            for (double currentTime = 0; currentTime < simulationTime;)
            {
                carsAmount++;
                // ��������� ������� �� ���������� ������� ������
                double arrivalTime = currentTime + Exponential();

                // ���������� ������� ���������
                currentTime = arrivalTime;

                // ��������, �������� �� ���� �� ���� �������
                int freePump = FindFreePump(pumpEndTimes, arrivalTime);

                if (freePump != -1)
                {
                    // ���� ������� ��������, �������� ��������
                    double refuelTime;
                    if (freePump == 0)
                    {
                        refuelTime = ExponentialFuel();
                    }
                    else
                    {
                        refuelTime = ExponentialFuelSec();
                    }
                    pumpEndTimes[freePump] = arrivalTime + refuelTime;
                    carsServed++;
                }
                else if (waitingQueue.Count < QueueCapacity)
                {
                    // ���� ��� ������� ������, �� ���� ����� � �������, ��������� ������ � �������
                    waitingQueue.Enqueue(arrivalTime);
                }
                else
                {
                    // ���� ��� ������� ������ � ������� ���������, ������ ��������� ����
                    carsMissed++;
                }

                // ��������, ����� �� ������ �� ������� ������ ��������
                while (waitingQueue.Count > 0 && FindFreePump(pumpEndTimes, currentTime) != -1)
                {
                    double waitingCarArrivalTime = waitingQueue.Dequeue();
                    double waitingTime = currentTime - waitingCarArrivalTime;
                    totalWaitingTime += waitingTime;

                    int freePumpNow = FindFreePump(pumpEndTimes, currentTime);
                    double refuelTime;
                    if (freePump == 0)
                    {
                        refuelTime = ExponentialFuel();
                    }
                    else
                    {
                        refuelTime = ExponentialFuelSec();
                    }
                    pumpEndTimes[freePumpNow] = currentTime + refuelTime;
                    carsServed++;
                }
            }
            // ����� �����������
            OutputBox.Text = $"��������� �����: {carsServed}\n";
            OutputBox.Text += $"����� �������� ����: {carsMissed}\n";
            OutputBox.Text += $"���������� �����������: {(double)carsServed / (double)simulationTime:F2} ����/���\n";
            OutputBox.Text += $"������� ����� ��������: {(double)totalWaitingTime / (double)carsServed:F2} �����\n";
            OutputBox.Text += $"����������� ������������: {(double)carsServed / (double)carsAmount:F2}\n";
            OutputBox.Text += $"����������� ������: {(double)carsMissed / (double)carsAmount:F2}\n";
            OutputBox.Text += $"����������� ���������: {(double)totalWaitingTime / (double)simulationTime:F2}\n";
            //OutputBox.Text += $"{:F2}\n";
        }
        // ����� ��� ������ ��������� �������
        private static int FindFreePump(List<double> pumpEndTimes, double arrivalTime)
        {
            for (int i = 0; i < pumpEndTimes.Count; i++)
            {
                if (pumpEndTimes[i] <= arrivalTime)
                {
                    return i;
                }
            }
            return -1; // ��� ������� ������
        }

        // ����� ��� ��������� ���������� ������� �� ����������������� �������������
        private static double Exponential()
        {
            return (-1 * ((double)1 / (double)10) * Math.Log(random.NextDouble()));
        }
        private static double ExponentialFuel()
        {
            return (-1 * ((double)1 / (double)5) * Math.Log(random.NextDouble()));
        }
        private static double ExponentialFuelSec()
        {
            return (-1 * ((double)1 / (double)4) * Math.Log(random.NextDouble()));
        }
        void InitializeVar()
        {
            random = new Random();

            QueueCapacity = Convert.ToInt32(QueueTextBox.Text);
            carsAmount = 0; // ���������� ���� �����
            carsServed = 0; // ���������� ����������� �����
            carsMissed = 0; // ���������� �����, ������� �������� ����

            totalWaitingTime = 0; // ����� ����� �������� � �������
        }
    }
}