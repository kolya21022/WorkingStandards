using System;


namespace WorkingStandards.Entities.Reports
{
    /// <summary>
    /// Запись отчета [Сводная по изделиям в разрезе цехов и по разрядам]
    /// </summary>
    /// <inheritdoc />
    public class SummeryOfProductInContextOfWorkGuildAndOfDischarge : IComparable<SummeryOfProductInContextOfWorkGuildAndOfDischarge>
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
		/// Цех
		/// </summary>
		public decimal Kc { get; set; }

		/// <summary>
		/// Трудоёмкость для 1 разряда
		/// </summary>
		public decimal Vstk1 { get; set; }

		/// <summary>
		/// Трудоёмкость для 2 разряда
		/// </summary>
		public decimal Vstk2 { get; set; }

		/// <summary>
		/// Трудоёмкость для 3 разряда
		/// </summary>
		public decimal Vstk3 { get; set; }

		/// <summary>
		/// Трудоёмкость для 4 разряда
		/// </summary>
		public decimal Vstk4 { get; set; }

		/// <summary>
		/// Трудоёмкость для 5 разряда
		/// </summary>
		public decimal Vstk5 { get; set; }

		/// <summary>
		/// Трудоёмкость для 6 разряда
		/// </summary>
		public decimal Vstk6 { get; set; }

		public decimal Rstk1 { get; set; }
		public decimal Rstk2 { get; set; }
		public decimal Rstk3 { get; set; }
		public decimal Rstk4 { get; set; }
		public decimal Rstk5 { get; set; }
		public decimal Rstk6 { get; set; }


		public decimal Prtnorm1 { get; set; }
		public decimal Prtnorm2 { get; set; }
		public decimal Prtnorm3 { get; set; }
		public decimal Prtnorm4 { get; set; }
		public decimal Prtnorm5 { get; set; }
		public decimal Prtnorm6 { get; set; }

		public decimal Nadb1 { get; set; }
		public decimal Nadb2 { get; set; }
		public decimal Nadb3 { get; set; }
		public decimal Nadb4 { get; set; }
		public decimal Nadb5 { get; set; }
		public decimal Nadb6 { get; set; }

		public int CompareTo(SummeryOfProductInContextOfWorkGuildAndOfDischarge other)
		{
			var ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
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
			var kcComparison = Kc.CompareTo(other.Kc);
			if (kcComparison != 0)
			{
				return kcComparison;
			}
			var vstk1Comparison = Vstk1.CompareTo(other.Vstk1);
			if (vstk1Comparison != 0)
			{
				return vstk1Comparison;
			}
			var vstk2Comparison = Vstk2.CompareTo(other.Vstk2);
			if (vstk2Comparison != 0)
			{
				return vstk2Comparison;
			}
			var vstk3Comparison = Vstk3.CompareTo(other.Vstk3);
			if (vstk3Comparison != 0)
			{
				return vstk3Comparison;
			}
			var vstk4Comparison = Vstk4.CompareTo(other.Vstk4);
			if (vstk4Comparison != 0)
			{
				return vstk4Comparison;
			}
			var vstk5Comparison = Vstk5.CompareTo(other.Vstk5);
			if (vstk5Comparison != 0)
			{
				return vstk5Comparison;
			}
			var vstk6Comparison = Vstk6.CompareTo(other.Vstk6);
			if (vstk6Comparison != 0)
			{
				return vstk6Comparison;
			}
			var rstk1Comparison = Rstk1.CompareTo(other.Rstk1);
			if (rstk1Comparison != 0)
			{
				return rstk1Comparison;
			}
			var rstk2Comparison = Rstk2.CompareTo(other.Rstk2);
			if (rstk2Comparison != 0)
			{
				return rstk2Comparison;
			}
			var rstk3Comparison = Rstk3.CompareTo(other.Rstk3);
			if (rstk3Comparison != 0)
			{
				return rstk3Comparison;
			}
			var rstk4Comparison = Rstk4.CompareTo(other.Rstk4);
			if (rstk4Comparison != 0)
			{
				return rstk4Comparison;
			}
			var rstk5Comparison = Rstk5.CompareTo(other.Rstk5);
			if (rstk5Comparison != 0)
			{
				return rstk5Comparison;
			}
			var rstk6Comparison = Rstk6.CompareTo(other.Rstk6);
			if (rstk6Comparison != 0)
			{
				return rstk6Comparison;
			}
			var prtnorm1Comparison = Prtnorm1.CompareTo(other.Prtnorm1);
			if (prtnorm1Comparison != 0)
			{
				return prtnorm1Comparison;
			}
			var prtnorm2Comparison = Prtnorm2.CompareTo(other.Prtnorm2);
			if (prtnorm2Comparison != 0)
			{
				return prtnorm2Comparison;
			}
			var prtnorm3Comparison = Prtnorm3.CompareTo(other.Prtnorm3);
			if (prtnorm3Comparison != 0)
			{
				return prtnorm3Comparison;
			}
			var prtnorm4Comparison = Prtnorm4.CompareTo(other.Prtnorm4);
			if (prtnorm4Comparison != 0)
			{
				return prtnorm4Comparison;
			}
			var prtnorm5Comparison = Prtnorm5.CompareTo(other.Prtnorm5);
			if (prtnorm5Comparison != 0)
			{
				return prtnorm5Comparison;
			}
			var prtnorm6Comparison = Prtnorm6.CompareTo(other.Prtnorm6);
			if (prtnorm6Comparison != 0)
			{
				return prtnorm6Comparison;
			}
			var nadb1Comparison = Nadb1.CompareTo(other.Nadb1);
			if (nadb1Comparison != 0)
			{
				return nadb1Comparison;
			}
			var nadb2Comparison = Nadb2.CompareTo(other.Nadb2);
			if (nadb2Comparison != 0)
			{
				return nadb2Comparison;
			}
			var nadb3Comparison = Nadb3.CompareTo(other.Nadb3);
			if (nadb3Comparison != 0)
			{
				return nadb3Comparison;
			}
			var nadb4Comparison = Nadb4.CompareTo(other.Nadb4);
			if (nadb4Comparison != 0)
			{
				return nadb4Comparison;
			}
			var nadb5Comparison = Nadb5.CompareTo(other.Nadb5);
			if (nadb5Comparison != 0)
			{
				return nadb5Comparison;
			}
			return Nadb6.CompareTo(other.Nadb6);
		}

