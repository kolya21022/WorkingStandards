using System;

namespace WorkingStandards.Entities.External
{
    /// <summary>
    /// Деталь расчета сводных трудовых нормативов
    /// </summary>
    /// <inheritdoc />
    public class DetailCalculate : IComparable<DetailCalculate>
    {
        public decimal CodeDetail { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; }
        public bool IsCalculate { get; set; }

        protected bool Equals(DetailCalculate other)
        {
            const StringComparison comparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
            return CodeDetail == other.CodeDetail 
                   && string.Equals(Name, other.Name, comparisonIgnoreCase) 
                   && string.Equals(Mark, other.Mark, comparisonIgnoreCase) 
                   && IsCalculate == other.IsCalculate;
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
            return Equals((DetailCalculate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CodeDetail.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^ (Mark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Mark) : 0);
                hashCode = (hashCode * 397) ^ IsCalculate.GetHashCode();
                return hashCode;
            }
        }

        public int CompareTo(DetailCalculate other)
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
            var codeProductComparison = CodeDetail.CompareTo(other.CodeDetail);
            if (codeProductComparison != 0)
            {
                return codeProductComparison;
            }
            var nameComparison = string.Compare(Name, other.Name, comparisonIgnoreCase);
            if (nameComparison != 0)
            {
                return nameComparison;
            }
            var markComparison = string.Compare(Mark, other.Mark, comparisonIgnoreCase);
            if (markComparison != 0)
            {
                return markComparison;
            }
            return IsCalculate.CompareTo(other.IsCalculate);
        }
    }
}
