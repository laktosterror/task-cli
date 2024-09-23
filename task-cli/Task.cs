namespace task_cli;

public class Task(string name)
{
    private string _name = name;
    public string CreatedAt { get; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
    public string UpdatedAt { get; set; } = "Never Updated";
    public string Status { get; set; } = "Todo";

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
        }
    }
}