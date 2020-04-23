using System;

namespace WorkingStandards.Entities.Reports
{
    /// <summary>
    /// Запись отчета [Просмотр трудоемкости и зарплаты на сбор.единицы по цехам]
    /// </summary>
    public class ComplexityAndSalaryOnUnitByWorkGuild : IComparable<ComplexityAndSalaryOnUnitByWorkGuild>
    {
        /// <summary>
        /// Для общего вывода в отчете
        /// </summary>
        public decimal Group => 1;

        /// <summary>
        /// Код детали
        /// </summary>
        public decimal DetailId { get; set; }

        /// <summary>
        /// Наименование детали
        /// </summary>
        public string DetailName { get; set; }

        /// <summary>
        /// Обозначение детали
        /// </summary>
        public string DetailMark { get; set; }

        /// <summary>
        /// Цех
        /// </summary>
        public decimal WorkGuildId { get; set; }

        /// <summary>
        /// Участок
        /// </summary>
        public decimal AreaId { get; set; }

	    /// <summary>
	    /// Трудоёмкость
	    /// </summary>
		public decimal Vstk { get; set; }
        public decimal Rstk { get; set; }
        public decimal Nadb { get; set; }
        public decimal Prtnorm { get; set; }

        protected bool Equals(ComplexityAndSalaryOnUnitByWorkGuild other)
        {
            const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
            return DetailId == other.DetailId 
                   && string.Equals(DetailName, other.DetailName, ordinalIgnoreCase) 
                   && string.Equals(DetailMark, other.DetailMark, ordinalIgnoreCase) 
                   && WorkGuildId == other.WorkGuildId 
                   && AreaId == other.AreaId 
                   && Vstk == other.Vstk 
                   && Rstk == other.Rstk 
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
            return Equals((ComplexityAndSalaryOnUnitByWorkGuild) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DetailId.GetHashCode();
                hashCode = (hashCode * 397) ^ (DetailName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(DetailName) : 0);
                hashCode = (hashCode * 397) ^ (DetailMark != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(DetailMark) : 0);
                hashCode = (hashCode * 397) ^ WorkGuildId.GetHashCode();
                hashCode = (hashCode * 397) ^ AreaId.GetHashCode();
                hashCode = (hashCode * 397) ^ Vstk.GetHashCode();
                hashCode = (hashCode * 397) ^ Rstk.GetHashCode();
                hashCode = (hashCode * 397) ^ Nadb.GetHashCode();
                hashCode = (hashCode * 397) ^ Prtnorm.GetHashCode();
                return hashCode;
            }
        }

        public int CompareTo(ComplexityAndSalaryOnUnitByWorkGuild other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }
            var workGuildIdComparison = WorkGuildId.CompareTo(other.WorkGuildId);
            if (workGuildIdComparison != 0)
            {
                return workGuildIdComparison;
            }
            var areaIdComparison = AreaId.CompareTo(other.AreaId);
            if (areaIdComparison != 0)
            {
                return areaIdComparison;
            }
            var detalIdComparison = DetailId.CompareTo(other.DetailId);
            if (detalIdComparison != 0)
            {
                return detalIdComparison;
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
            var nadbComparison = Nadb.CompareTo(other.Nadb);
            if (nadbComparison != 0)
            {
                return nadbComparison;
            }
            return Prtnorm.CompareTo(other.Prtnorm);
        }
    }
}
