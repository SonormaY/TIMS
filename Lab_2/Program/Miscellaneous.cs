using Plotly.NET;
using Plotly.NET.TraceObjects;

namespace Program
{
    public static class Miscellaneous
    {
        public static Dictionary<double, int> ReadFromFile(string path, bool intervaled = false)
        {
            Dictionary<double, int> result = new Dictionary<double, int>();
            using (StreamReader sr = new StreamReader(path))
            {
                string[] parameters = sr.ReadLine().Split(',');
                var start = double.Parse(parameters[0]);
                var step = double.Parse(parameters[1]);
                var n = int.Parse(parameters[2]);
                string[] strData = sr.ReadLine().Split(',');
                if (strData.Length != n)
                {
                    throw new ArgumentException("n != quantity of data in file");
                }

                for (int i = 0; i < n; i++)
                {
                    result.Add(Math.Round(start + (intervaled ? i + 1 : i) * step, 3), int.Parse(strData[i]));
                }
            }
            return result;
        }
        public static void BuildHistogram(Dictionary<double, int> data)
        {
            var h = Math.Round(data.Keys.ToArray()[^1] - data.Keys.ToArray()[^2], 3);
            double[] x = new double[data.Values.Sum()];
            for (int i = 0, j = 0; i < data.Values.Count; i++)
            {
                for (int k = 0; k < data.Values.ToArray()[i]; k++)
                {
                    x[j++] = data.Keys.ToArray()[i] - h / 2;
                }
            }
            Chart2D.Chart.Histogram<double, int>(
                data: x,
                XBins: Bins.init(
                    Start: data.Keys.ToArray()[0] - h,
                    End: data.Keys.ToArray()[^1],
                    Size: h
                ),
                orientation: StyleParam.Orientation.Vertical
            ).Show();
        }
        public static void BuildDiagram(Dictionary<double, int> data)
        {
            Chart2D.Chart.Column<int, double, int, int, int>(
                values: data.Values.ToArray(),
                Keys: data.Keys.ToArray()
            ).Show();
        }
        public static Dictionary<double, int> InputHandler(string defaultPath = "default.txt")
        {
            Console.Clear();
            Console.Write($"Enter path to file (press Enter to read from {defaultPath}): ");
            string path = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(path))
            {
                path = defaultPath;
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
            int maximumDigits = Math.Max(data.Values.Max().ToString().Length, data.Keys.Max().ToString().Length + 2);
            int dashesToPrint = (maximumDigits + 3) * data.Count + 1;
            Console.WriteLine(new string('-', dashesToPrint));
            Console.WriteLine("| " + string.Join(" | ", data.Keys.Select(k => k.ToString().PadRight(maximumDigits))) + " |");
            Console.WriteLine(new string('-', dashesToPrint));
            Console.WriteLine("| " + string.Join(" | ", data.Values.Select(v => v.ToString().PadRight(maximumDigits))) + " |");
            Console.WriteLine(new string('-', dashesToPrint));
            Console.ForegroundColor = temp;
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
        public static double GetXemp(Dictionary<double, int> data, double[] npi)
        {
            return data.Select((x, index) => Math.Pow(x.Value - npi[index], 2) / npi[index]).Sum();
        }
        public static double GetXKr(double r)
        {
            Console.Write("Enter alpha: ");
            double alpha = double.Parse(Console.ReadLine() ?? "");
            if (alpha <= 0 || alpha >= 1)
            {
                Console.WriteLine("Invalid alpha");
                GetXKr(r);
            }
            return MathNet.Numerics.Distributions.ChiSquared.InvCDF(r, 1 - alpha);
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
        public static double GetH(Dictionary<double, int> data)
        {
            return Math.Round(data.Keys.ToArray()[^1] - data.Keys.ToArray()[^2], 3);
        }
    }
}