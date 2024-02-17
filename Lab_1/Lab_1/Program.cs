using Plotly.NET;
using Plotly.NET.TraceObjects;
using System;

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
System.Console.WriteLine("Варіаційний ряд: ");
for (int i = 0; i < n; i++)
{
    System.Console.Write(sample[i] + " ");
}
System.Console.WriteLine();
// Частотна таблиця
System.Console.WriteLine("Частотна таблиця: ");
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
ChartDomain.Chart.Table<int[], int, int[], int>(
    headerValues: headers,
    cellsValues: new int[] []{
        dict.Values.ToArray()
    }
).Show();

// Графік
Chart2D.Chart.Line<int, int, int>(
    x: dict.Keys,
    y: dict.Values.ToArray()
).Show();

Chart2D.Chart.Column<int, int, int ,int, int>(
    values: dict.Values,
    Keys: dict.Keys.ToArray()
).Show();

// Емпірична функція розподілу
var emp = new double[dict.Keys.Count];
for (int i = 0; i < dict.Keys.Count; i++)
{
    emp[i] = dict.Values.Take(i + 1).Sum() / (double)n;
}
var lines = new List<GenericChart.GenericChart>();
for (int i = 0; i < dict.Keys.Count - 1; i++)
{
    lines.Add(Chart2D.Chart.Line<int, double, int>(
        x: new int[2]{dict.Keys.ToArray()[i], dict.Keys.ToArray()[i + 1]},
        y: new double[2] { emp[i], emp[i] }
    ));
}    
Chart.Combine(lines).Show();

