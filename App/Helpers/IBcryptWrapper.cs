namespace App.Helpers
{
    public interface IBcryptWrapper
    {
        public string hashPassword(string password);
        public bool isPasswordCorrect(string password, string encryptedPassword);
    }
}
