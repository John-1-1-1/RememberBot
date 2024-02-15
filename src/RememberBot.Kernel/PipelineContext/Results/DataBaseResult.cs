using RememberBot.Kernel.Tables;

namespace RememberBot.Kernel.PipelineContext.Results;

public class DataBaseResult {
    private DbState _dbStateUser;
    private DbState _dbStateTask;
    private TelegramUser? _user;
    private TelegramTask? _task;

    public DataBaseResult AddUser(TelegramUser? user) {
        _user = user;
        _dbStateUser = DbState.Add;
        return this;
    }
    
    public DataBaseResult UpdateUser(TelegramUser? user) {
        _user = user;
        _dbStateUser = DbState.Update;
        return this;
    }

    public DataBaseResult AddTask(TelegramTask? task) {
        _task = task;
        _dbStateTask = DbState.Add;
        return this;
    }
    
    public DataBaseResult UpdateTask(TelegramTask? task) {
        _task = task;
        _dbStateTask = DbState.Update;
        return this;
    }
    
    public DbState StateTask => _dbStateTask;

    public TelegramTask? Task => _task;
    
    public DbState StateUser => _dbStateUser;

    public TelegramUser? User => _user;
}

