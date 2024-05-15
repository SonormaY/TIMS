using Plotly.NET;
using MathNet.Numerics.LinearAlgebra;
using SuperDuperMenuLib;

namespace Lab
{
   class Program
   {
      static void Bebra(string[] args)
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
        
        // x = {0, 1, 2, 3, 4, 5, 6};
        // y = 2, 3, 5, 10, 17, 26};
        // CorTable = {
        //     {18, 3, 2, 0, 0, 0, 0},
        //     {2, 20, 0, 0, 0, 0, 0},
        //     {3, 5, 10, 2, 0, 0, 0},
        //     {0, 0, 7, 12, 5, 0, 0},
        //     {0, 0, 0, 0, 20, 3, 0},
        //     {0, 0, 0, 0, 0, 45, 5}
        // };
        int [] x_n = new int[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            int sum_x = 0;
            for (int j = 0; j < y.Length ; j++)
            {
              sum_x += CorTable[j, i];
            }
            x_n[i] = sum_x;
            Console.WriteLine("x_n: " + x_n[i]);
        }

        double [] yxk = new double[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
          for (int j = 0; j < y.Length; j++)
          {
            yxk[i] += y[j] * CorTable[j, i];
          }
          yxk[i] /= x_n[i];
          Console.WriteLine("y_xk: " + yxk[i]);
        }

        double [] firstEquation = new double[3];
        double [] secondEquation = new double[3];

        for (int i = 0; i < x.Length; i++)
        {
          firstEquation[0] += x[i] * x_n[i];
          firstEquation[1] += x_n[i];
          firstEquation[2] += x_n[i] * Math.Log10(yxk[i]);

          secondEquation[0] += x[i] * x[i] * x_n[i];
          secondEquation[1] += x[i] * x_n[i];
          secondEquation[2] += x_n[i] * x[i] * Math.Log10(yxk[i]);
        }
        
        var A = Matrix<double>.Build.DenseOfArray(new double[,] {
          {firstEquation[0], firstEquation[1]},
          {secondEquation[0], secondEquation[1]}
        });

        var B = Vector<double>.Build.Dense(new double[] {
          firstEquation[2],
          secondEquation[2]
        });

        var X = A.Solve(B);

        Console.WriteLine($"a = {Math.Pow(10, X[0])}");
        Console.WriteLine($"b = {Math.Pow(10, X[1])}");
        
        double a = Math.Pow(10, X[0]);
        double b = Math.Pow(10, X[1]);


        var points = Chart2D.Chart.Point<int, double, double>(x, yxk, Name: "M(x_i, y_ser_i)");
        double[] xRegres = new double[10000];
        double[] yRegres = new double[10000];
        xRegres[0] = -1;
        yRegres[0] = b * Math.Pow(a, xRegres[0]);
        for (int i = 1; i < 10000; i++)
        {
          xRegres[i] = xRegres[i - 1] + 0.0012;
          yRegres[i] = b * Math.Pow(a, xRegres[i]);
        }
        var Line = Chart2D.Chart.Line<double, double, double>(x: xRegres, y: yRegres, Name: "y = b * a^x");
        var chart = Chart.Combine(new [] {points, Line});
        chart.Show();
        
        Console.WriteLine("Assumption: Exponential function");
        double delta = 0;
        for (int i = 0; i < y.Length; i++)
        {
          for ( int j = 0; j < x.Length; j++)
          {
            delta += CorTable[i, j] * Math.Pow(y[i] - b * Math.Pow(a, x[j]), 2);
          }
          Console.WriteLine($"delta_{i} = {delta}");
        }
        double D = delta / x_n.Sum();

        Console.WriteLine($"D = {D}");

        double deltaSqr = 0;
        for (int i = 0; i < x.Length; i++)
        {
          deltaSqr += Math.Pow(yxk[i] - b * Math.Pow(a, x[i]), 2) * x_n[i];
        }

        Console.WriteLine($"deltaSqr = {deltaSqr}");
        
        var menu = new SuperDuperMenu();
        var entries = new Dictionary<string, Action>
        {
            {"SuperDuperMenu", () => menu.Run()},
        };
        menu.LoadEntries(entries);
        // menu.Run();
      }
   }
}