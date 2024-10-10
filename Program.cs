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

        // Start checking for reminders
        CheckReminders();
    }

    static void AddTask(string taskName, string description, DateTime dueDate, DateTime reminderTime)
    {
        var newTask = new TodoItem(taskName, description, dueDate, reminderTime);
        todoList.Add(newTask);
        Console.WriteLine($"Added Task: {taskName}");
    }

    static void DisplayTodoList()
    {
        Console.WriteLine("\nMy To-Do List:");
        foreach (var task in todoList)
        {
            Console.WriteLine(task.ToString());
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