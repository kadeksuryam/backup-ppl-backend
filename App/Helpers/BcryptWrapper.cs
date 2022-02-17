namespace App.Helpers
{
    using BCrypt = BCrypt.Net.BCrypt;
    public class BcryptWrapper : IBcryptWrapper
    {
        public string hashPassword(string password)
        {
            return BCrypt.HashPassword(password);
        }

        public bool isPasswordCorrect(string password, string encryptedPassword)
        {
            return BCrypt.Verify(password, encryptedPassword);
        }
    }
}
