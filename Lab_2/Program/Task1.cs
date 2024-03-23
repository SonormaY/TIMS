using Microsoft.FSharp.Core;

namespace Program
{
    public static class Task1
    {
        public static Dictionary<double, int> InputHandler()
        {
            Console.Clear();
            Console.Write("Enter path to file (press Enter to read from default.txt): ");
            string path = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(path))
            {
                path = "default.txt";
            }
            return Miscellaneous.ReadFromFile(path);
        }
        public static void PrintArray(double[] arr, bool round = false)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (round)
            {
                int maximumDigits = arr.Select(x => Math.Round(x, 3).ToString().Length).Max();
                int dashesToPrint = (maximumDigits + 3) * arr.Length + 1;
                Console.WriteLine(new string('-', dashesToPrint));
                Console.WriteLine("| " + string.Join(" | ", arr.Select(k => Math.Round(k ,3).ToString().PadRight(maximumDigits))) + " |");
                Console.WriteLine(new string('-', dashesToPrint));
            }
            else
            {
                int maximumDigits = arr.Select(x => x.ToString().Length).Max();
                int dashesToPrint = (maximumDigits + 3) * arr.Length + 1;
                Console.WriteLine(new string('-', dashesToPrint));
                Console.WriteLine("| " + string.Join(" | ", arr.Select(k => k.ToString().PadRight(maximumDigits))) + " |");
                Console.WriteLine(new string('-', dashesToPrint));
            }
            Console.ForegroundColor = temp;
        }
        public static void PrintData(Dictionary<double, int> data)
        {
            ConsoleColor temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            int maximumDigits = Math.Max(data.Values.Max().ToString().Length, data.Keys.Max().ToString().Length);
            int dashesToPrint = (maximumDigits + 3) * data.Count + 1;
            Console.WriteLine(new string('-', dashesToPrint));
            Console.WriteLine("| " + string.Join(" | ", data.Keys.Select(k => k.ToString().PadRight(maximumDigits))) + " |");
            Console.WriteLine(new string('-', dashesToPrint));
            Console.WriteLine("| " + string.Join(" | ", data.Values.Select(v => v.ToString().PadRight(maximumDigits))) + " |");
            Console.WriteLine(new string('-', dashesToPrint));
            Console.ForegroundColor = temp;
        }
        public static void GetMiscData(Dictionary<double, int> data, out double h, out double a, out double sigma, out ConsoleKey key)
        {
            var tempH = GetH(data);
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
            }
        }
        public static double GetXkr(int value)
        {
            double[] Xkr = {3.8415, 5.9915, 7.8147, 9.4877, 11.0705, 12.5916, 14.0671, 15.5073, 16.919, 18.307, 19.6751, 21.0261, 22.362, 23.6848, 24.9958, 26.2962, 27.5871, 28.8693, 30.1435, 31.4104};
            return Xkr[value - 1];
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
        public static double GetH(Dictionary<double, int> data)
        {
            return Math.Round(data.Keys.ToArray()[^1] - data.Keys.ToArray()[^2], 3);
        }
        public static void MergeData(Dictionary<double, int> data, ref double[] npi)
        {
            bool firstHalfDone = false;
            for (int i = 0; i < npi.Length; i++)
            {
                if (npi[i] < 10 || data.Values.ElementAt(i) < 5)
                {
                    int indexDecider = firstHalfDone ? i - 1 : i;
                    data[data.Keys.ElementAt(indexDecider + 1)] += data[data.Keys.ElementAt(indexDecider)];
                    data.Remove(data.Keys.ElementAt(indexDecider));
                    npi[indexDecider + 1] += npi[indexDecider];
                    npi = npi.Where((x, index) => index != indexDecider).ToArray();
                    i--;
                }
                else
                {
                    firstHalfDone = true;
                }
            }
        }
        public static int GetR(double[] npi, bool manual)
        {
            return npi.Length - (manual ? 1 : 3);
        }
        public static double GetXemp(Dictionary<double, int> data, double[] npi)
        {
            return data.Select((x, index) => Math.Pow(x.Value - npi[index], 2) / npi[index]).Sum();
        }
        public static void IsH0Rejected(double Xemp, double XKr)
        {
            ConsoleColor temp = Console.ForegroundColor;
            if (Xemp < XKr)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("H0 is accepted");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("H0 is rejected");
            }
            Console.ForegroundColor = temp;
        }
        public static void ExecuteTask1()
        {
            Console.Title = "Task 1";

            // Read data from file
            var data = InputHandler();

            //Get parameters
            GetMiscData(data, out double h, out double a, out double sigma, out ConsoleKey key);

            Console.Clear();    
            // Print data
            Console.WriteLine("Data:");
            PrintData(data);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n| a = {Math.Round(a, 3)} | sigma = {Math.Round(sigma, 3)} |\n");
            Console.ResetColor();

            // Build histogram
            Miscellaneous.BuildHistogram(data);

            // Calculate pi and npi
            Console.WriteLine("Pi:");
            var pi = CalculatePi(data, h, a, sigma);
            PrintArray(pi, true);
            Console.WriteLine("\nNpi:");
            double[] npi = pi.Select(x => x * data.Values.Sum()).ToArray();
            PrintArray(npi, true);

            // Merge data
            MergeData(data, ref npi);
            Console.WriteLine("\nMerged data:");
            PrintData(data);
            Console.WriteLine("\nMerged Npi:");
            PrintArray(npi, true);
            
            // Calculate r, Xemp and compare with XKr
            int r = GetR(npi, key == ConsoleKey.M);
            double XKr = GetXkr(r);
            double Xemp = GetXemp(data, npi);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nXKr = {XKr}, Xemp = {Xemp}");
            Console.ResetColor();
            IsH0Rejected(Xemp, XKr);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}