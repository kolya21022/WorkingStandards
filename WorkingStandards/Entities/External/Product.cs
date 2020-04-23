using System;
using System.Globalization;

namespace WorkingStandards.Entities.External
{
    /// <summary>
    /// Изделие
    /// </summary>
    public class Product : IComparable<Product>
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Mark { get; set; }

        /// <summary>
        /// Код изделия строковый
        /// </summary>
        public string DisplayCodeString
        {
            get
            {
                var display = Id.ToString(CultureInfo.InvariantCulture);
                while (display.Length < 14)
                {
                    display = "0" + display;
                }
                return display;
            }
        }
        protected bool Equals(Product other)
        {
            const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
            return Id == other.Id
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

            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Product)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : 0);
                hashCode = (hashCode * 397) ^ (Mark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Mark) : 0);
                return hashCode;
            }
        }

        public int CompareTo(Product other)
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
            var idComparison = Id.CompareTo(other.Id);
            if (idComparison != 0)
            {
                return idComparison;
            }
            var nameComparison = string.Compare(Name, other.Name, ordinalIgnoreCase);
            if (nameComparison != 0)
            {
                return nameComparison;
            }
            return string.Compare(Mark, other.Mark, ordinalIgnoreCase);
        }
    }
}