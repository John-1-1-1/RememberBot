namespace RememberBot.Kernel.Tables;

public class TelegramTask {
    public int Id { get; set; }
    public long TgId { get; set; }
    public DateTime DateTime { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public bool IsUsedWordsWeighter { get; set; } = false;

    public TaskPriority Priority { get; set; } = TaskPriority.None;
}