		protected bool Equals(SummeryOfProductInContextOfWorkGuildAndOfDischarge other)
		{
			var ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
			return ProductId == other.ProductId 
			       && string.Equals(ProductName, other.ProductName, ordinalIgnoreCase)
				   && string.Equals(ProductMark, other.ProductMark, ordinalIgnoreCase) 
			       && Kc == other.Kc 
			       && Vstk1 == other.Vstk1 
			       && Vstk2 == other.Vstk2 
			       && Vstk3 == other.Vstk3 
			       && Vstk4 == other.Vstk4 
			       && Vstk5 == other.Vstk5 
			       && Vstk6 == other.Vstk6
			       && Rstk1 == other.Rstk1 
			       && Rstk2 == other.Rstk2 
			       && Rstk3 == other.Rstk3 
			       && Rstk4 == other.Rstk4 
			       && Rstk5 == other.Rstk5 
			       && Rstk6 == other.Rstk6 
			       && Prtnorm1 == other.Prtnorm1 
			       && Prtnorm2 == other.Prtnorm2 
			       && Prtnorm3 == other.Prtnorm3 
			       && Prtnorm4 == other.Prtnorm4 
			       && Prtnorm5 == other.Prtnorm5 
			       && Prtnorm6 == other.Prtnorm6 
			       && Nadb1 == other.Nadb1 
			       && Nadb2 == other.Nadb2 
			       && Nadb3 == other.Nadb3 
			       && Nadb4 == other.Nadb4 
			       && Nadb5 == other.Nadb5 
			       && Nadb6 == other.Nadb6;
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
			return Equals((SummeryOfProductInContextOfWorkGuildAndOfDischarge) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = ProductId.GetHashCode();
				hashCode = (hashCode * 397) ^ (ProductName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductName) : 0);
				hashCode = (hashCode * 397) ^ (ProductMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductMark) : 0);
				hashCode = (hashCode * 397) ^ Kc.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk1.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk2.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk3.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk4.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk5.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk6.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk1.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk2.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk3.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk4.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk5.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk6.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm1.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm2.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm3.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm4.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm5.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm6.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb1.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb2.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb3.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb4.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb5.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb6.GetHashCode();
				return hashCode;
			}
		}
	}
}
