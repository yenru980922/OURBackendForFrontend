namespace BackendForFrontend.Models.Dtos
{
    public class MemberRegistrationDto
    {
        public string Name { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }


    }
}
