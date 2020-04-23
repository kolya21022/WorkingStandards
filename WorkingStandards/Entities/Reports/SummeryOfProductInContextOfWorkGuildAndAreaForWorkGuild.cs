using System;

namespace WorkingStandards.Entities.Reports
{
	/// <summary>
	/// Запись отчета [Сводная по изделиям в разрезе цехов, участков (для цехов)]
	/// </summary>
	public class SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild 
	    : IComparable<SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild>
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
		/// Обозначение изделия
		/// </summary>
		public string ProductMark { get; set; }

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
		public decimal Premper { get; set; }
		public decimal Nadb { get; set; }
		public decimal Prtnorm { get; set; }

		public int CompareTo(SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild other)
		{
			const StringComparison ordinalIgnorCase = StringComparison.OrdinalIgnoreCase;
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
			var productNameComparison = string.Compare(ProductName, other.ProductName, ordinalIgnorCase);
			if (productNameComparison != 0)
			{
				return productNameComparison;
			}
			var productMarkComparison = string.Compare(ProductMark, other.ProductMark, ordinalIgnorCase);
			if (productMarkComparison != 0)
			{
				return productMarkComparison;
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
			var premperComparison = Premper.CompareTo(other.Premper);
			if (premperComparison != 0)
			{
				return premperComparison;
			}
			var nadbComparison = Nadb.CompareTo(other.Nadb);
			if (nadbComparison != 0)
			{
				return nadbComparison;
			}
			return Prtnorm.CompareTo(other.Prtnorm);
		}

		protected bool Equals(SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild other)
		{
			const StringComparison ordinalIgnorCase = StringComparison.OrdinalIgnoreCase;
			return ProductId == other.ProductId 
			       && string.Equals(ProductName, other.ProductName, ordinalIgnorCase) 
			       && string.Equals(ProductMark, other.ProductMark, ordinalIgnorCase) 
			       && Kc == other.Kc 
			       && Uch == other.Uch 
			       && Vstk == other.Vstk 
			       && Rstk == other.Rstk 
			       && Premper == other.Premper 
			       && Nadb == other.Nadb 
			       && Prtnorm == other.Prtnorm;
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
			return Equals((SummeryOfProductInContextOfWorkGuildAndAreaForWorkGuild) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = ProductId.GetHashCode();
				hashCode = (hashCode * 397) ^ (ProductName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductName) : 0);
				hashCode = (hashCode * 397) ^ (ProductMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ProductMark) : 0);
				hashCode = (hashCode * 397) ^ Kc.GetHashCode();
				hashCode = (hashCode * 397) ^ Uch.GetHashCode();
				hashCode = (hashCode * 397) ^ Vstk.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk.GetHashCode();
				hashCode = (hashCode * 397) ^ Premper.GetHashCode();
				hashCode = (hashCode * 397) ^ Nadb.GetHashCode();
				hashCode = (hashCode * 397) ^ Prtnorm.GetHashCode();
				return hashCode;
			}
		}
	}
}
