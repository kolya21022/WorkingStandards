using System;

namespace WorkingStandards.Entities.External
{
    /// <summary>
    /// Участок предприятия
    /// </summary>
    public class Area : IComparable<Area>
    {
        public decimal Id { get; set; }

        public int CompareTo(Area other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }
            return Id.CompareTo(other.Id);
        }

        protected bool Equals(Area other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Area)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
