using System;

namespace WorkingStandards.Entities.External
{
    /// <summary>
    /// Деталь
    /// </summary>
    public class Detail : IComparable<Detail>
    {
        public decimal CodeDetail { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; }

        public int CompareTo(Detail other)
        {
            const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }
            var codeDetailComparison = CodeDetail.CompareTo(other.CodeDetail);
            if (codeDetailComparison != 0)
            {
                return codeDetailComparison;
            }
            var nameComparison = string.Compare(Name, other.Name, ordinalIgnoreCase);
            if (nameComparison != 0)
            {
                return nameComparison;
            }
            return string.Compare(Mark, other.Mark, ordinalIgnoreCase);
        }

        protected bool Equals(Detail other)
        {
            const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
            return CodeDetail == other.CodeDetail 
                   && string.Equals(Name, other.Name, ordinalIgnoreCase) 
                   && string.Equals(Mark, other.Mark, ordinalIgnoreCase);
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
            return Equals((Detail) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CodeDetail.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^ (Mark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Mark) : 0);
                return hashCode;
            }
        }
    }
}
