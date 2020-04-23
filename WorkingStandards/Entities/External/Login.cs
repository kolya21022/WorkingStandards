namespace WorkingStandards.Entities.External
{
    public class Login
    {
        #region Public Fields
        public decimal Workguild { get; set; }
        public decimal Lvl { get; set; }
        #endregion

        public string Display
        {
            get
            {
                if (Workguild == 99)
                {
                    return "ОТИЗ";
                }

                return "Цех: " + Workguild;
            }
        }

        public Login(decimal workguild, decimal lvl)
        {
            Workguild = workguild;
            Lvl = lvl;

        }
    }
}
