using System;

namespace PrintSystem.BLL.DTOs
{
    public class PrintJobDto
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public int Copies { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
    }
}