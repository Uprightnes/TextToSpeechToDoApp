using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Threading;

class Program
{
    static List<TodoItem> todoList = new List<TodoItem>();
    static SpeechSynthesizer synthesizer = new SpeechSynthesizer();

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to my To-Do List");

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add a task");
            Console.WriteLine("2. Edit a task");
            Console.WriteLine("3. Delete a task");
            Console.WriteLine("4. Exit");

            string option = Console.ReadLine();

            if (option == "1")
            {
                Console.WriteLine("\nEnter a task name (or type 'exit' to finish): ");
                string taskName = Console.ReadLine();
                if (taskName.ToLower() == "exit") break;

                Console.WriteLine("Enter a description: ");
                string description = Console.ReadLine();

                Console.WriteLine("Enter the due date (yyyy-mm-dd): ");
                DateTime dueDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Enter the reminder time (hh:mm 24-hour format): ");
                DateTime reminderTime = DateTime.Parse(Console.ReadLine());

                AddTask(taskName, description, dueDate, reminderTime);
                DisplayTodoList();
            }
            else if (option == "2")
            {
                EditTask();
            }
            else if (option == "3")
            {
                DeleteTask();
            }
            else if (option == "4")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option, please try again.");
            }
        }

        // Start checking for reminders
        CheckReminders();
    }

    static void AddTask(string taskName, string description, DateTime dueDate, DateTime reminderTime)
    {
        var newTask = new TodoItem(taskName, description, dueDate, reminderTime);
        todoList.Add(newTask);
        Console.WriteLine($"Added Task: {taskName}");
    }

    static void EditTask()
    {
        DisplayTodoList();
        Console.WriteLine("Enter the number of the task you want to edit:");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < todoList.Count)
        {
            Console.WriteLine("Enter a new task name (leave blank to keep current):");
            string taskName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(taskName))
                todoList[index].TaskName = taskName;

            Console.WriteLine("Enter a new description (leave blank to keep current):");
            string description = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(description))
                todoList[index].Description = description;

            Console.WriteLine("Enter a new due date (yyyy-mm-dd) (leave blank to keep current):");
            string dueDateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dueDateInput))
                todoList[index].DueDate = DateTime.Parse(dueDateInput);

            Console.WriteLine("Enter a new reminder time (hh:mm) (leave blank to keep current):");
            string reminderTimeInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(reminderTimeInput))
                todoList[index].ReminderTime = DateTime.Parse(reminderTimeInput);

            Console.WriteLine("Task updated successfully.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
    }

    static void DeleteTask()
    {
        DisplayTodoList();
        Console.WriteLine("Enter the number of the task you want to delete:");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < todoList.Count)
        {
            todoList.RemoveAt(index);
            Console.WriteLine("Task deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
    }

    static void DisplayTodoList()
    {
        Console.WriteLine("\nMy To-Do List:");
        for (int i = 0; i < todoList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {todoList[i].ToString()}");
        }
    }

    static void ReadTaskAloud(string taskName)
    {
        synthesizer.Speak(taskName); // Speak the task name aloud
    }

    static void CheckReminders()
    {
        // Run the reminder check in a separate thread
        System.Threading.Thread reminderThread = new System.Threading.Thread(() =>
        {
            while (true)
            {
                foreach (var task in todoList)
                {
                    if (DateTime.Now >= task.ReminderTime && task.ReminderTime > DateTime.Now.AddMinutes(-1))
                    {
                        ReadTaskAloud(task.TaskName); // Read the task name aloud when the reminder time is reached
                        Console.WriteLine($"Reminder: {task.TaskName} is due!");
                    }
                }
                System.Threading.Thread.Sleep(60000); // Check every minute
            }
        });
        reminderThread.Start();
    }
}