namespace Kosystem.Shared
{
    public class RoomModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PersonModelCollection People { get; } = new PersonModelCollection();

        public RoomModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
