namespace PB503_Libary_Managment_System_ASP.NET.Models
{
	public class BaseEntity
	{
        public int ID { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
