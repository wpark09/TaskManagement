using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TaskManager
{
    class App
    {
        private List<string> taskList = new List<string>();

        const int tasksPerPage = 25;
        char invisibleMarker = '\u200b';

        string path = @"C:\Users\wpark\Documents\ToDoList.txt";

        public App()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
        }

        public void Run()
        {
            ReadListFromFile();
            DisplayMenu();
        }

        private void DisplayMenu()
        {
            bool valid = false;
            do
            {
                Console.Clear();
                ASCIIMenu();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("=======================================================================================");
                Console.WriteLine("Main menu:             D: Display tasks   ||   A: Add a task   ||   Q: Quit");
                Console.WriteLine("=======================================================================================");
                Console.ResetColor();

                var userInput = GetUserInput();

                switch (userInput)
                {
                    case ConsoleKey.D:
                        DisplayTasks();
                        valid = true;
                        break;
                    case ConsoleKey.A:
                        AddATask();
                        valid = true;
                        break;
                    case ConsoleKey.Q:
                        WriteListToFile();
                        Console.Clear();
                        Console.WriteLine("Task list is updated. \nGood bye.");
                        valid = true;
                        break;
                }
            } while (!valid);
        }

        private ConsoleKey GetUserInput()
        {
            return Console.ReadKey().Key;
        }

        private void DisplayTasks()
        {
            Console.Clear();
            RemoveFirstActionedItems();

            int totalTasks = taskList.Count;
            int pageNumber = 0;
            int totalPages;
            totalPages = (totalTasks / tasksPerPage) + 1;

            var page = (pageNumber / tasksPerPage);
            var startingPoint = FirstElementInPage(page);
            int EndingPoint = FirstElementInPage(page + 1);

            try
            {
                for (int i = startingPoint; (i < EndingPoint) && (i < taskList.Count); ++i)

                {
                    if (taskList[i].Contains(invisibleMarker))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{i + 1}. {taskList[i].Trim(invisibleMarker)}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. {taskList[i]}");
                    }
                }
                ++pageNumber;
                Console.WriteLine();

            }
            catch (ArgumentOutOfRangeException)
            {; }
            PrintListMenu();

            bool valid = false;

            var userInput = GetUserInput();

            do
            {
                switch (userInput)
                {
                    case ConsoleKey.S:
                        SelectTask();
                        valid = true;
                        break;
                    case ConsoleKey.N:
                        pageNumber = 1;
                        NextPage();
                        Console.Clear();
                        break;
                    case ConsoleKey.Q:
                        DisplayMenu();
                        valid = true;
                        break;
                }
            } while (!valid);
        }

        private void NextPage()
        {
            Console.Clear();
            bool valid = false;
            int count;
            int pageNumber = 1;
            for (pageNumber = 1; pageNumber < taskList.Count / tasksPerPage; ++pageNumber)
            {
                for (count = (pageNumber * tasksPerPage); count < ((pageNumber + 1) * tasksPerPage); ++count)
                {
                    if (taskList[count].Contains(invisibleMarker))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{count + 1}. {taskList[count].Trim(invisibleMarker)}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"{count + 1}. {taskList[count]}");
                    }
                }
                PrintListMenu();
                pageNumber++;
                var input = GetUserInput();
                do
                {
                    switch (input)
                    {
                        case ConsoleKey.S:
                            SelectTask();
                            valid = true;
                            break;
                        case ConsoleKey.N:
                            Console.Clear();
                            pageNumber = 2;
                            try
                            {
                                for (count = (pageNumber * tasksPerPage); count < ((pageNumber + 1) * tasksPerPage); ++count)
                                {
                                    if (taskList[count].Contains(invisibleMarker))
                                    {
                                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine($"{count + 1}. {taskList[count].Trim(invisibleMarker)}");
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.WriteLine($"{count + 1}. {taskList[count]}");
                                    }
                                }
                            }

                            catch (ArgumentOutOfRangeException)
                            {
                                ;
                            }
                            {
                                Console.WriteLine("=======================================================================================");
                                Console.WriteLine("     End of List\n");
                            }
                            break;
                        case ConsoleKey.Q:
                            DisplayMenu();
                            valid = true;
                            break;
                    }
                } while (!valid);

            }


        }

        private int getPage(int i)
        {
            return i / tasksPerPage;

        }

        private static int FirstElementInPage(int page)
        {
            return page * tasksPerPage;
        }

        private void RemoveFirstActionedItems()
        {
            int i = getPage(0) * tasksPerPage;

            if (taskList[i].Contains(invisibleMarker))
            {
                ++i;
            }

        }

        private static void PrintListMenu()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("=======================================================================================");
            Console.WriteLine("Display Menu:             S: Select a task   ||   N: Next page   ||   Q: Quit");
            Console.WriteLine("=======================================================================================");
            Console.ResetColor();
        }

        private void SelectTask()
        {
            Console.Write("Enter index number of the task: ");
            int userSelect = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            Console.WriteLine($"\n\n\n{taskList[userSelect - 1]}\n\n\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("================================================================================================================");
            Console.WriteLine("Menu||   C: Mark as complete   ||   A: Add task back to the list   ||   D: Delete the task   ||   Q: Quit");
            Console.WriteLine("================================================================================================================");
            Console.ResetColor();
            int i = 0;
            int pageNumber = userSelect / tasksPerPage;
            var selection = GetUserInput();

            switch (selection) //Task complete or incomplete?
            {
                case ConsoleKey.C: //attach a string to highlight 
                    Console.Clear();

                    taskList[userSelect - 1] += invisibleMarker;

                    for (i = 0; i < 25; ++i)
                    {
                        if (taskList[i].Contains(invisibleMarker))
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"{i + 1}. {taskList[i].Trim(invisibleMarker)}");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"{i + 1}. {taskList[i]}");
                        }
                    }
                    DisplayTasks();
                    break;
                case ConsoleKey.A://add task back to the bottom of the list
                    Console.Clear();
                    taskList.Insert(pageNumber * tasksPerPage + (tasksPerPage - 1), taskList[userSelect - 1]);
                    taskList.RemoveAt(userSelect - 1);
                    DisplayTasks();
                    break;
                case ConsoleKey.D:
                    taskList.RemoveAt(userSelect - 1);
                    DisplayTasks();
                    break;
                case ConsoleKey.Q:
                    DisplayTasks();
                    break;
            }
        }

        private void AddATask()
        {
            string task;

            for (int count = 0; count > -1; count++)
            {
                Console.Clear();
                Console.Write("Enter a new task: ");
                task = Console.ReadLine();
                taskList.Add(task);
                Console.Clear();
                Console.WriteLine($"({task}) added.");


                Console.WriteLine("Add more tasks?: \nA: Add another task   ||   Q: Quit");
                var userInput = GetUserInput();
                bool valid = false;
                do
                {
                    switch (userInput)
                    {
                        case ConsoleKey.A:
                            AddATask();
                            valid = true;
                            break;
                        case ConsoleKey.Q:
                            DisplayMenu();
                            valid = true;
                            break;
                    }
                } while (!valid);
            }
        }

        private void ReadListFromFile()
        {

            try
            {
                taskList = new List<string>(File.ReadAllLines(path));
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("File does not exist.");
            }
        }

        private void WriteListToFile()
        {
            StreamWriter sw = new StreamWriter(path); //Add true bool inside parameter
                                                      //skips creating txt file if it exists

            foreach (string element in taskList)
                sw.Write($"{element}\r\n");
            sw.Close();

        }

        private void ASCIIMenu()
        {
            Console.Title = "ASCII Art";
            Console.ForegroundColor = ConsoleColor.Blue;
            string title = @"                     :'######::'####:'##::::'##:'########::'##:::::::'########:
                     '##... ##:. ##:: ###::'###: ##.... ##: ##::::::: ##.....::
                      ##:::..::: ##:: ####'####: ##:::: ##: ##::::::: ##:::::::
                     . ######::: ##:: ## ### ##: ########:: ##::::::: ######:::
                     :..... ##:: ##:: ##. #: ##: ##.....::: ##::::::: ##...::::
                     '##::: ##:: ##:: ##:.:: ##: ##:::::::: ##::::::: ##:::::::
                     . ######::'####: ##:::: ##: ##:::::::: ########: ########:
                     :......:::....::..:::::..::..:::::::::........::........::
           :'######:::'######:::::'###::::'##::: ##:'##::: ##:'####:'##::: ##::'######:::
           '##... ##:'##... ##:::'## ##::: ###:: ##: ###:: ##:. ##:: ###:: ##:'##... ##::
            ##:::..:: ##:::..:::'##:. ##:: ####: ##: ####: ##:: ##:: ####: ##: ##:::..:::
           . ######:: ##:::::::'##:::. ##: ## ## ##: ## ## ##:: ##:: ## ## ##: ##::'####:
           :..... ##: ##::::::: #########: ##. ####: ##. ####:: ##:: ##. ####: ##::: ##::
           '##::: ##: ##::: ##: ##.... ##: ##:. ###: ##:. ###:: ##:: ##:. ###: ##::: ##::
           . ######::. ######:: ##:::: ##: ##::. ##: ##::. ##:'####: ##::. ##:. ######:::
           :......::::......:::..:::::..::..::::..::..::::..::....::..::::..:::......::::
            ";
            Console.WriteLine(title);
            Console.ResetColor();
        }

    }
}