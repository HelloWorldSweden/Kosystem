using Kosystem.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kosystem.Common
{
    public static class RoomHelpers
    {
        public static List<User> GetSortedEnqueued(this Room room)
        {
            var users = room.Users.Where(o => o.EnqueuedAt.HasValue).ToList();
            users.Sort((a, b) => a.EnqueuedAt.Value.CompareTo(b.EnqueuedAt.Value));
            return users;
        }
    }
}
