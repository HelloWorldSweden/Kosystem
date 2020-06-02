namespace Kosystem.States
{
    public class Person
    {
        public int RoomId { get; set; }

        public string Name { get; set; }

        public Person(int roomId, string name)
        {
            RoomId = roomId;
            Name = name;
        }
    }
}
