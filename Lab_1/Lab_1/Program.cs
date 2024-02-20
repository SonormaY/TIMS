using Plotly.NET;
using Plotly.NET.LayoutObjects;
using Plotly.NET.TraceObjects;
using LinearAxisTuple = System.Tuple<Plotly.NET.StyleParam.LinearAxisId, Plotly.NET.StyleParam.LinearAxisId>;
using System;
using System.Text;
using System.Diagnostics.Metrics;
using System.ComponentModel;
using System.Runtime.CompilerServices;

Console.OutputEncoding = Encoding.UTF8;

static void Task1(){
    Console.Clear();
    /*Згенерувати вибірку заданого об’єму (не менше 50) з вказаного проміжку для
    дискретної статистичної змінної. На підставі отриманих вибіркових даних:
     побудувати варіаційний ряд та частотну таблицю; представити графічно
    статистичний матеріал, побудувати графік емпіричної функції розподілу
    обчислити числові характеристики дискретного розподілу.*/

    // Задання вибірки
    Console.WriteLine("Введіть початок проміжку: ");
    var start = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Введіть кінець проміжку: ");
    var end = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Введіть об'єм вибірки: ");
    var n = Convert.ToInt32(Console.ReadLine());

    var rand = new Random();
    var sample = new int[n];
    for (int i = 0; i < n; i++)
    {
        sample[i] = rand.Next(start, end + 1);
    }

    // Вибірка
    System.Console.WriteLine("Вибірка: ");
    for (int i = 0; i < n; i++)
    {
        System.Console.Write(sample[i] + " ");
    }
    System.Console.WriteLine();

    // Варіаційний ряд
    Array.Sort(sample);
    Console.WriteLine("Варіаційний ряд: ");
    for (int i = 0; i < n; i++)
    {
        Console.Write(sample[i] + " ");
    }
    Console.WriteLine();

    // Частотна таблиця
    var dict = new System.Collections.Generic.Dictionary<int, int>();
    for (int i = 0; i < n; i++)
    {
        if (dict.ContainsKey(sample[i]))
        {
            dict[sample[i]]++;
        }
        else
        {
            dict.Add(sample[i], 1);
        }
    }

    // Виведення частотної таблиці
    var headers = new int[dict.Keys.Count] [];
    for (int i = 0; i < dict.Keys.ToArray().Length; i++)
    {
        headers[i] = new int[]{dict.Keys.ToArray()[i]};
    }
    var table = ChartDomain.Chart.Table<int[], int, int[], int>(
        headerValues: headers,
        cellsValues: new int[][]{
            dict.Values.ToArray()
        }
    );

    // Графік
    var poligon = Chart2D.Chart.Line<int, int, int>(
        x: dict.Keys,
        y: dict.Values.ToArray()
    );

    var diagram = Chart2D.Chart.Column<int, int, int, int, int>(
        values: dict.Values,
        Keys: dict.Keys.ToArray()
    );

    // Емпірична функція розподілу
    var emp = new double[dict.Keys.Count];
    for (int i = 0; i < dict.Keys.Count; i++)
    {
        emp[i] = dict.Values.Take(i + 1).Sum() / (double)n;
    }
    var lines = new List<GenericChart.GenericChart>();
    for (int i = 0; i < dict.Keys.Count; i++)
    {
        lines.Add(Chart2D.Chart.Line<int, double, int>(
            x: new int[2]{dict.Keys.ToArray()[i] - 1, dict.Keys.ToArray()[i]},
            y: new double[2] { emp[i], emp[i] }
        ));
    }
    var empiric_func = Chart.Combine(lines);

    Chart.Grid<IEnumerable<GenericChart.GenericChart>>(2,2).Invoke(new[] { poligon, diagram, empiric_func, table }).Show();

    // Числові характеристики
    Console.WriteLine("Центральної тенденції");
    if (n % 2 == 0)
    {
        Console.WriteLine($"Медіана {sample[(n / 2) - 1]}, {sample[n / 2]}");
    }
    else
    {
        Console.WriteLine(sample[(int)Math.Floor(n / 2.0)]);
    }
    var avg = sample.Average();
    Console.WriteLine($"Середнє арифметичне: {avg}");
    var max = 0;
    var mode = 0;
    foreach (var item in dict)
    {
        if (item.Value > max)
        {
            max = item.Value;
            mode = item.Key;
        }
    }
    Console.WriteLine($"Мода: {mode}");
    Console.WriteLine("Розсіювання");
    var d = 0.0;
    for (int i = 0; i < n; i++)
    {
        d += Math.Pow(sample[i] - avg, 2);
    }
    Console.WriteLine($"Девіація: {d}");
    var vs = d / (n - 1);
    Console.WriteLine($"Варіанса: {vs}");
    var s = Math.Sqrt(vs);
    Console.WriteLine($"Стандарт: {s}");
    Console.WriteLine($"Розмах: {Math.Abs(end - start)}");
    Console.WriteLine($"Коефіцієнт варіації: {s / avg}");
    Console.WriteLine("Вибіркова дисперсія: " + d / n);
    Console.WriteLine("Вибіркове середнє квадратичне відхилення: " + Math.Sqrt(d / n));
    
    System.Console.WriteLine("Квантилі: ");
    if (n % 4 == 0)
    {
        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"Q{i} квартиль: {sample[((n / 4) * i) - 1]}");
        }
        Console.WriteLine($"Інтерквартильна широта: {sample[((n / 4) * 3 - 1)] - sample[((n / 4) - 1)]}");
    }
    System.Console.WriteLine();
    if (n % 8 == 0)
    {
        for (int i = 1; i <= 7; i++)
        {
            System.Console.WriteLine($"Q{i} октиль: {sample[((n / 8) * i) - 1]}");
        }
        Console.WriteLine($"Інтероктильна широта: {sample[((n / 8) * 7 - 1)] - sample[((n / 8) - 1)]}");
    }
    System.Console.WriteLine();
    if (n % 10 == 0)
    {
        for (int i = 1; i <= 9; i++)
        {
            System.Console.WriteLine($"Q{i} дециль: {sample[((n / 10) * i) - 1]}");
        }
        Console.WriteLine($"Інтердецильна широта: {sample[((n / 10) * 9 - 1)] - sample[((n / 10) - 1)]}");
    }
    System.Console.WriteLine();
    if (n % 100 == 0)
    {
        for (int i = 1; i <= 99; i++)
        {
            System.Console.WriteLine($"Q{i} центиль: {sample[((n / 100) * i) - 1]}");
        }
        Console.WriteLine($"Інтерквартильна широта: {sample[((n / 100) * 99 - 1)] - sample[((n / 100) - 1)]}");
    }
    System.Console.WriteLine();
    Console.WriteLine("Коефіцієнти асиметрії та ексцесу");
    var a1 = 0.0;
    var a2 = 0.0;
    var a3 = 0.0;
    for (int i = 0; i < n; i++)
    {
        a1 += Math.Pow(sample[i] - avg, 3);
        a2 += Math.Pow(sample[i] - avg, 2);
        a3 += Math.Pow(sample[i] - avg, 4);
    }
    a1 /= n;
    a2 /= n;
    a3 /= n;
    var asym = a1 / Math.Pow(a2, 1.5);
    Console.WriteLine($"Коефіцієнт асиметрії: {asym}");
    var exc = a3 / Math.Pow(a2, 2) - 3;
    Console.WriteLine($"Коефіцієнт ексцесу: {exc}");
}

