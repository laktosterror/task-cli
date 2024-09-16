using Newtonsoft.Json;

namespace task_cli;

internal class Program
{
    private const string DataPath = @".\data.json";
    private static List<Task> _tasks = [];

    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Error: Missing arguments!\nRun \"task-cli.exe help\" for a list of commands!");
        }
        else
        {
            string taskName;
            int taskId;

            switch (args[0])
            {
                case "help":
                    PrintHelp();
                    break;

                case "list":
                    if (args.Length == 1)
                    {
                        ReadTasksFromFile();
                        PrintTasksOverview("none");
                    }
                    else if (args.Length > 1)
                    {
                        var filter = args[1];
                        ReadTasksFromFile();
                        PrintTasksOverview(filter);
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid filter!");
                    }

                    break;

                case "details":
                    if (args.Length > 1 && int.TryParse(args[1], out taskId))
                    {
                        ReadTasksFromFile();
                        taskId = int.Parse(args[1]);
                        PrintTaskDetails(taskId);
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid task ID!");
                    }

                    break;

                case "add":
                    if (args.Length > 1)
                    {
                        ReadTasksFromFile();
                        taskName = args[1];
                        var newTaskId = CreateTask(taskName);
                        Console.WriteLine($"Success: Created task {taskName} with id {newTaskId}.");
                        WriteTasksToFile();
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid task name!");
                    }

                    break;

                case "update":
                    if (args.Length > 1 && int.TryParse(args[1], out taskId))
                    {
                        ReadTasksFromFile();
                        taskId = int.Parse(args[1]);
                        taskName = args[2];
                        UpdateTask(taskId, taskName);
                        Console.WriteLine($"Success: Updated task with id {taskId}, new name {taskName}.");
                        WriteTasksToFile();
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid task ID!");
                    }

                    break;

                case "delete":
                    if (args.Length > 1 && int.TryParse(args[1], out taskId))
                    {
                        ReadTasksFromFile();
                        taskId = int.Parse(args[1]);
                        DeleteTask(taskId);
                        Console.WriteLine($"Success: Deleted task with id {taskId}.");
                        WriteTasksToFile();
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid task ID!");
                    }

                    break;

                case "mark-done":
                    if (args.Length > 1 && int.TryParse(args[1], out taskId))
                    {
                        ReadTasksFromFile();
                        taskId = int.Parse(args[1]);
                        MarkTaskDone(taskId);
                        Console.WriteLine($"Success: Marked task with id {taskId} as done.");
                        WriteTasksToFile();
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid task ID!");
                    }

                    break;

                case "mark-in-progress":
                    if (args.Length > 1 && int.TryParse(args[1], out taskId))
                    {
                        ReadTasksFromFile();
                        taskId = int.Parse(args[1]);
                        MarkTaskInProgress(taskId);
                        Console.WriteLine($"Success: Marked task with id {taskId} as in-progress.");
                        WriteTasksToFile();
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid task ID!");
                    }

                    break;

                default:
                    Console.WriteLine(
                        "Error: Could not parse arguments!\nRun \"task-cli.exe help\" for a list of commands!");
                    break;
            }
        }
    }

    private static void WriteTasksToFile()
    {
        try
        {
            var json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
            File.WriteAllText(DataPath, json);
        }
        catch
        {
            Console.WriteLine("Error: Could not save data to file!");
        }
    }

    private static void ReadTasksFromFile()
    {
        if (File.Exists(DataPath))
            try
            {
                var json = File.ReadAllText(DataPath);
                _tasks = JsonConvert.DeserializeObject<List<Task>>(json);
            }
            catch
            {
                Console.WriteLine("Error: Could not read data from file!");
            }
    }

    private static void PrintHelp()
    {
        const string helpText = """

                                Usage: task-cli <command>
                                                            
                                Commands:
                                    help                          Print this help text.
                                    list                          Print all tasks.
                                    list done                     Print done tasks.
                                    list todo                     Print todo tasks.
                                    list in-progress              Print in-progress tasks.
                                    details <id>                  Print task details.
                                    add <task desc>               Add task with description.
                                    update <id> <task desc>       Update task with new description.
                                    delete <id>                   Delete task.
                                    mark-done <id>                Mark task as done as done.
                                    mark-in-progress <id>         Mark task as in-progress.

                                """;

        Console.WriteLine(helpText);
    }

    private static void PrintTasksOverview(string filter)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine(" Id | Status      | Description ");
        Console.WriteLine("--------------------------------");
        switch (filter.ToLower())
        {
            case "done":
                foreach (var task in _tasks)
                    if (task.Status == "Done")
                        Console.WriteLine($"{task.Id,3} | {task.Status,-11} | {task.Name,2}");
                break;
            case "todo":
                foreach (var task in _tasks)
                    if (task.Status == "Todo")
                        Console.WriteLine($"{task.Id,3} | {task.Status,-11} | {task.Name,2}");
                break;
            case "in-progress":
                foreach (var task in _tasks)
                    if (task.Status == "In Progress")
                        Console.WriteLine($"{task.Id,3} | {task.Status,-11} | {task.Name,2}");
                break;
            default:
                foreach (var task in _tasks)
                    Console.WriteLine($"{task.Id,3} | {task.Status,-11} | {task.Name,2}");
                break;
        }
    }

    private static void PrintTaskDetails(int taskId)
    {
        foreach (var task in _tasks)
        {
            if (task.Id != taskId) continue;
            Console.WriteLine("--------------------------------");
            Console.WriteLine(" Task Details:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"{"Task Id",11} : {task.Id}");
            Console.WriteLine($"{"Task Status",11} : {task.Status}");
            Console.WriteLine($"{"Created At",11} : {task.CreatedAt}");
            Console.WriteLine($"{"Updated At",11} : {task.UpdatedAt}");
            Console.WriteLine($"{"Task Name",11} : {task.Name}");
            break;
        }
    }

    private static int CreateTask(string taskName)
    {
        var availableTaskId = 0;
        foreach (var task in _tasks)
            if (task.Id > availableTaskId)
                availableTaskId = task.Id;

        availableTaskId++;
        _tasks.Add(new Task(availableTaskId, taskName));
        return availableTaskId;
    }

    private static void UpdateTask(int taskId, string taskName)
    {
        foreach (var task in _tasks)
            if (task.Id == taskId)
                task.Name = taskName;
    }

    private static void DeleteTask(int taskId)
    {
        for (var i = 0; i < _tasks.Count; i++)
            if (_tasks[i].Id == taskId)
                _tasks.RemoveAt(i);
    }

    private static void MarkTaskDone(int taskId)
    {
        foreach (var task in _tasks)
            if (task.Id == taskId)
                task.Status = "Done";
    }

    private static void MarkTaskInProgress(int taskId)
    {
        foreach (var task in _tasks)
            if (task.Id == taskId)
                task.Status = "In Progress";
    }
}