using System;

namespace WorkingStandards.Entities.Reports
{
    /// <summary>
    /// Запись отчета [Аннулируемые детали]
    /// </summary>
    public class CancelledDetail : IComparable<CancelledDetail>
    {
	    /// <summary>
	    /// Код детали
	    /// </summary>
		public decimal CodeDetail { get; set; }

	    /// <summary>
	    /// Наименование детали
	    /// </summary>
		public string NameDetail { get; set; }

	    /// <summary>
	    ///  Марка детали
	    /// </summary>
		public string OboznDetail { get; set; }

	    /// <summary>
	    /// Опреация
	    /// </summary>
		public decimal Operac { get; set; }

	    /// <summary>
	    /// Цех
	    /// </summary>
		public decimal WorkGuildId { get; set; }

	    /// <summary>
	    /// Участок
	    /// </summary>
		public decimal AreaId { get; set; }

	    /// <summary>
	    /// Код технической операции
	    /// </summary>
		public decimal TehnoperId { get; set; }

	    /// <summary>
	    /// Наименование технической операции
	    /// </summary>
		public string NameTehnoper { get; set; }
        public decimal Koefvr { get; set; }

	    /// <summary>
	    /// Код профессии
	    /// </summary>
		public decimal ProfId { get; set; }

	    /// <summary>
	    /// Наименование профессии
	    /// </summary>
		public string NameProf { get; set; }
        public decimal Kolrab { get; set; }

	    /// <summary>
	    /// Разряд
	    /// </summary>
		public decimal Razr { get; set; }

	    /// <summary>
	    /// Кол-во деталей
	    /// </summary>
		public decimal Koldet { get; set; }

	    /// <summary>
	    /// Ед. норм
	    /// </summary>
		public decimal Ednorm { get; set; }
        public decimal Tarset { get; set; }
        public decimal Vidnorm { get; set; }
        public decimal Razmpart { get; set; }

	    /// <summary>
	    /// ТПЗ
	    /// </summary>
		public decimal Tpz { get; set; }

	    /// <summary>
	    /// Время штуч.
	    /// </summary>
		public decimal Vst { get; set; }

	    /// <summary>
	    /// Коэф. неосн.
	    /// </summary>
		public decimal Koefneos { get; set; }

	    /// <summary>
	    /// Трудоёмкость
	    /// </summary>
		public decimal Vstk { get; set; }
        public decimal Rstk { get; set; }

	    /// <summary>
	    /// Номер извещения
	    /// </summary>
		public string Nomizv { get; set; }

	    /// <summary>
	    /// Дата извещения
	    /// </summary>
		public DateTime DateIzv { get; set; }


