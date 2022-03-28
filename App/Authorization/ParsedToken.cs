namespace App.Authorization
{
    public class ParsedToken
    {
        public uint? userId { get; set; }
        public string userRole { get; set; }

        public ParsedToken(uint? userId, string userRole)
        {
            this.userId = userId;
            this.userRole = userRole;
        }
    }
}
