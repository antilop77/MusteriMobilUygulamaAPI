namespace MusteriMobilUygulamaAPI.Models
{
    public class cLoggerX
    {
        public int user_id { get; set; }
        public string? request_url { get; set; }
        public string? request_ip { get; set; }
        public string? query_string { get; set; }
        public string? exception { get; set; }
        public string? request_body { get; set; }
        public string? user_type { get; set; }
    }
}