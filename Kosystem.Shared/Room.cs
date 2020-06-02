namespace Kosystem.States
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Room(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
