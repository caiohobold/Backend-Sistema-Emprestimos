﻿namespace EmprestimosAPI.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
