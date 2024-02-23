class Program
{
    static Mutex mutex = new Mutex();
    static Random random = new Random();
    static List<int> generatedNumbers = new List<int>();
    static List<int> primes = new List<int>();
    static List<int> filteredNumbers = new List<int>();

    static void Main(string[] args)
    {
        Thread producerThread = new Thread(GenerateAndWriteNumbers);
        Thread consumerThread1 = new Thread(ReadAndFilterPrimes);
        Thread consumerThread2 = new Thread(FilterNumbersEndingWithSeven);

        producerThread.Start();
        consumerThread1.Start();
        consumerThread2.Start();

        producerThread.Join();
        consumerThread1.Join();
        consumerThread2.Join();

        Console.WriteLine("\nAll threads finished execution.");
    }

    static void GenerateAndWriteNumbers()
    {
        mutex.WaitOne();
        try
        {
            using (StreamWriter writer = new StreamWriter("numbers.txt"))
            {
                for (int i = 0; i < 100; i++)
                {
                    int num = random.Next(1, 1000);
                    generatedNumbers.Add(num);
                    writer.WriteLine(num);
                }
            }
        }
        finally
        {
            mutex.ReleaseMutex();
        }

        Console.WriteLine("Generated numbers:\n" + string.Join(", ", generatedNumbers));
        Console.WriteLine("\nNumbers wrote numbers to file.\n");
    }

    static void ReadAndFilterPrimes()
    {
        mutex.WaitOne();
        try
        {            
            using (StreamReader reader = new StreamReader("numbers.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int num = int.Parse(line);
                    if (IsPrime(num))
                        primes.Add(num);
                }
            }

            using (StreamWriter writer = new StreamWriter("primes.txt"))
            {
                foreach (int prime in primes)
                {
                    writer.WriteLine(prime);
                }
            }   

        }
        finally
        {
            mutex.ReleaseMutex();
        }

        Console.WriteLine("\nFiltered primes:\n" + string.Join(", ", primes));
        Console.WriteLine("\nPrimes wrote to file.\n");
    }

    static void FilterNumbersEndingWithSeven()
    {
        mutex.WaitOne();
        try
        {            
            using (StreamReader reader = new StreamReader("primes.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int num = int.Parse(line);
                    if (num % 10 == 7)
                        filteredNumbers.Add(num);
                }
            }

            using (StreamWriter writer = new StreamWriter("ending_with_seven.txt"))
            {
                foreach (int number in filteredNumbers)
                {
                    writer.WriteLine(number);
                }
            }
            
        }
        finally
        {
            mutex.ReleaseMutex();
        }

        Console.WriteLine("\nFiltered numbers:\n" + string.Join(", ", filteredNumbers));
        Console.WriteLine("\nNumbers ending with seven and wrote to file.");
    }

    static bool IsPrime(int num)
    {
        if (num <= 1)
            return false;
        if (num == 2)
            return true;
        if (num % 2 == 0)
            return false;
        for (int i = 3; i <= Math.Sqrt(num); i += 2)
        {
            if (num % i == 0)
                return false;
        }
        return true;
    }
}