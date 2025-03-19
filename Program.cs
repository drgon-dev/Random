using System;
using System.Collections.Generic;
using System.Linq;

class GasStationSimulation
{
    private static Random random = new Random();

    // Параметры симуляции
    private const int simulationTime = 1000000; // Общее время симуляции
    private const int QueueCapacity = 3; // Количество мест в очереди
    private static int carsAmount = 0; // Количество всех машин
    private static int carsServed = 0; // Количество обслуженных машин
    private static int carsMissed = 0; // Количество машин, которые проехали мимо

    private static double totalWaitingTime = 0; // Общее время ожидания в очереди

    static void Main(string[] args)
    {
        // Очередь для хранения времени окончания заправки на каждой колонке
        List<double> pumpEndTimes = new List<double> { 0, 0 };
        Queue<double> waitingQueue = new Queue<double>(); // Очередь машин

        for (double currentTime = 0; currentTime < simulationTime;)
        {
            carsAmount++;
            // Генерация времени до следующего приезда машины
            double arrivalTime = currentTime + Exponential();

            // Обновление времени симуляции
            currentTime = arrivalTime;

            // Проверка, свободна ли хотя бы одна колонка
            int freePump = FindFreePump(pumpEndTimes, arrivalTime);

            if (freePump != -1)
            {
                // Если колонка свободна, начинаем заправку
                double refuelTime = ExponentialSec();
                pumpEndTimes[freePump] = arrivalTime + refuelTime;
                carsServed++;
            }
            else if (waitingQueue.Count < QueueCapacity)
            {
                // Если все колонки заняты, но есть место в очереди, добавляем машину в очередь
                waitingQueue.Enqueue(arrivalTime);
            }
            else
            {
                // Если все колонки заняты и очередь заполнена, машина проезжает мимо
                carsMissed++;
            }

            // Проверка, может ли машина из очереди начать заправку
            while (waitingQueue.Count > 0 && FindFreePump(pumpEndTimes, currentTime) != -1)
            {
                double waitingCarArrivalTime = waitingQueue.Dequeue();
                double waitingTime = currentTime - waitingCarArrivalTime;
                totalWaitingTime += waitingTime;

                int freePumpNow = FindFreePump(pumpEndTimes, currentTime);
                double refuelTime = ExponentialSec();
                pumpEndTimes[freePumpNow] = currentTime + refuelTime;
                carsServed++;
            }
        }

        // Вывод результатов
        Console.WriteLine($"Обслужено машин: {carsServed}");
        Console.WriteLine($"Машин проехало мимо: {carsMissed}");
        Console.WriteLine($"Пропускная способность: {(double)carsServed / (double)simulationTime:F2} авто/час");
        Console.WriteLine($"Среднее время ожидания: {(double)totalWaitingTime / (double)carsServed:F2} часов");
        Console.WriteLine($"Вероятность обслуживания: {(double)carsServed / (double)carsAmount:F2}");
        Console.WriteLine($"Вероятность отказа: {(double)carsMissed / (double)carsAmount:F2}");
        Console.WriteLine($"Вероятность занятости: {(double)totalWaitingTime / (double)simulationTime:F2}");
        //Console.WriteLine($"{:F2}");
    }

    // Метод для поиска свободной колонки
    private static int FindFreePump(List<double> pumpEndTimes, double arrivalTime)
    {
        for (int i = 0; i < pumpEndTimes.Count; i++)
        {
            if (pumpEndTimes[i] <= arrivalTime)
            {
                return i;
            }
        }
        return -1; // Все колонки заняты
    }

    // Метод для генерации случайного времени по экспоненциальному распределению
    private static double Exponential()
    {
        return (-1 * ((double)1 / (double)10) * Math.Log(random.NextDouble()));
    }
    private static double ExponentialSec()
    {
        return (-1 * ((double)1 / (double)5) * Math.Log(random.NextDouble()));
    }
}