namespace EmprestimosAPI.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
