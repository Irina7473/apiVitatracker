namespace MedicineReminderAPI.Service
{
    public class AuthResult
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public AuthResult(int id, string token)
        {
            Id = id;
            Token = token;
        }
    }
}
