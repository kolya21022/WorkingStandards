using System;

namespace WorkingStandards.Entities.External
{
    /// <summary>
    /// Класс выпуска изделия
    /// </summary>
    /// <inheritdoc />
    public class RealaseProduct : IComparable<RealaseProduct>
    {
        public decimal CodeDetail { get; set; }
        public decimal Realase { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; }

        protected bool Equals(RealaseProduct other)
        {
            const StringComparison comparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
            return CodeDetail == other.CodeDetail 
                   && Realase == other.Realase 
                   && string.Equals(Name, other.Name, comparisonIgnoreCase) 
                   && string.Equals(Mark, other.Mark, comparisonIgnoreCase);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((RealaseProduct) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CodeDetail.GetHashCode();
                hashCode = (hashCode * 397) ^ Realase.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^ (Mark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Mark) : 0);
                return hashCode;
            }
        }

        public int CompareTo(RealaseProduct other)
        {
            const StringComparison comparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
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
            var realaseComparison = Realase.CompareTo(other.Realase);
            if (realaseComparison != 0)
            {
                return realaseComparison;
            }
            var nameComparison = string.Compare(Name, other.Name, comparisonIgnoreCase);
            if (nameComparison != 0)
            {
                return nameComparison;
            }
            return string.Compare(Mark, other.Mark, comparisonIgnoreCase);
        }
    }
}
