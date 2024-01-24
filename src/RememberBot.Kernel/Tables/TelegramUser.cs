namespace RememberBot.Kernel.Tables;

public class TelegramUser {
    public int Id { get; set; }
    public long TgId { get; set; }
    public int? LocalTime { get; set; }
    public TelegramState UserState { get; set; } = TelegramState.None;
    public string Times { get; set; } = string.Empty;
    public string AddedText { get; set; } = string.Empty;
}