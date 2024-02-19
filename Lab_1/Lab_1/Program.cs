using Plotly.NET;
using Plotly.NET.LayoutObjects;
using Plotly.NET.TraceObjects;
using LinearAxisTuple = System.Tuple<Plotly.NET.StyleParam.LinearAxisId, Plotly.NET.StyleParam.LinearAxisId>;
using System;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;
/*Згенерувати вибірку заданого об’єму (не менше 50) з вказаного проміжку для
дискретної статистичної змінної. На підставі отриманих вибіркових даних:
 побудувати варіаційний ряд та частотну таблицю; представити графічно
статистичний матеріал, побудувати графік емпіричної функції розподілу
обчислити числові характеристики дискретного розподілу.*/

static void Zavd1(){
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
    System.Console.WriteLine("Варіаційний ряд: ");
    for (int i = 0; i < n; i++)
    {
        System.Console.Write(sample[i] + " ");
    }
    System.Console.WriteLine();

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
        Console.WriteLine($"Медіана{(sample[n / 2] + sample[n / 2 - 1]) / 2}");
    }
    else
    {
        Console.WriteLine(sample[n / 2]);
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
    Console.WriteLine("Введіть а: ");
    var a = Convert.ToInt32(Console.ReadLine());
    var kvant = sample[(int)((a * n)/100) - 1];
    Console.WriteLine($"Квантиль порядку {a}: {kvant}");

    System.Console.WriteLine("Коефіцієнти асиметрії та ексцесу");
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
    sample1[i] = rand1.NextDouble() * (end1 - start1) + start1;
}

// Вибірка
System.Console.WriteLine("Вибірка: ");
for (int i = 0; i < n1; i++)
{
    System.Console.Write(sample1[i] + " ");
}
System.Console.WriteLine();
// Варіаційний ряд
Array.Sort(sample1);
System.Console.WriteLine("Варіаційний ряд: ");
for (int i = 0; i < n1; i++)
{
    System.Console.Write(sample1[i] + " ");
}
System.Console.WriteLine();
// Інтервальний статистичний розподіл
var k = (int)Math.Ceiling(Math.Log(n1, 2));
var h = (sample1.Max() - sample1.Min()) / k;
var freq = new int[k];
// for (int i = 0; i < n1; i++)
// {
//     if(sample1[i]){
        
//     }
// }

