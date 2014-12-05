using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    public sealed class TimelineSpanIndication
        : IEquatable<TimelineSpanIndication>
    {
        public static readonly TimelineSpanIndication Zero = new TimelineSpanIndication(TimeSpan.Zero);


        public TimelineSpanIndication(
            TimeSpan value, 
            string description)
        {
            this.Value = value;
            this.Description = description ?? string.Empty;
        }


        public TimelineSpanIndication(TimeSpan value)
            : this(value, string.Empty)
        {
        }


        public TimeSpan Value { get; private set; }
        public string Description { get; set; }


        #region Equality override

        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to TimelineSpanIndication return false:
            var p = obj as TimelineSpanIndication;
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Value == p.Value;
        }


        public bool Equals(TimelineSpanIndication other)
        {
            return this.Value == other.Value;
        }


        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }


        public static bool operator ==(
            TimelineSpanIndication a, 
            TimelineSpanIndication b)
        {
            // If both are null, or both are same instance, return true.
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || 
                ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Value == b.Value;
        }


        public static bool operator !=(
            TimelineSpanIndication a, 
            TimelineSpanIndication b)
        {
            return !(a == b);
        }

        #endregion


        #region Relational override

        public static bool operator >(TimelineSpanIndication v1, TimelineSpanIndication v2)
        {
            return (v1.Value > v2.Value);
        }


        public static bool operator <(TimelineSpanIndication v1, TimelineSpanIndication v2)
        {
            return (v1.Value < v2.Value);
        }


        public static bool operator >=(TimelineSpanIndication v1, TimelineSpanIndication v2)
        {
            return (v1.Value >= v2.Value);
        }


        public static bool operator <=(TimelineSpanIndication v1, TimelineSpanIndication v2)
        {
            return (v1.Value <= v2.Value);
        }

        #endregion

    }
}
