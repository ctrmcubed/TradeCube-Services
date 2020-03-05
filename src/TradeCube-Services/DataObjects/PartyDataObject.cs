namespace TradeCube_Services.DataObjects
{
    public class PartyDataObject
    {
        public string Party { get; set; }

        public string PartyLongName { get; set; }

        public bool Internal { get; set; }

        public ContactDataObject PrimaryConfirmationContact { get; set; }
    }
}