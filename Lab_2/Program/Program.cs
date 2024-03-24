namespace Program
{
    static class Program
    {
        static void Main(string[] args)
        {
            string[] tasks = { "Task 1", "Task 2", "Exit"};
            int selectedTaskIndex = 0;

            Console.Title = "Main menu";

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                for (int i = 0; i < tasks.Length; i++)
                {
                    Console.ResetColor();
                    if (i == selectedTaskIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("-> " + tasks[i]);
                    }
                    else
                    {
                        Console.WriteLine("   " + tasks[i]);
                    }
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedTaskIndex = (selectedTaskIndex - 1 + tasks.Length) % tasks.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedTaskIndex = (selectedTaskIndex + 1) % tasks.Length;
                        break;
                    case ConsoleKey.Enter:
                        // Perform the selected task
                        Console.Clear();
                        switch (selectedTaskIndex)
                        {
                            case 0:
                                Console.CursorVisible = true;
                                Task1.ExecuteTask1();
                                break;
                            case 1:
                                Console.CursorVisible = true;
                                Task2.ExecuteTask2();
                                break;
                            default:
                                Console.ResetColor();
                                Console.CursorVisible = true;
                                return;
                        }
                        break;
                }
            }
        }
    }
}