namespace TradeCube_Services.DataObjects
{
    public class EnergyIdentificationCodeDataObject
    {
        // ReSharper disable once InconsistentNaming
        public string EIC { get; set; }

        // ReSharper disable once InconsistentNaming
        public string EICLongName { get; set; }

        // ReSharper disable once InconsistentNaming
        public string EICType { get; set; }

        public string DisplayName { get; set; }

        // ReSharper disable once InconsistentNaming
        public string VATCode { get; set; }

        // ReSharper disable once InconsistentNaming
        public string EICParent { get; set; }

        // ReSharper disable once InconsistentNaming
        public string EICResponsible { get; set; }

        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Function { get; set; }
    }
}