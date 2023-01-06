using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MyDotNetNlogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DBLogController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly NLog.Logger _logger;
        private readonly string _connectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        public DBLogController(IConfiguration config)
        {
            // Get the global log instance
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _config = config;
            _connectionString = _config.GetValue<string>("NLog:targets:database:connectionString");

            // Example log with a property
            _logger.WithProperty("Filter", 10).Debug($"Connection-String is {_connectionString}");
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        [HttpPost(Name = "MakeALogEntryForFilterUserMessage")]
        public void Post(int filter = 10, string user = "User", string message = "Message")
        {
            string str = $"Entering Post() withe values filter: {filter} user: {user} message: {message} MakeALogEntryForFilterUserMessage";
            System.Diagnostics.Debug.WriteLine(str);
            // Example log with a property
            _logger.WithProperty("Filter", 11).Debug(str);

            // Example log with a set of properties
            var prop = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Filter", filter),
                new KeyValuePair<string, object>("AppUser", user)
            };
            _logger.WithProperties(prop).Info(message);

            // Example log with a property
            _logger.WithProperty("Filter", 11).Debug("Leaving Post() MakeALogEntryForFilterUserMessage");
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetLogRecords")]
        public IEnumerable<LogRecord> Get(int n = 10)
        {
            // Example Log with a property
            _logger.WithProperty("Filter", 12).Debug("Entering Get() GetLogRecords");

            LogRecord[] logRecord = DBGetLogEntries(n).ToArray();

            // Example Log with a property
            _logger.WithProperty("Filter", 12).Debug("Leaving Get() GetLogRecords");

            return logRecord;
        }

        /// <summary>
        /// DBGetLogEntries
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private List<LogRecord> DBGetLogEntries(int n = 10)
        {
            string connectionString = _connectionString;
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string sql = $"SELECT TOP {n} Id, Added_Date, Level, Filter, AppUser, Message, StackTrace, Exception, Logger, RequestUrl, RequestType  FROM dbo.AppLogs WHERE Filter > 0 ORDER BY Id DESC;";
            SqlCommand cmd = new SqlCommand(sql, connection);

            List<LogRecord> logRecordList = new List<LogRecord>(); ;
            LogRecord logRecord;
            SqlDataReader reader = cmd.ExecuteReader();
            int counter = 0;
            while (reader.Read())
            {
                logRecord = new LogRecord
                {
                    Id = (Int64)reader.GetValue(0),
                    Added_Date = (DateTime)reader.GetValue(1),
                    Level = (string)reader.GetValue(2),
                    Filter = (int)reader.GetValue(3),
                    AppUser = (string)reader.GetValue(4),
                    Message = (string)reader.GetValue(5),
                    StackTrace = (string)reader.GetValue(6),
                    Logger = (string)reader.GetValue(7),
                    Exception = (string)reader.GetValue(8),
                    RequestUrl = (string)reader.GetValue(9),
                    RequestType = (string)reader.GetValue(10)
                };
                logRecordList.Add(logRecord);
                counter++;
                // just for debugging
                string rowResult = string.Format("Id {0} Added_Date {1} Level {2} Filter {3} AppUser {4} Message {5} StackTrace {6}  Logger {8} Exception {7} RequestUrl {9} RequestType {10}",
                    reader.GetValue(0),
                    reader.GetValue(1),
                    reader.GetValue(2),
                    reader.GetValue(3),
                    reader.GetValue(4),
                    reader.GetValue(5),
                    reader.GetValue(6),
                    reader.GetValue(7),
                    reader.GetValue(8),
                    reader.GetValue(9),
                    reader.GetValue(10));
                System.Diagnostics.Debug.WriteLine(rowResult);
            }

            reader.Close();
            cmd.Dispose();
            connection.Close();

            // Example Log with a property
            _logger.WithProperty("Filter", 13).Debug($"We fetched {counter} records in DBGetLogEntries() where Filter > 0");

            return logRecordList;
        }
    }
}