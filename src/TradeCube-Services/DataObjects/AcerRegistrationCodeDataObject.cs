namespace TradeCube_Services.DataObjects
{
    public class AcerRegistrationCodeDataObject
    {
        // ReSharper disable once InconsistentNaming
        public string ACERCode { get; set; }

        // ReSharper disable once InconsistentNaming
        public string ACERLongName { get; set; }

        public AddressType Address { get; set; }

        // ReSharper disable once InconsistentNaming
        public string EIC { get; set; }

        // ReSharper disable once InconsistentNaming
        public string BIC { get; set; }

        // ReSharper disable once InconsistentNaming
        public string LEI { get; set; }

        public string Website { get; set; }
        public string TransparencyWebsite { get; set; }
    }
}