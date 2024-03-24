namespace Program
{
    public static class Task2
    {
        public static void GetMiscData(Dictionary<double, int> data, out double h, out double lambda, out ConsoleKey key)
        {
            h = Miscellaneous.GetH(data);
            Console.Clear();
            Console.Write("Choose to enter parameters (m)anually or (a)utomatically: ");
            key = Console.ReadKey().Key;
            Console.WriteLine();
            if (key == ConsoleKey.M)
            {
                Console.Write("Enter lambda: ");
                lambda = double.Parse(Console.ReadLine());
            }
            else if (key == ConsoleKey.A)
            {
                lambda = data.Select(x => x.Key * x.Value).Sum() / data.Values.Sum();
            }
            else
            {
                Console.WriteLine("Invalid key");
                GetMiscData(data, out h, out lambda, out key);
            }
        }
        public static double[] CalculatePi(Dictionary<double, int> data, double lambda)
        {
            double[] pi = new double[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                pi[i] = MathNet.Numerics.Distributions.Poisson.PMF(lambda, i);
            }
            return pi;
        }
        public static int GetR(double[] npi, bool manual)
        {
            return npi.Length - (manual ? 1 : 2);
        }
        public static void ExecuteTask2()
        {
            Console.Title = "Task 2";

            // Read data from file
            var data = Miscellaneous.InputHandler("default2.txt");
            
            // Get parameters
            GetMiscData(data, out double h, out double lambda, out ConsoleKey key);

            Console.Clear();
            // Print data
            Console.WriteLine("H0: data is distributed by Poisson distribution");
            Console.WriteLine("\nData:");
            Miscellaneous.PrintData(data);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n| lambda = {lambda} |\n");
            Console.ResetColor();

            //Build histogram
            Miscellaneous.BuildDiagram(data);

            // Calculate pi and npi
            Console.WriteLine("Pi:");
            var pi = CalculatePi(data, lambda);
            Miscellaneous.PrintArray(pi, true);
            Console.WriteLine("\nNpi:");
            double[] npi = pi.Select(x => x * data.Values.Sum()).ToArray();
            Miscellaneous.PrintArray(npi, true);

            // Merge data
            Miscellaneous.MergeData(data, ref npi);
            Console.WriteLine("\nMerged data:");
            Miscellaneous.PrintData(data);
            Console.WriteLine("\nMerged Npi:");
            Miscellaneous.PrintArray(npi, true);

            // Calculate Xemp and XKr
            Console.WriteLine();
            double Xemp = Miscellaneous.GetXemp(data, npi);
            double XKr = Miscellaneous.GetXKr(GetR(npi, key == ConsoleKey.M));

            // Print results
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nXKr = {XKr}, Xemp = {Xemp}");
            Console.ResetColor();
            Miscellaneous.IsH0Rejected(Xemp, XKr);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}