class Program
{
    static SemaphoreSlim semaphore = new SemaphoreSlim(3);

    static void Main(string[] args)
    {
        List<Thread> threads = new List<Thread>();

        for (int i = 0; i < 10; i++)
        {
            Thread thread = new Thread(ThreadFunction);
            threads.Add(thread);
        }

        foreach (Thread thread in threads)
        {
            thread.Start();            
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("All threads have finished.");
    }

    static void ThreadFunction()
    {
        Random random = new Random();
        int threadId = Thread.CurrentThread.ManagedThreadId;

        Console.WriteLine($"Thread {threadId} started.");

        try
        {
            semaphore.Wait();

            Console.WriteLine($"Thread {threadId} entered the critical section.");

            for (int i = 0; i < 3; i++)
            {
                int randomNumber = random.Next(1, 100);
                Console.WriteLine($"Thread {threadId}: {randomNumber}");
                Thread.Sleep(1000);
            }

            Console.WriteLine($"Thread {threadId} finished its work.");
        }
        finally
        {
            semaphore.Release();
        }
    }
}
