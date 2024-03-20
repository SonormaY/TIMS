namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Enter number of Task: ");
                int task = int.Parse(Console.ReadLine());
                switch (task)
                {
                    case 1:
                        Task1.ExecuteTask1();
                        break;
                    case 2:
                        throw new NotImplementedException();
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Task not found");
                        break;
                }
            }
            
        }
    }
}