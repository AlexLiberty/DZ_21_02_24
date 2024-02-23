class Program
{
    static readonly object lockObj = new object();
    static readonly ManualResetEvent generationCompleted = new ManualResetEvent(false);

    static void Main(string[] args)
    {
        List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
        Random random = new Random();

        Thread generatorThread = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                int num1 = random.Next(1, 11);
                int num2 = random.Next(1, 11);
                lock (lockObj)
                {
                    pairs.Add(new Tuple<int, int>(num1, num2));
                    Console.WriteLine($"Generated pair: ({num1}, {num2})");
                }
                Thread.Sleep(500);
            }
            generationCompleted.Set();
        });
        generatorThread.Start();

        Thread sumThread = new Thread(() =>
        {
            generationCompleted.WaitOne(); 
            List<int> sums = new List<int>();
            foreach (var pair in pairs)
            {
                int sum = pair.Item1 + pair.Item2;
                sums.Add(sum);
                Console.WriteLine($"Sum of pair ({pair.Item1}, {pair.Item2}): {sum}");
            }
            File.WriteAllLines("sums.txt", sums.ConvertAll(s => s.ToString()));
        });
        sumThread.Start();

        Thread productThread = new Thread(() =>
        {
            generationCompleted.WaitOne();
            List<int> products = new List<int>();
            foreach (var pair in pairs)
            {
                int product = pair.Item1 * pair.Item2;
                products.Add(product);
                Console.WriteLine($"Product of pair ({pair.Item1}, {pair.Item2}): {product}");
            }
            File.WriteAllLines("products.txt", products.ConvertAll(p => p.ToString()));
        });
        productThread.Start();

        generatorThread.Join();
        sumThread.Join();
        productThread.Join();

        Console.WriteLine("All threads completed.");
    }
}