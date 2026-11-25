using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class LogEntriesIndexModel
    {
        public LogEntrySearch Search { get; set; } = new LogEntrySearch();
        public PagedResult<LogEntry> Data { get; set; } = new PagedResult<LogEntry>();
    }
}
