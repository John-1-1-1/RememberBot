using RememberBot.Kernel;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramWorker.DataBaseContext;

namespace RememberBot.TelegramWorker.Services;

public class DataBaseService {
 private readonly ApplicationContext _applicationContext;
    private readonly ILogger<DataBaseService> _logger;
    
    public DataBaseService(ILogger<DataBaseService> logger, IServiceProvider serviceProvider) {
        _logger = logger;
        var scope = serviceProvider.CreateScope();
        var applicationContext = scope.ServiceProvider.GetService<ApplicationContext>();

        if (applicationContext != null) {
            _applicationContext = applicationContext;
        }
        else {
            _logger.LogError(typeof(ApplicationContext) + " is null");
            throw new Exception(typeof(ApplicationContext) + " is null");
        }
    }

    public void AddUser(TelegramUser? user) {
        try {

            if (user == null) {
                return;
            }
            
            var findUser = _applicationContext.Users.FirstOrDefault(u => u.TgId == user.TgId);

            if (findUser == null) {
                _applicationContext.Users.Add(user);
                _applicationContext.SaveChanges();
            }
        }
        catch {
            _logger.LogError("AddUser: ApplicationContext incorrect");
        }
    }

    public void UpdateUser(TelegramUser user) { 
        try { 
            _applicationContext.Users.Update(user); 
            _applicationContext.SaveChanges();
        } catch { 
            _logger.LogError("UpdateUser: ApplicationContext incorrect"); 
        }
    }


    public TelegramUser? GetUser(long? tgId) {
        if (tgId == null) {
            return null;
        }

        try {
            return _applicationContext.Users.FirstOrDefault(u => u.TgId == tgId);
        }
        catch {
            _logger.LogError("GetUser: ApplicationContext incorrect");
            return null;
        }
    }
    
    public void AddTasks(TelegramTask tasks) {
        try { 
            _applicationContext.Task.Add(tasks); 
            _applicationContext.SaveChanges();
        } catch { 
            _logger.LogError("AddTasks: ApplicationContext incorrect"); 
        }
    }
    
    public void UpdateTask(TelegramTask task) {
        try {
            _applicationContext.Task.Update(task);
            _applicationContext.SaveChanges();
        }
        catch (Exception e) {
            _logger.LogError("UpdateTask: ApplicationContext incorrect"); 
        }
    }
}