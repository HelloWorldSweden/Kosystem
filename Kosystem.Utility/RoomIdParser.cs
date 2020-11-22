using System.Text.RegularExpressions;

namespace Kosystem.Utility
{
    public static class RoomIdParser
    {
        private static readonly Regex _roomIdRegex = new Regex(@"^\s*#?\s*(\d+)\s*$", RegexOptions.Compiled);

        public static bool TryParse(string roomIdString, out int roomId)
        {
            var match = _roomIdRegex.Match(roomIdString);
            var idGroup = match.Groups[1];

            if (match.Success is false || idGroup.Success is false)
            {
                roomId = default;
                return false;
            }

            return int.TryParse(idGroup.Value, out roomId);
        }

        public static bool RoomIdEquals(string roomIdString, int roomId)
        {
            var match = _roomIdRegex.Match(roomIdString);
            var idGroup = match.Groups[1];

            if (match.Success is false || idGroup.Success is false)
            {
                return false;
            }

            return int.TryParse(idGroup.Value, out var id) && roomId == id;
        }
    }
}
