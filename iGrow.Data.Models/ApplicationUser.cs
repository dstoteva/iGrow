namespace iGrow.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    public class ApplicationUser : IdentityUser<Guid>
    {
        public virtual ICollection<MyTask> Tasks { get; set; } = new List<MyTask>();
        public virtual ICollection<Habit> Habits { get; set; } = new List<Habit>();
    }
}
