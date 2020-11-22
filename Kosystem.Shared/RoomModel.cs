namespace Kosystem.Shared
{
    public record RoomModel(int Id, string Name)
    {
        public RoomModel Update(UpdateRoomModel patch)
        {
            return this with { Name = patch.Name };
        }
    }
}
