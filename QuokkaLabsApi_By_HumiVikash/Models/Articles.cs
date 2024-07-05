namespace QuokkaLabsApi_By_HumiVikash.Models
{
    public class Articles
    {
        public int Id { get; set; }
        public string ArticalName { get; set; }

        public string? ArticalDescription { get; set; }

     
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
