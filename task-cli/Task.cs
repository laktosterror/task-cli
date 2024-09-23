namespace task_cli;

public class Task(string name)
{
    private string _name = name;
    private string _status = "Todo";
    public readonly string CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
    public string UpdatedAt = "Never Updated";

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
        }

    }
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