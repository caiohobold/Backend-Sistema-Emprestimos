namespace EmprestimosAPI.DTO.Feedback
{
    public class FeedbackModel
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
