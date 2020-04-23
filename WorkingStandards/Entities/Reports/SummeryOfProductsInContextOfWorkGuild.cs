using System;

namespace WorkingStandards.Entities.Reports
{
	/// <summary>
	/// Запись отчета [Сводная по изделиям в разрезе цехов]
	/// </summary>
	public class SummeryOfProductsInContextOfWorkGuild : IComparable<SummeryOfProductsInContextOfWorkGuild>
	{
		/// <summary>
		/// Доп. параметр для облегчения подсчета вывода в самом отчете
		/// </summary>
		public int Group => 1;

		/// <summary>
		/// Код изделия
		/// </summary>
		public decimal ProductId { get; set; }

		/// <summary>
		/// Марка изделия
		/// </summary>
		public string ProductMark { get; set; }

		/// <summary>
		/// Наименование изделия
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Трёдоёмкость по определённым цехам и всему заводу
		/// </summary>
		public decimal Vstk2 { get; set; }
		public decimal Vstk3 { get; set; }
		public decimal Vstk4 { get; set; }
		public decimal Vstk5 { get; set; }
		public decimal Vstk21 { get; set; }
		public decimal VstkZavod { get; set; }


		public decimal Rstk2 { get; set; }
		public decimal Rstk3 { get; set; }
		public decimal Rstk4 { get; set; }
		public decimal Rstk5 { get; set; }
		public decimal Rstk21 { get; set; }
		public decimal RstkZavod { get; set; }


		public decimal Prtnorm2 { get; set; }
		public decimal Prtnorm3 { get; set; }
		public decimal Prtnorm4 { get; set; }
		public decimal Prtnorm5 { get; set; }
		public decimal Prtnorm21 { get; set; }
		public decimal PrtnormZavod { get; set; }


		public decimal Nadb2 { get; set; }
		public decimal Nadb3 { get; set; }
		public decimal Nadb4 { get; set; }
		public decimal Nadb5 { get; set; }
		public decimal Nadb21 { get; set; }
		public decimal NadbZavod { get; set; }


		public int CompareTo(SummeryOfProductsInContextOfWorkGuild other)
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
			var productMarkComparison = string.Compare(ProductMark, other.ProductMark, ordinalIgnoreCase);
			if (productMarkComparison != 0)
			{
				return productMarkComparison;
			}
			var productNameComparison = string.Compare(ProductName, other.ProductName, ordinalIgnoreCase);
			if (productNameComparison != 0)
			{
				return productNameComparison;
			}
			var vstk2Comparison = Vstk2.CompareTo(other.Vstk2);
			if (vstk2Comparison != 0)
			{
				return vstk2Comparison;
			}
			var rstk2Comparison = Rstk2.CompareTo(other.Rstk2);
			if (rstk2Comparison != 0)
			{
				return rstk2Comparison;
			}
			var prtnorm2Comparison = Prtnorm2.CompareTo(other.Prtnorm2);
			if (prtnorm2Comparison != 0)
			{
				return prtnorm2Comparison;
			}
			var nadb2Comparison = Nadb2.CompareTo(other.Nadb2);
			if (nadb2Comparison != 0)
			{
				return nadb2Comparison;
			}
			var vstk3Comparison = Vstk3.CompareTo(other.Vstk3);
			if (vstk3Comparison != 0)
			{
				return vstk3Comparison;
			}
			var rstk3Comparison = Rstk3.CompareTo(other.Rstk3);
			if (rstk3Comparison != 0)
			{
				return rstk3Comparison;
			}
			var prtnorm3Comparison = Prtnorm3.CompareTo(other.Prtnorm3);
			if (prtnorm3Comparison != 0)
			{
				return prtnorm3Comparison;
			}
			var nadb3Comparison = Nadb3.CompareTo(other.Nadb3);
			if (nadb3Comparison != 0)
			{
				return nadb3Comparison;
			}
			var vstk4Comparison = Vstk4.CompareTo(other.Vstk4);
			if (vstk4Comparison != 0)
			{
				return vstk4Comparison;
			}
			var rstk4Comparison = Rstk4.CompareTo(other.Rstk4);
			if (rstk4Comparison != 0)
			{
				return rstk4Comparison;
			}
			var prtnorm4Comparison = Prtnorm4.CompareTo(other.Prtnorm4);
			if (prtnorm4Comparison != 0)
			{
				return prtnorm4Comparison;
			}
			var nadb4Comparison = Nadb4.CompareTo(other.Nadb4);
			if (nadb4Comparison != 0)
			{
				return nadb4Comparison;
			}
			var vstk5Comparison = Vstk5.CompareTo(other.Vstk5);
			if (vstk5Comparison != 0)
			{
				return vstk5Comparison;
			}
			var rstk5Comparison = Rstk5.CompareTo(other.Rstk5);
			if (rstk5Comparison != 0)
			{
				return rstk5Comparison;
			}
			var prtnorm5Comparison = Prtnorm5.CompareTo(other.Prtnorm5);
			if (prtnorm5Comparison != 0)
			{
				return prtnorm5Comparison;
			}
			var nadb5Comparison = Nadb5.CompareTo(other.Nadb5);
			if (nadb5Comparison != 0)
			{
				return nadb5Comparison;
			}
			var vstk21Comparison = Vstk21.CompareTo(other.Vstk21);
			if (vstk21Comparison != 0)
			{
				return vstk21Comparison;
			}
			var rstk21Comparison = Rstk21.CompareTo(other.Rstk21);
			if (rstk21Comparison != 0)
			{
				return rstk21Comparison;
			}
			var prtnorm21Comparison = Prtnorm21.CompareTo(other.Prtnorm21);
			if (prtnorm21Comparison != 0)
			{
				return prtnorm21Comparison;
			}
			var nadb21Comparison = Nadb21.CompareTo(other.Nadb21);
			if (nadb21Comparison != 0)
			{
				return nadb21Comparison;
			}
			var vstkZavodComparison = VstkZavod.CompareTo(other.VstkZavod);
			if (vstkZavodComparison != 0)
			{
				return vstkZavodComparison;
			}
			var rstkZavodComparison = RstkZavod.CompareTo(other.RstkZavod);
			if (rstkZavodComparison != 0)
			{
				return rstkZavodComparison;
			}
			var prtnormZavodComparison = PrtnormZavod.CompareTo(other.PrtnormZavod);
			if (prtnormZavodComparison != 0)
			{
				return prtnormZavodComparison;
			}
			return NadbZavod.CompareTo(other.NadbZavod);
		}