static void Task2(){
    Console.Clear();
    /*2. Згенерувати вибірку заданого об’єму (не менше 50) з вказаного проміжку для 
    неперервної статистичної змінної. На підставі отриманих вибіркових даних: 
     утворити інтервальний статистичний розподіл, побудувати гістограму та 
    графік емпіричної функції розподілу, обчислити числові характеристики. */

    Console.WriteLine("Введіть початок проміжку: ");
    var start1 = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Введіть кінець проміжку: ");
    var end1 = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Введіть об'єм вибірки: ");
    var n1 = Convert.ToInt32(Console.ReadLine());

    var rand1 = new Random();
    var sample1 = new double[n1];
    for (int i = 0; i < n1; i++)
    {
        sample1[i] = Math.Round(rand1.NextDouble() * (end1 - start1) + start1, 3);
    }

    // Вибірка
    System.Console.WriteLine("Вибірка: ");
    for (int i = 0; i < n1; i++)
    {
        System.Console.Write(sample1[i] + " ");
    }
    // Варіаційний ряд
    Array.Sort(sample1);
    System.Console.WriteLine();
    // Інтервальний статистичний розподіл
    var k = (int)Math.Ceiling(Math.Log(n1, 2));
    var h = (sample1.Max() - sample1.Min()) / k;
    var intervals = new double[k];
    for (int i = 0; i < k; i++)
    {
        intervals[i] = Math.Round(sample1.Min() + h * (i + 1), 3);
    }
    var hist = new System.Collections.Generic.Dictionary<double, int>();
    for (int i = 0; i < k; i++)
    {
        hist.Add(intervals[i], 0);
    }
    for (int i = 0; i < n1; i++)
    {
        for (int j = 0; j < k; j++)
        {
            if (sample1[i] <= intervals[j])
            {
                hist[intervals[j]]++;
                break;
            }
        }
    }
    var headers = new string[hist.Keys.Count] [];
    for (int i = 0; i < hist.Keys.ToArray().Length; i++)
    {
        headers[i] = new string[]{$"({Math.Round(hist.Keys.ToArray()[i] - h, 3)}; {Math.Round(hist.Keys.ToArray()[i], 3)}]"};
    }
    var table = ChartDomain.Chart.Table<string[], string, int[], int>(
        headerValues: headers,
        cellsValues: new int[][]{
            hist.Values.ToArray()
        }
    );

    var histogram = Chart2D.Chart.Histogram<double, int>(
        data: sample1,
        XBins: Bins.init(Start: sample1.Min(), End: sample1.Max(), Size: h + 0.001d),
        orientation: StyleParam.Orientation.Vertical
    );

    var sample_mid = new System.Collections.Generic.Dictionary<double, int>();
    for (int i = 0; i < k; i++)
    {
        sample_mid.Add(i * h - h / 2 ,hist[intervals[i]]);
    }

    // Емпірична функція розподілу
    var emp = new double[sample_mid.Keys.Count];
        for (int i = 0; i < sample_mid.Keys.Count; i++)
        {
            emp[i] = sample_mid.Values.Take(i + 1).Sum() / (double)n1;
        }
        var lines = new List<GenericChart.GenericChart>();
        for (int i = 0; i < sample_mid.Keys.Count; i++)
        {
            lines.Add(Chart2D.Chart.Line<double, double, int>(
                x: new double[2]{sample_mid.Keys.ToArray()[i] - h, sample_mid.Keys.ToArray()[i]},
                y: new double[2] { emp[i], emp[i] }
            ));
        }
        var empiric_func = Chart.Combine(lines);

    Chart.Grid<IEnumerable<GenericChart.GenericChart>>(3, 1).Invoke(new[] { table, histogram, empiric_func}).Show();

    // Числові характеристики
    Console.WriteLine("Центральної тенденції");
    var avg1 = sample1.Average();
    Console.WriteLine($"Середнє арифметичне: {avg1}");
    System.Console.WriteLine("Медіана: ");
    if (n1 % 2 == 0)
    {
        Console.WriteLine($"Медіана {sample1[(n1 / 2) - 1]}, {sample1[n1 / 2]}");
    }
    else
    {
        Console.WriteLine($"Медіана: {sample1[(int)Math.Floor(n1 / 2.0)]}");
    }
    var max1 = 0;
    var mode1 = 0.0;
    foreach (var item in hist)
    {
        if (item.Value > max1)
        {
            max1 = item.Value;
            mode1 = item.Key;
        }
    }
    System.Console.WriteLine($"Мода: {mode1}");
    Console.WriteLine("Розсіювання");
    var d1 = 0.0;
    for (int i = 0; i < n1; i++)
    {
        d1 += Math.Pow(sample1[i] - avg1, 2);
    }
    Console.WriteLine($"Девіація: {d1}");
    var vs1 = d1 / (n1 - 1);
    Console.WriteLine($"Варіанса: {vs1}");
    var s1 = Math.Sqrt(vs1);
    Console.WriteLine($"Стандарт: {s1}");
    Console.WriteLine($"Розмах: {Math.Abs(sample1.Max() - sample1.Min())}");
    Console.WriteLine($"Коефіцієнт варіації: {s1 / avg1}");
    Console.WriteLine($"Вибіркова дисперсія: {d1 / n1}");
    Console.WriteLine($"Вибіркове середнє квадратичне відхилення: {Math.Sqrt(d1 / n1)}");
    System.Console.WriteLine("Квантилі: ");
    if (n1 % 4 == 0)
    {
        for (int i = 1; i <= 3; i++)
        {
            Console.WriteLine($"Q{i} квартиль: {sample1[((n1 / 4) * i) - 1]}");
        }
    }
    System.Console.WriteLine();
    if (n1 % 8 == 0)
    {
        for (int i = 1; i <= 7; i++)
        {
            System.Console.WriteLine($"Q{i} октиль: {sample1[((n1 / 8) * i) - 1]}");
        }
    }
    System.Console.WriteLine();
    if (n1 % 10 == 0)
    {
        for (int i = 1; i <= 9; i++)
        {
            System.Console.WriteLine($"Q{i} дециль: {sample1[((n1 / 10) * i) - 1]}");
        }
    }
    System.Console.WriteLine();
    if (n1 % 100 == 0)
    {
        for (int i = 1; i <= 99; i++)
        {
            System.Console.WriteLine($"Q{i} центиль: {sample1[((n1 / 100) * i) - 1]}");
        }
    }
    System.Console.WriteLine();
    Console.WriteLine("Коефіцієнти асиметрії та ексцесу");
    var a11 = 0.0;
    var a21 = 0.0;
    var a31 = 0.0;
    for (int i = 0; i < n1; i++)
    {
        a11 += Math.Pow(sample1[i] - avg1, 3);
        a21 += Math.Pow(sample1[i] - avg1, 2);
        a31 += Math.Pow(sample1[i] - avg1, 4);
    }
    a11 /= n1;
    a21 /= n1;
    a31 /= n1;
    var asym1 = a11 / Math.Pow(a21, 1.5);
    Console.WriteLine($"Коефіцієнт асиметрії: {asym1}");
    var exc1 = a31 / Math.Pow(a21, 2) - 3;
    Console.WriteLine($"Коефіцієнт ексцесу: {exc1}");

}

while(true){
    Console.Clear();
    Console.WriteLine("Choose a task:");
    Console.WriteLine("1. Task 1");
    Console.WriteLine("2. Task 2");

    int choice = int.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
            Task1();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        case 2:
            Task2();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            break;
        default:
            Console.WriteLine("Invalid choice");
            break;
    }
}
