using System;


namespace WorkingStandards.Entities.Reports
{
	/// <summary>
	/// Запись отчета [Печать по изделиям в разрезе детале-операций]
	/// </summary>
	public class PrintingOfProsuctInContextOfDetalOperations: IComparable<PrintingOfProsuctInContextOfDetalOperations>
	{
		/// <summary>
		/// Код изделия
		/// </summary>
		public decimal ProductId { get; set; }

		/// <summary>
		/// YНаименование изделия
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
		/// Операция
		/// </summary>
		public decimal Operac { get; set; }

		/// <summary>
		/// Код технической операции
		/// </summary>
		public decimal Tehoper { get; set; }

		/// <summary>
		/// Наименование технической операции
		/// </summary>
		public string OperationName { get; set; }

		/// <summary>
		/// Трудоёмкость
		/// </summary>
		public decimal Vstk { get; set; }

		public decimal Rstk { get; set; }

		/// <summary>
		/// Выпуск 
		/// </summary>
		public decimal Vypusk { get; set; }

		public int CompareTo(PrintingOfProsuctInContextOfDetalOperations other)
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
			var operacComparison = Operac.CompareTo(other.Operac);
			if (operacComparison != 0)
			{
				return operacComparison;
			}
			var tehoperComparison = Tehoper.CompareTo(other.Tehoper);
			if (tehoperComparison != 0)
			{
				return tehoperComparison;
			}
			var operationNameComparison = string.Compare(OperationName, other.OperationName, ordinalIgnoreCase);
			if (operationNameComparison != 0)
			{
				return operationNameComparison;
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
			return Vypusk.CompareTo(other.Vypusk);
		}

		protected bool Equals(PrintingOfProsuctInContextOfDetalOperations other)
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
			       && Operac == other.Operac 
			       && Tehoper == other.Tehoper 
			       && string.Equals(OperationName, other.OperationName, ordinalIgnoreCase) 
			       && Vstk == other.Vstk 
			       && Rstk == other.Rstk 
			       && Vypusk == other.Vypusk;
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
			return Equals((PrintingOfProsuctInContextOfDetalOperations) obj);
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
				hashCode = (hashCode * 397) ^ Operac.GetHashCode();
				hashCode = (hashCode * 397) ^ Tehoper.GetHashCode();
				hashCode = (hashCode * 397) ^ (OperationName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(OperationName) : 0);
				hashCode = (hashCode * 397) ^ Vstk.GetHashCode();
				hashCode = (hashCode * 397) ^ Rstk.GetHashCode();
				hashCode = (hashCode * 397) ^ Vypusk.GetHashCode();
				return hashCode;
			}
		}
	}
}
