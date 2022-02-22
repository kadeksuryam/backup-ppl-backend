namespace App.Models
{
    public class Level : IEntity
    {
        public uint Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public uint RequiredExp { get; set; }
    }
}
