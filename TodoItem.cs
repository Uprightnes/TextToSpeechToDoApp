using System;
public class TodoItem
{
    public string TaskName { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime ReminderTime { get; set; }

    public TodoItem(string taskName, string description, DateTime dueDate, DateTime reminderTime)
    {
        TaskName = taskName;
        Description = description;
        DueDate = dueDate;
        ReminderTime = reminderTime;
    }

    public override string ToString()
    {
        return $"{TaskName} - {Description} (Due: {DueDate.ToShortDateString()}, Reminder: {ReminderTime.ToShortTimeString()})";
    }
}