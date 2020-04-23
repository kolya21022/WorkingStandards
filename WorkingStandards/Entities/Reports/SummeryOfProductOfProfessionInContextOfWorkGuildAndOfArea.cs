using System;

namespace WorkingStandards.Entities.Reports
{
	/// <summary>
	/// Запись отчета [Сводная по изделиям по профессиям в разрезе цехов, участков]
	/// </summary>
	public class SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea: IComparable<SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea>
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
		/// Код профессии
		/// </summary>
		public decimal ProfessionId { get; set; }

		/// <summary>
		/// Наименование профессии
		/// </summary>
		public string ProfessionName { get; set; }

		/// <summary>
		/// Цех
		/// </summary>
		public decimal Kc { get; set; }

		/// <summary>
		/// Участок 
		/// </summary>
		public decimal Uch { get; set; }

		/// <summary>
		/// Трудоёмкость 
		/// </summary>
		public decimal Vstk { get; set; }

		public decimal Rstk { get; set; }

		public decimal Prtnorm { get; set; }

		public decimal Nadb { get; set; }


		public int CompareTo(SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea other)
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
			var professionIdComparison = ProfessionId.CompareTo(other.ProfessionId);
			if (professionIdComparison != 0)
			{
				return professionIdComparison;
			}
			var professionNameComparison = string.Compare(ProfessionName, other.ProfessionName, ordinalIgnoreCase);
			if (professionNameComparison != 0)
			{
				return professionNameComparison;
			}
			var kcComparison = Kc.CompareTo(other.Kc);
			if (kcComparison != 0)
			{
				return kcComparison;
			}
			var uchComparison = Uch.CompareTo(other.Uch);
			if (uchComparison != 0)
			{
				return uchComparison;
			}
			var vstkComparison = Vstk.CompareTo(other.Vstk);
			if (vstkComparison != 0)
			{
				return vstkComparison;
			}
			var rstkComparison = Rstk.CompareTo(other.Rstk);
			if (rstkComparison != 0)
			{
				return rstkComparison;
			}
			var prtnormComparison = Prtnorm.CompareTo(other.Prtnorm);
			if (prtnormComparison != 0)
			{
				return prtnormComparison;
			}
			return Nadb.CompareTo(other.Nadb);
		}

		protected bool Equals(SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea other)
		{
			const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
			return ProductId == other.ProductId 
			       && string.Equals(ProductName, other.ProductName, ordinalIgnoreCase) 
			       && string.Equals(ProductMark, other.ProductMark, ordinalIgnoreCase) 
			       && ProfessionId == other.ProfessionId 
			       && string.Equals(ProfessionName, other.ProfessionName, ordinalIgnoreCase) 
			       && Kc == other.Kc 
			       && Uch == other.Uch 
			       && Vstk == other.Vstk 
			       && Rstk == other.Rstk 
			       && Prtnorm == other.Prtnorm 
			       && Nadb == other.Nadb;
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
			return Equals((SummeryOfProductOfProfessionInContextOfWorkGuildAndOfArea) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = ProductId.GetHashCode();
				hashCode = (hashCode * 397) ^ (ProductName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductName) : 0);
				hashCode = (hashCode * 397) ^ (ProductMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductMark) : 0);
				hashCode = (hashCode * 397) ^ ProfessionId.GetHashCode();
				hashCode = (hashCode * 397) ^ (ProfessionName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProfessionName) : 0);
				hashCode = (hashCode * 397) ^ Kc.GetHashCode();
				hashCode = (hashCode * 397) ^ Uch.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb.GetHashCode();
				return hashCode;
			}
		}
	}
}
