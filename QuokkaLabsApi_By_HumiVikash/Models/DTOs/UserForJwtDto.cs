namespace QuokkaLabsApi_By_HumiVikash.Models.DTOs
{
    public class UserForJwtDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Role { get; set; }

        public bool isSuccess { get; set; }
    }
}
