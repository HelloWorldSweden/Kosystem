using System;
using System.Threading;

namespace Kosystem.Utility
{
    public class ConcurrentIdGenerator
    {
        private int _lastId = unchecked((int)DateTime.UtcNow.Ticks);

        public int Next(int limit)
        {
            return GenerateId(Interlocked.Increment(ref _lastId), limit);
        }

        private static int GenerateId(int counter, int limit)
        {
            return NotLowerThan1(unchecked((int)Hash((uint)counter) & limit));
        }

        /// <summary>
        /// Taken from <see href="https://www.geeksforgeeks.org/compute-the-minimum-or-maximum-max-of-two-integers-without-branching/"/>
        /// </summary>
        private static int NotLowerThan1(int a)
        {
            return a ^ ((a ^ 1) & -(a << 1));
        }

        /// <summary>
        /// Taken from <see href="https://gist.github.com/badboy/6267743"/>
        /// </summary>
        private static uint Hash(uint a)
        {
            a = (a + 0x7ed55d16) + (a << 12);
            a = (a ^ 0xc761c23c) ^ (a >> 19);
            a = (a + 0x165667b1) + (a << 5);
            a = (a + 0xd3a2646c) ^ (a << 9);
            a = (a + 0xfd7046c5) + (a << 3);
            a = (a ^ 0xb55a4f09) ^ (a >> 16);
            return a;
        }
    }
}
