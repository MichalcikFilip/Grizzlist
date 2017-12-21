using Grizzlist.Persistent;
using System;

namespace Grizzlist.Tasks.Types
{
    public struct Activity : IPersistentEntity<long>
    {
        public long ID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsActive { get { return End == default(DateTime); } }
        public TimeSpan Length { get { return (IsActive ? DateTime.Now : End) - Start; } }

        public Activity Close()
        {
            End = DateTime.Now;

            return this;
        }

        public static Activity NewActivity()
        {
            return new Activity() { Start = DateTime.Now };
        }
    }
}
