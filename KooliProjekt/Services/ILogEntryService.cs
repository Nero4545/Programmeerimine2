using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ILogEntryService
    {
        Task<PagedResult<LogEntry>> List(int page, int pageSize, LogEntrySearch? search = null);
        Task<LogEntry?> Get(int id);
        Task Save(LogEntry item);
        Task Delete(int id);
    }
}
