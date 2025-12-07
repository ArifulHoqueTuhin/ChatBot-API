namespace ChatBot_API.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; } 
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsSoftDeleted { get; set; } = false;
    }
}
