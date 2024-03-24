namespace Program
{
    public static class Task1
    {
        public static void GetMiscData(Dictionary<double, int> data, out double h, out double a, out double sigma, out ConsoleKey key)
        {
            var tempH = Miscellaneous.GetH(data);
            h = tempH;
            a = 0;
            sigma = 0;
            Console.Clear();
            Console.Write("Choose to enter parameters (m)anually or (a)utomatically: ");
            key = Console.ReadKey().Key;
            Console.WriteLine();
            if (key == ConsoleKey.M)
            {
                Console.Write("Enter a: ");
                double inputA = double.Parse(Console.ReadLine() ?? "0");
                a = inputA;
                Console.Write("Enter sigma: ");
                sigma = double.Parse(Console.ReadLine() ?? "0");
            } 
            else if (key == ConsoleKey.A)
            {
                double tempA = data.Select(x => (x.Key - tempH / 2) * x.Value).Sum() / data.Values.Sum();
                a = tempA;
                sigma = Math.Sqrt(data.Select(x => Math.Pow(x.Key - tempH / 2 - tempA, 2) * x.Value).Sum() / data.Values.Sum());
            }
            else
            {
                Console.WriteLine("Invalid key");
                GetMiscData(data, out h, out a, out sigma, out key);
            }
        }
        public static double Erf(double x)
        {
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;
            int sign = 1;
            if (x < 0)
            {
                sign = -1;
            }
            x = Math.Abs(x);
            double t = 1 / (1 + p * x);
            double y = 1 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
            return sign * y;
        }
        public static double F(double x, double a, double sigma)
        {
            double xC = (x - a) / sigma;
            if (Math.Abs(xC) >= 5)
            {
                return x / 10;
            }
            return Math.Sqrt(Math.PI) * Erf(xC / Math.Sqrt(2)) / Math.Sqrt(2) / Math.Sqrt(2 * Math.PI);
        }
        public static double[] CalculatePi(Dictionary<double, int> data, double h, double a, double sigma)
        {
            double[] pi = new double[data.Values.Count];
            pi[0] = F(data.Keys.ToArray()[0], a, sigma) + 0.5;
            for (int i = 1; i < data.Values.Count - 1; i++)
            {
                pi[i] = F(data.Keys.ToArray()[i], a, sigma) - F(data.Keys.ToArray()[i] - h, a, sigma);
            }
            pi[^1] = 1 - pi.Sum();
            return pi;
        }
        public static int GetR(double[] npi, bool manual)
        {
            return npi.Length - (manual ? 1 : 3);
        }
        public static void ExecuteTask1()
        {
            Console.Title = "Task 1";

            // Read data from file
            var data = Miscellaneous.InputHandler();

            //Get parameters
            GetMiscData(data, out double h, out double a, out double sigma, out ConsoleKey key);

            Console.Clear();    
            // Print data
            Console.WriteLine("H0: data is distributed normally");
            Console.WriteLine("\nData:");
            Miscellaneous.PrintData(data);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n| a = {Math.Round(a, 3)} | sigma = {Math.Round(sigma, 3)} |\n");
            Console.ResetColor();

            // Build histogram
            Miscellaneous.BuildHistogram(data);

            // Calculate pi and npi
            Console.WriteLine("Pi:");
            var pi = CalculatePi(data, h, a, sigma);
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
            
            // Calculate r, Xemp and XKr
            Console.WriteLine();
            double XKr = Miscellaneous.GetXKr(GetR(npi, key == ConsoleKey.M));
            double Xemp = Miscellaneous.GetXemp(data, npi);

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