        public int CompareTo(CancelledDetail other)
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
            var codeDetailComparison = CodeDetail.CompareTo(other.CodeDetail);
            if (codeDetailComparison != 0)
            {
                return codeDetailComparison;
            }
            var nameDetailComparison = string.Compare(NameDetail, other.NameDetail, ordinalIgnoreCase);
            if (nameDetailComparison != 0)
            {
                return nameDetailComparison;
            }
            var oboznDetailComparison = string.Compare(OboznDetail, other.OboznDetail, ordinalIgnoreCase);
            if (oboznDetailComparison != 0)
            {
                return oboznDetailComparison;
            }
            var operacComparison = Operac.CompareTo(other.Operac);
            if (operacComparison != 0)
            {
                return operacComparison;
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
            var tehnoperIdComparison = TehnoperId.CompareTo(other.TehnoperId);
            if (tehnoperIdComparison != 0)
            {
                return tehnoperIdComparison;
            }
            var nameTehnoperComparison = string.Compare(NameTehnoper, other.NameTehnoper, ordinalIgnoreCase);
            if (nameTehnoperComparison != 0)
            {
                return nameTehnoperComparison;
            }
            var koefvrComparison = Koefvr.CompareTo(other.Koefvr);
            if (koefvrComparison != 0)
            {
                return koefvrComparison;
            }
            var profIdComparison = ProfId.CompareTo(other.ProfId);
            if (profIdComparison != 0)
            {
                return profIdComparison;
            }
            var nameProfComparison = string.Compare(NameProf, other.NameProf, ordinalIgnoreCase);
            if (nameProfComparison != 0)
            {
                return nameProfComparison;
            }
            var kolrabComparison = Kolrab.CompareTo(other.Kolrab);
            if (kolrabComparison != 0)
            {
                return kolrabComparison;
            }
            var razrComparison = Razr.CompareTo(other.Razr);
            if (razrComparison != 0)
            {
                return razrComparison;
            }
            var koldetComparison = Koldet.CompareTo(other.Koldet);
            if (koldetComparison != 0)
            {
                return koldetComparison;
            }
            var ednormComparison = Ednorm.CompareTo(other.Ednorm);
            if (ednormComparison != 0)
            {
                return ednormComparison;
            }
            var tarsetComparison = Tarset.CompareTo(other.Tarset);
            if (tarsetComparison != 0)
            {
                return tarsetComparison;
            }
            var vidnormComparison = Vidnorm.CompareTo(other.Vidnorm);
            if (vidnormComparison != 0)
            {
                return vidnormComparison;
            }
            var razmpartComparison = Razmpart.CompareTo(other.Razmpart);
            if (razmpartComparison != 0)
            {
                return razmpartComparison;
            }
            var tpzComparison = Tpz.CompareTo(other.Tpz);
            if (tpzComparison != 0)
            {
                return tpzComparison;
            }
            var vstComparison = Vst.CompareTo(other.Vst);
            if (vstComparison != 0)
            {
                return vstComparison;
            }
            var koefneosComparison = Koefneos.CompareTo(other.Koefneos);
            if (koefneosComparison != 0)
            {
                return koefneosComparison;
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
            var nomizvComparison = string.Compare(Nomizv, other.Nomizv, ordinalIgnoreCase);
            if (nomizvComparison != 0)
            {
                return nomizvComparison;
            }
            return DateIzv.CompareTo(other.DateIzv);
        }

        protected bool Equals(CancelledDetail other)
        {
            const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
            return CodeDetail == other.CodeDetail 
                   && string.Equals(NameDetail, other.NameDetail, ordinalIgnoreCase) 
                   && string.Equals(OboznDetail, other.OboznDetail, ordinalIgnoreCase) 
                   && Operac == other.Operac 
                   && WorkGuildId == other.WorkGuildId 
                   && AreaId == other.AreaId 
                   && TehnoperId == other.TehnoperId 
                   && string.Equals(NameTehnoper, other.NameTehnoper, ordinalIgnoreCase) 
                   && Koefvr == other.Koefvr 
                   && ProfId == other.ProfId 
                   && string.Equals(NameProf, other.NameProf, ordinalIgnoreCase) 
                   && Kolrab == other.Kolrab 
                   && Razr == other.Razr 
                   && Koldet == other.Koldet 
                   && Ednorm == other.Ednorm 
                   && Tarset == other.Tarset 
                   && Vidnorm == other.Vidnorm 
                   && Razmpart == other.Razmpart 
                   && Tpz == other.Tpz 
                   && Vst == other.Vst 
                   && Koefneos == other.Koefneos 
                   && Vstk == other.Vstk 
                   && Rstk == other.Rstk 
                   && string.Equals(Nomizv, other.Nomizv, ordinalIgnoreCase) 
                   && DateIzv.Equals(other.DateIzv);
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
            return Equals((CancelledDetail) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CodeDetail.GetHashCode();
                hashCode = (hashCode * 397) ^ (NameDetail != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(NameDetail) : 0);
                hashCode = (hashCode * 397) ^ (OboznDetail != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(OboznDetail) : 0);
                hashCode = (hashCode * 397) ^ Operac.GetHashCode();
                hashCode = (hashCode * 397) ^ WorkGuildId.GetHashCode();
                hashCode = (hashCode * 397) ^ AreaId.GetHashCode();
                hashCode = (hashCode * 397) ^ TehnoperId.GetHashCode();
                hashCode = (hashCode * 397) ^ (NameTehnoper != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(NameTehnoper) : 0);
                hashCode = (hashCode * 397) ^ Koefvr.GetHashCode();
                hashCode = (hashCode * 397) ^ ProfId.GetHashCode();
                hashCode = (hashCode * 397) ^ (NameProf != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(NameProf) : 0);
                hashCode = (hashCode * 397) ^ Kolrab.GetHashCode();
                hashCode = (hashCode * 397) ^ Razr.GetHashCode();
                hashCode = (hashCode * 397) ^ Koldet.GetHashCode();
                hashCode = (hashCode * 397) ^ Ednorm.GetHashCode();
                hashCode = (hashCode * 397) ^ Tarset.GetHashCode();
                hashCode = (hashCode * 397) ^ Vidnorm.GetHashCode();
                hashCode = (hashCode * 397) ^ Razmpart.GetHashCode();
                hashCode = (hashCode * 397) ^ Tpz.GetHashCode();
                hashCode = (hashCode * 397) ^ Vst.GetHashCode();
                hashCode = (hashCode * 397) ^ Koefneos.GetHashCode();
                hashCode = (hashCode * 397) ^ Vstk.GetHashCode();
                hashCode = (hashCode * 397) ^ Rstk.GetHashCode();
                hashCode = (hashCode * 397) ^ (Nomizv != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Nomizv) : 0);
                hashCode = (hashCode * 397) ^ DateIzv.GetHashCode();
                return hashCode;
            }
        }
    }
}

//SELECT anul67.detal, su73.name as nameDetal, su73.obozn, anul67.operac, anul67.ceh, anul67.uch, anul67.tehoper, tehnoper.naim as nameTehnoper, anul67.koefvr, anul67.prof, fsprof.name as nameProf,
//anul67.kolrab, anul67.razr, anul67.koldet, anul67.ednorm, anul67.tarset, anul67.vidnorm, anul67.razmpart, anul67.tpz, anul67.vst, anul67.koefneos, anul67.vstk, anul67.rstk, anul67.nomizv, anul67.dataizv FROM anul67
//LEFT JOIN "d:\VirtualMashinFiles\trudnorm\SERVER\V\FOXPRO\base\su73.dbf" on anul67.detal = su73.detal
//LEFT JOIN "d:\VirtualMashinFiles\trudnorm\SERVER\V\FOXPRO\base\fsprof.dbf" on anul67.prof = fsprof.prof
//LEFT JOIN "d:\VirtualMashinFiles\trudnorm\SERVER\V\FOXPRO\base\tehnoper.dbf" on anul67.tehoper = tehnoper.kod