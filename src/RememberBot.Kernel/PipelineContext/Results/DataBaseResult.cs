using RememberBot.Kernel.Tables;

namespace RememberBot.Kernel.PipelineContext.Results;

public class DataBaseResult {
    private DbState _dbState;
    private TelegramUser? _user;
    private UserTask? _task;

    public DataBaseResult AddUser(TelegramUser? user) {
        _user = user;
        _dbState = DbState.Add;
        return this;
    }
    
    public DataBaseResult UpdateUser(TelegramUser? user) {
        _user = user;
        _dbState = DbState.Update;
        return this;
    }

    public DbState State => _dbState;

    public TelegramUser? User => _user;
}

public enum DbState {
    Add,
    Update
}