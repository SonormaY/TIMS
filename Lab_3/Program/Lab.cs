using Plotly.NET;
using MathNet.Numerics.LinearAlgebra;

namespace Lab_3
{
    static public class Lab
    {
        public static int[] CalculateSumsLINQ(int n1, int n2, int[,] corTable)
        {
            return Enumerable.Range(0, n1)
                .Select(i => Enumerable.Range(0, n2)
                    .Sum(j => corTable[j, i]))
                .ToArray();
        }
        public static double[] CalculateConditionalAveragesLINQ(int[] y, int n2, int[] x_n, int[,] corTable)
        {
            return Enumerable.Range(0, n2)
                .Select(i => Enumerable.Range(0, y.Length)
                    .Sum(j => (double)y[j] * corTable[j, i]) / x_n[i])
                .ToArray();
        }
        public static (double a, double b) CalculateABLINQ(int[] x, int[] x_n, double[] yxk)
        {
            var A = Matrix<double>.Build.DenseOfRows(
                [Enumerable.Range(0, x.Length).Aggregate(new double[2], (acc, i) => {
                    acc[0] += x[i] * x_n[i];
                    acc[1] += x_n[i];
                    return acc;
                }),
                Enumerable.Range(0, x.Length).Aggregate(new double[2], (acc, i) => {
                    acc[0] += x[i] * x[i] * x_n[i];
                    acc[1] += x[i] * x_n[i];
                    return acc;
                })]
            );
            var B = Vector<double>.Build.Dense(
                Enumerable.Range(0, x.Length)
                .Aggregate(new double[2], (acc, i) => {
                    acc[0] += x_n[i] * Math.Log10(yxk[i]);
                    acc[1] += x_n[i] * x[i] * Math.Log10(yxk[i]);
                    return acc;
                })
            );

            var X = A.Solve(B);
            return (Math.Pow(10, X[0]), Math.Pow(10, X[1]));
        }
        public static GenericChart.GenericChart GetPoints(int[] x, double[] yxk)
        {
            return Chart2D.Chart.Point<int, double, double>(x, yxk, Name: "M(xᵢ, ȳₓ)");
        }
        public static GenericChart.GenericChart GetLine(int[] x, double[] yxk, double a, double b)
        {
            double[] xRegres = Enumerable.Range(0, 10000)
                .Select(i => -1 + i * 0.0012)
                .ToArray();
            double[] yRegres = xRegres.Select(x => b * Math.Pow(a, x)).ToArray();
            return Chart2D.Chart.Line<double, double, double>(x: xRegres, y: yRegres, Name: $"ȳₓ = {Math.Round(b, 3)} * {Math.Round(a, 3)}ˣ\n");
        }
        public static void ShowCharts(int[] x, double[] yxk, int[] x_n, double a, double b)
        {
            var points = GetPoints(x, yxk);
            var line = GetLine(x, yxk, a, b);
            line.Show();
            points.Show();
            var combine = Chart.Combine([points, line]);
            combine.Show();
        }
        public static double CalculateDLINQ(int[] x, int[] y, int[,] CorTable, double a, double b, int N){
            return Enumerable.Range(0, y.Length)
                .Select(i => Enumerable.Range(0, x.Length)
                    .Sum(j => CorTable[i, j] * Math.Pow(y[i] - b * Math.Pow(a, x[j]), 2))
                )
                .Sum() / N;
        }
        public static double CalculateDeltaLINQ(int[] x, int[] x_n, double[] yxk, double a, double b)
        {
            return Enumerable.Range(0, x.Length)
                .Sum(i => Math.Pow(yxk[i] - b * Math.Pow(a, x[i]), 2) * x_n[i]);
        }
    }
}