		protected bool Equals(SummeryOfProductsInContextOfWorkGuild other)
		{
			const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
			return ProductId == other.ProductId 
			       && string.Equals(ProductMark, other.ProductMark, ordinalIgnoreCase) 
			       && string.Equals(ProductName, other.ProductName, ordinalIgnoreCase) 
			       && Vstk2 == other.Vstk2 
			       && Rstk2 == other.Rstk2 
			       && Prtnorm2 == other.Prtnorm2 
			       && Nadb2 == other.Nadb2 
			       && Vstk3 == other.Vstk3 
			       && Rstk3 == other.Rstk3 
			       && Prtnorm3 == other.Prtnorm3 
			       && Nadb3 == other.Nadb3 
			       && Vstk4 == other.Vstk4 
			       && Rstk4 == other.Rstk4 
			       && Prtnorm4 == other.Prtnorm4 
			       && Nadb4 == other.Nadb4 
			       && Vstk5 == other.Vstk5 
			       && Rstk5 == other.Rstk5 
			       && Prtnorm5 == other.Prtnorm5 
			       && Nadb5 == other.Nadb5 
			       && Vstk21 == other.Vstk21 
			       && Rstk21 == other.Rstk21 
			       && Prtnorm21 == other.Prtnorm21 
			       && Nadb21 == other.Nadb21 
			       && VstkZavod == other.VstkZavod 
			       && RstkZavod == other.RstkZavod 
			       && PrtnormZavod == other.PrtnormZavod 
			       && NadbZavod == other.NadbZavod;
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
			return Equals((SummeryOfProductsInContextOfWorkGuild) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = ProductId.GetHashCode();
				hashCode = (hashCode * 397) ^ (ProductMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductMark) : 0);
				hashCode = (hashCode * 397) ^ (ProductName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductName) : 0);
				hashCode = (hashCode * 397) ^ Vstk2.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk2.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm2.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb2.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk3.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk3.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm3.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb3.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk4.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk4.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm4.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb4.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk5.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk5.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm5.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb5.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk21.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk21.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm21.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb21.GetHashCode();
				hashCode = (hashCode * 397) ^ VstkZavod.GetHashCode();
				hashCode = (hashCode * 397) ^ RstkZavod.GetHashCode();
				hashCode = (hashCode * 397) ^ PrtnormZavod.GetHashCode();
				hashCode = (hashCode * 397) ^ NadbZavod.GetHashCode();
				return hashCode;
			}
		}
	}
}
