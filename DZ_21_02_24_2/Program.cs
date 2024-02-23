using System.Diagnostics;

class Program
{
    private static int counter = 0;
    private static object lockObject = new object();
    private static Mutex mutex = new Mutex();

    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Monitor
        stopwatch.Start();
        Thread[] monitorThreads = new Thread[5];
        for (int i = 0; i < monitorThreads.Length; i++)
        {
            monitorThreads[i] = new Thread(MonitorIncrement);
            monitorThreads[i].Start();
        }
        foreach (var thread in monitorThreads)
        {
            thread.Join();
        }
        stopwatch.Stop();
        Console.WriteLine("Using Monitor: " + "time: " + stopwatch.Elapsed + " counter:" + counter);

        // Mutex
        counter = 0;
        stopwatch.Reset();
        stopwatch.Start();
        Thread[] mutexThreads = new Thread[5];
        for (int i = 0; i < mutexThreads.Length; i++)
        {
            mutexThreads[i] = new Thread(MutexIncrement);
            mutexThreads[i].Start();
        }
        foreach (var thread in mutexThreads)
        {
            thread.Join();
        }
        stopwatch.Stop();
        Console.WriteLine("Using Mutex: " + "time: " + stopwatch.Elapsed + " counter:" + counter);

        // Lock
        counter = 0;
        stopwatch.Reset();
        stopwatch.Start();
        Thread[] lockThreads = new Thread[5];
        for (int i = 0; i < lockThreads.Length; i++)
        {
            lockThreads[i] = new Thread(LockIncrement);
            lockThreads[i].Start();
        }
        foreach (var thread in lockThreads)
        {
            thread.Join();
        }
        stopwatch.Stop();
        Console.WriteLine("Using Lock: " + "time: " + stopwatch.Elapsed + " counter:" + counter);
    }

    static void MonitorIncrement()
    {
        for (int i = 0; i < 1000000; i++)
        {
            lock (lockObject)
            {
                counter++;
            }
        }
    }

    static void MutexIncrement()
    {
        for (int i = 0; i < 1000000; i++)
        {
            mutex.WaitOne();
            counter++;
            mutex.ReleaseMutex();
        }
    }

    static void LockIncrement()
    {
        for (int i = 0; i < 1000000; i++)
        {
            lock (lockObject)
            {
                counter++;
            }
        }
    }
}
