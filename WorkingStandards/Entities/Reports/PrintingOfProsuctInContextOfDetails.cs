using System;

namespace WorkingStandards.Entities.Reports
{
	/// <summary>
	/// Запись отчета [Печать по изделиям в разрезе деталей]
	/// </summary>
	public class PrintingOfProsuctInContextOfDetails: IComparable<PrintingOfProsuctInContextOfDetails>
	{
		/// <summary>
		/// Код изделия
		/// </summary>
		public decimal ProductId { get; set; }

		/// <summary>
		/// Наименование изделия
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Марка изделия
		/// </summary>
		public string ProductMark { get; set; }

		/// <summary>
		/// Код детали
		/// </summary>
		public decimal DetalId { get; set; }

		/// <summary>
		/// Наименование детали
		/// </summary>
		public string DetalName { get; set; }

		/// <summary>
		/// Обозначение детали
		/// </summary>
		public string DetalMark { get; set; }

		/// <summary>
		/// Цех
		/// </summary>
		public decimal Kc { get; set; }

		/// <summary>
		/// Количество деталей
		/// </summary>
		public decimal Kol { get; set; }

		/// <summary>
		/// Трудоёмкость
		/// </summary>
		public decimal Vstk { get; set; }

		public decimal Rstk { get; set; }


		public int CompareTo(PrintingOfProsuctInContextOfDetails other)
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
			var productIdComparison = ProductId.CompareTo(other.ProductId);
			if (productIdComparison != 0)
			{
				return productIdComparison;
			}
			var productNameComparison = string.Compare(ProductName, other.ProductName, ordinalIgnoreCase);
			if (productNameComparison != 0)
			{
				return productNameComparison;
			}
			var productMarkComparison = string.Compare(ProductMark, other.ProductMark, ordinalIgnoreCase);
			if (productMarkComparison != 0)
			{
				return productMarkComparison;
			}
			var detalIdComparison = DetalId.CompareTo(other.DetalId);
			if (detalIdComparison != 0)
			{
				return detalIdComparison;
			}
			var detalNameComparison = string.Compare(DetalName, other.DetalName, ordinalIgnoreCase);
			if (detalNameComparison != 0)
			{
				return detalNameComparison;
			}
			var detalMarkComparison = string.Compare(DetalMark, other.DetalMark, ordinalIgnoreCase);
			if (detalMarkComparison != 0)
			{
				return detalMarkComparison;
			}
			var kcComparison = Kc.CompareTo(other.Kc);
			if (kcComparison != 0)
			{
				return kcComparison;
			}
			var kolComparison = Kol.CompareTo(other.Kol);
			if (kolComparison != 0)
			{
				return kolComparison;
			}
			var vstkComparison = Vstk.CompareTo(other.Vstk);
			if (vstkComparison != 0)
			{
				return vstkComparison;
			}
			return Rstk.CompareTo(other.Rstk);
		}

		protected bool Equals(PrintingOfProsuctInContextOfDetails other)
		{
			const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
			return ProductId == other.ProductId 
			       && string.Equals(ProductName, other.ProductName, ordinalIgnoreCase) 
			       && string.Equals(ProductMark, other.ProductMark, ordinalIgnoreCase) 
			       && DetalId == other.DetalId 
			       && string.Equals(DetalName, other.DetalName, ordinalIgnoreCase) 
			       && string.Equals(DetalMark, other.DetalMark, ordinalIgnoreCase) 
			       && Kc == other.Kc 
			       && Kol == other.Kol 
			       && Vstk == other.Vstk 
			       && Rstk == other.Rstk;
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
			return Equals((PrintingOfProsuctInContextOfDetails) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = ProductId.GetHashCode();
				hashCode = (hashCode * 397) ^ (ProductName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductName) : 0);
				hashCode = (hashCode * 397) ^ (ProductMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductMark) : 0);
				hashCode = (hashCode * 397) ^ DetalId.GetHashCode();
				hashCode = (hashCode * 397) ^ (DetalName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(DetalName) : 0);
				hashCode = (hashCode * 397) ^ (DetalMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(DetalMark) : 0);
				hashCode = (hashCode * 397) ^ Kc.GetHashCode();
				hashCode = (hashCode * 397) ^ Kol.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk.GetHashCode();
				return hashCode;
			}
		}
	}
}
