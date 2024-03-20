using Plotly.NET;
using Plotly.NET.TraceObjects;

namespace Program
{
    public static class Miscellaneous
    {
        public static Dictionary<double, int> ReadFromFile(string path)
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
                    result.Add(Math.Round(start + (i + 1) * step, 3), int.Parse(strData[i]));
                }
            }
            return result;
        }

        public static void BuildHistogram(Dictionary<double, int> data)
        {
            var h = Math.Round(data.Keys.ToArray()[^1] - data.Keys.ToArray()[^2], 3);
            double[] x = new double[data.Values.ToArray().Sum()];
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
    }
}