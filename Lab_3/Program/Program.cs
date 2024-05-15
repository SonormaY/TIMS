using Plotly.NET;
using SuperDuperMenuLib;
namespace Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            int [] x = {0, 4, 6, 7, 8, 9, 10};
            int [] y = {5, 20, 40, 62, 78, 95};
            int [,] CorTable = {
                {25, 0, 2, 0, 0, 0, 0},
                {10, 60, 0, 0, 0, 0, 0},
                {0, 2, 22, 2, 0, 0, 0},
                {0, 0, 0, 1, 2, 0, 0},
                {0, 0, 0, 0, 0, 28, 0,},
                {0, 0, 0, 0, 0, 0, 21}
            };
            var menu = new SuperDuperMenu();
            var entries = new Dictionary<string, Action> {
                {"Run", () => {
                    var x_n = Lab.CalculateSumsLINQ(x.Length, y.Length, CorTable);
                    string x_n_str = string.Join(" | ", x_n);
                    Console.WriteLine(new string('-', x_n_str.Length + 9) + $"\nx_n: | {x_n_str} |");

                    var yxk = Lab.CalculateConditionalAveragesLINQ(y, x.Length, x_n, CorTable);
                    string y_xk_str = string.Join(" | ", yxk.Select(n => Math.Round((decimal)n, 3)));
                    Console.WriteLine(new string('-', y_xk_str.Length + 10) + $"\ny_xk: | {y_xk_str} |" + "\n" + new string('-', y_xk_str.Length + 10));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Assumption: Exponential function\n");

                    var (a, b) = Lab.CalculateABLINQ(x, x_n, yxk);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"a: {Math.Round(a, 3)}, b: {Math.Round(b, 3)}\n");
                    Chart.Combine([
                        Lab.GetPoints(x, yxk),
                        Lab.GetLine(x, yxk, a, b)
                    ]).Show();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"D: {Math.Round(Lab.CalculateDLINQ(x, y, CorTable, a, b, x_n.Sum()), 3)}");
                    Console.WriteLine($"Delta: {Math.Round(Lab.CalculateDeltaLINQ(x, x_n, yxk, a, b), 3)}");
                }}
            };

            menu.LoadEntries(entries);
            menu.Run();
        }
    }
}
