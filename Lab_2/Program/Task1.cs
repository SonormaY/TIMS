using Microsoft.FSharp.Core;

namespace Program
{
    public static class Task1
    {
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
        public static void ExecuteTask1()
        {
            Dictionary<int, double> XKr_table = new Dictionary<int, double>
            {
                { 1, 3.8415 },
                { 2, 5.9915 },
                { 3, 7.8147 },
                { 4, 9.4877 },
                { 5, 11.0705 },
                { 6, 12.5916 },
                { 7, 14.0671 },
                { 8, 15.5073 },
                { 9, 16.919 },
                { 10, 18.307 },
                { 11, 19.6751 },
                { 12, 21.0261 },
                { 13, 22.362 },
                { 14, 23.6848 },
                { 15, 24.9958 },
                { 16, 26.2962 },
                { 17, 27.5871 },
                { 18, 28.8693 },
                { 19, 30.1435 },
                { 20, 31.4104 }
            };
            // Console.Write("Enter path to file: ");
            // string path = Console.ReadLine();
            var data = Miscellaneous.ReadFromFile("test.txt");
            Console.WriteLine(string.Join(" | ", data.Keys.ToArray()));
            Console.WriteLine(string.Join(" | ", data.Values.ToArray()));

            double a = 0;
            double sigma = 0;
            var h = Math.Round(data.Keys.ToArray()[^1] - data.Keys.ToArray()[^2], 3);

            Console.WriteLine("Choose to enter parameters (m)anually or (a)utomatically");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.M)
            {
                Console.Write("Enter a: ");
                a = double.Parse(Console.ReadLine());
                Console.Write("Enter sigma: ");
                sigma = double.Parse(Console.ReadLine());
            } else if (key == ConsoleKey.A)
            {
                a = data.Select(x => (x.Key - h / 2) * x.Value).Sum() / data.Values.Sum();
                sigma = Math.Sqrt(data.Select(x => Math.Pow(x.Key - h / 2 - a, 2) * x.Value).Sum() / data.Values.Sum());
            }
            else
            {
                Console.WriteLine("Invalid key");
            }
            Console.WriteLine($"a = {Math.Round(a, 3)}, sigma = {Math.Round(sigma, 3)}");
            double[] pi = new double[data.Values.Count];
            pi[0] = F(data.Keys.ToArray()[0], a, sigma) + 0.5;
            for (int i = 1; i < data.Values.Count - 1; i++)
            {
                pi[i] = F(data.Keys.ToArray()[i], a, sigma) - F(data.Keys.ToArray()[i] - h, a, sigma);
            }
            pi[^1] = 1 - pi.Sum();
            for (int i = 0; i < pi.Length; i++)
            {
                Console.Write($"{Math.Round(pi[i], 3)} ");
            }
            
            double[] npi = new double[data.Values.Count];
            for (int i = 0; i < npi.Length; i++)
            {
                npi[i] = pi[i] * data.Values.Sum();
            }
            Console.WriteLine(string.Join(" | ", npi));

            int skippedFirst = 0;
            for (int i = 0; i < npi.Length; i++)
            {
                if (npi[i] < 10 || data.Values.ElementAt(i) < 5)
                {
                    npi[i + 1] += npi[i];
                    npi[i] = 0;
                    skippedFirst++;
                    continue;
                }
                break;
            }

            int skippedLast = 0;
            for (int i = npi.Length - 1; i >= 0; i--)
            {
                if (npi[i] < 10 || data.Values.ElementAt(i) < 5)
                {
                    npi[i - 1] += npi[i];
                    npi[i] = 0;
                    skippedLast++;
                    continue;
                }
                break;
            }
            double[] npi2 = new double[npi.Length - skippedFirst - skippedLast];
            for (int i = 0; i < npi2.Length - skippedLast; i++)
            {
                npi2[i] = npi[i + skippedFirst];
            }
            npi2[^1] = npi.Skip(npi2.Length).Sum();
            Console.WriteLine("--------------------");
            Console.WriteLine(string.Join(" | ", npi2));
            Console.WriteLine("--------------------");
            Dictionary<double, double> data2 = new Dictionary<double, double>();
            data2.Add(data.Keys.ElementAt(skippedFirst), data.Values.Take(skippedFirst + 1).Sum());
            for (int i = skippedFirst + 1; i < npi2.Length - skippedLast + 1; i++)
            {
                data2.Add(data.Keys.ElementAt(i), data.Values.ElementAt(i));
            }
            data2.Add(data.Keys.ElementAt(npi2.Length + 1), data.Values.Skip(npi2.Length - skippedLast + 1).Sum());
            Console.WriteLine(string.Join(" | ", data2.Keys.ToArray()));
            Console.WriteLine(string.Join(" | ", data2.Values.ToArray()));
            int r = npi2.Length - 3;
            if (key == ConsoleKey.M)
            {
                r = npi.Length - 1;
            }
            double XKr = 0;
            double Xemp = 0;
            for (int i = 0; i < npi2.Length; i++)
            {
                Xemp += Math.Pow(data2.Values.ElementAt(i) - npi2[i], 2) / npi2[i];
            }
            XKr = XKr_table[r];
            Console.WriteLine($"XKr = {XKr}, Xemp = {Xemp}");
            if (Xemp < XKr)
            {
                Console.WriteLine("H0 is not rejected");
            }
            else
            {
                Console.WriteLine("H0 is rejected");
            }
            //Miscellaneous.BuildHistogram(data);
            Console.ReadKey();
        }
    }
}