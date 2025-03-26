public class GasStationSimulation
{
    private Random random = new Random();

    private int queueCapacity; // Количество мест в очереди
    public int carsAmount; // Количество всех машин
    public int carsServed; // Количество обслуженных машин
    public int carsMissed; // Количество машин, которые проехали мимо

    public List<double> arrivalTimes = new List<double>();
    public List<double> waitingTimes = new List<double>();
    public List<int> queueLengths = new List<int>();

    public double totalWaitingTime; // Общее время ожидания в очереди

    private void Reset()
    {
        carsAmount = 0;
        carsServed = 0;
        carsMissed = 0;

        totalWaitingTime = 0;
        
        arrivalTimes.Clear();
        waitingTimes.Clear();
        queueLengths.Clear();
    }

    public GasStationSimulation(bool iRunAtStart = true, int capacity = 0)
    {
        if (iRunAtStart)
            RunSimulation(capacity);
    }

    public void RunSimulation(int capacity)
    {
        Reset();
        queueCapacity = capacity;

        // Очередь для хранения времени окончания заправки на каждой колонке
        List<double> pumpEndTimes = new List<double> { 0, 0 };
        Queue<double> waitingQueue = new Queue<double>(); // Очередь машин

        for (double currentTime = 0; currentTime < Constants.simulationTime;)
        {
            carsAmount++;
            double arrivalTime = currentTime + Exponential();
            currentTime = arrivalTime;

            // Сохраняем данные
            arrivalTimes.Add(arrivalTime);
            queueLengths.Add(waitingQueue.Count);

            int freePump = FindFreePump(pumpEndTimes, arrivalTime);

            if (freePump != -1)
            {
                // Если колонка свободна, начинаем заправку
                double refuelTime = freePump == 0 ? refuelTime = ExponentialFuel() : refuelTime = ExponentialFuelSec();

                pumpEndTimes[freePump] = arrivalTime + refuelTime;
                carsServed++;
                waitingTimes.Add(0);
            }
            else if (waitingQueue.Count < queueCapacity)
                waitingQueue.Enqueue(arrivalTime);
            else
            {
                carsMissed++;
                waitingTimes.Add(-1);
            }

            // Проверка, может ли машина из очереди начать заправку
            while (waitingQueue.Count > 0 && FindFreePump(pumpEndTimes, currentTime) != -1)
            {
                double waitingCarArrivalTime = waitingQueue.Dequeue();
                double waitingTime = currentTime - waitingCarArrivalTime;
                totalWaitingTime += waitingTime;

                int freePumpNow = FindFreePump(pumpEndTimes, currentTime);
                double refuelTime = freePump == 0 ? refuelTime = ExponentialFuel() : refuelTime = ExponentialFuelSec();

                pumpEndTimes[freePumpNow] = currentTime + refuelTime;
                carsServed++;

                waitingTimes.Add(waitingTime);
            }
        }
    }

    // Метод для поиска свободной колонки
    private int FindFreePump(List<double> pumpEndTimes, double arrivalTime)
    {
        for (int i = 0; i < pumpEndTimes.Count; i++)
            if (pumpEndTimes[i] <= arrivalTime)
                return i;

        return -1; // Все колонки заняты
    }

    private double Exponential()
    {
        return (-1 * ((double)1 / (double)10) * Math.Log(random.NextDouble()));
    }

    private double ExponentialFuel()
    {
        return (-1 * ((double)1 / (double)5) * Math.Log(random.NextDouble()));
    }

    private double ExponentialFuelSec()
    {
        return (-1 * ((double)1 / (double)4) * Math.Log(random.NextDouble()));
    }
}