namespace MyDotNetNlogApi
{
    public class LogRecord
    {
        public Int64 Id { get; set; }
        public DateTime Added_Date { get; set; }
        public string? Level { get; set; }
        public Int32? Filter { get; set; }
        public string? AppUser { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Logger { get; set; }
        public string? Exception { get; set; }
        public string? RequestUrl { get; set; }
        public string? RequestType { get; set; }
    }
}