using System;


namespace WorkingStandards.Entities.External
{
    /// <summary>
    /// Деталь и их отчеты
    /// </summary>
    /// <inheritdoc />
    public class DetailPrint : IComparable<DetailPrint>
    {
        public decimal CodeDetail { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; }
        public bool IsPrintFabrik { get; set; }
        public bool IsPrintWorkGuild { get; set; }
        public bool IsPrintWorkGuild02 { get; set; }
        public bool IsPrintWorkGuild03 { get; set; }
        public bool IsPrintWorkGuild04 { get; set; }
        public bool IsPrintWorkGuild05 { get; set; }

        public int CompareTo(DetailPrint other)
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
            var isFabrikComparison = IsPrintFabrik.CompareTo(other.IsPrintFabrik);
            if (isFabrikComparison != 0)
            {
                return isFabrikComparison;
            }
            var isWorkGuildComparison = IsPrintWorkGuild.CompareTo(other.IsPrintWorkGuild);
            if (isWorkGuildComparison != 0)
            {
                return isWorkGuildComparison;
            }
            var isWorkGuild02Comparison = IsPrintWorkGuild02.CompareTo(other.IsPrintWorkGuild02);
            if (isWorkGuild02Comparison != 0)
            {
                return isWorkGuild02Comparison;
            }
            var isWorkGuild03Comparison = IsPrintWorkGuild03.CompareTo(other.IsPrintWorkGuild03);
            if (isWorkGuild03Comparison != 0)
            {
                return isWorkGuild03Comparison;
            }
            var isWorkGuild04Comparison = IsPrintWorkGuild04.CompareTo(other.IsPrintWorkGuild04);
            if (isWorkGuild04Comparison != 0)
            {
                return isWorkGuild04Comparison;
            }
            return IsPrintWorkGuild05.CompareTo(other.IsPrintWorkGuild05);
        }

        protected bool Equals(DetailPrint other)
        {
            const StringComparison comparisonIgnoreCase = StringComparison.OrdinalIgnoreCase;
            return CodeDetail == other.CodeDetail 
                   && string.Equals(Name, other.Name, comparisonIgnoreCase) 
                   && string.Equals(Mark, other.Mark, comparisonIgnoreCase) 
                   && IsPrintFabrik == other.IsPrintFabrik && IsPrintWorkGuild == other.IsPrintWorkGuild
                   && IsPrintWorkGuild02 == other.IsPrintWorkGuild02
                   && IsPrintWorkGuild03 == other.IsPrintWorkGuild03
                   && IsPrintWorkGuild04 == other.IsPrintWorkGuild04
                   && IsPrintWorkGuild05 == other.IsPrintWorkGuild05;
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
            return Equals((DetailPrint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CodeDetail.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^ (Mark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Mark) : 0);
                hashCode = (hashCode * 397) ^ IsPrintFabrik.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPrintWorkGuild.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPrintWorkGuild02.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPrintWorkGuild03.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPrintWorkGuild04.GetHashCode();
                hashCode = (hashCode * 397) ^ IsPrintWorkGuild05.GetHashCode();
                return hashCode;
            }
        }
    }
}
