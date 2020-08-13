using MoreLinq.Extensions;
using System.Collections.Generic;

namespace TradeCube_Services.ThirdParty.Enegen.Extensions
{
    public static class EnegenGenstarExtensions
    {
        public static string ToDelimitedString(this EnegenGenstarHeader enegenGenstarHeader)
        {
            var line = new List<string>
            {
                enegenGenstarHeader.ContractName,
                enegenGenstarHeader.ContractDescription,
                enegenGenstarHeader.Trader,
                enegenGenstarHeader.TraderProductionConsumptionFlag,
                enegenGenstarHeader.Party2,
                enegenGenstarHeader.Party2ProductionConsumptionFlag,
                enegenGenstarHeader.ContractStartDate,
                enegenGenstarHeader.ContractEndDate,
                enegenGenstarHeader.Type,
                enegenGenstarHeader.ContractGroupIdentifier,
                enegenGenstarHeader.Profile,
                enegenGenstarHeader.Evergreen
            };

            return line.ToDelimitedString(",");
        }

        public static string ToDelimitedString(this EnegenGenstarDetail enegenGenstarDetail)
        {
            var line = new List<string>
            {
                enegenGenstarDetail.Date,
                enegenGenstarDetail.Period,
                enegenGenstarDetail.Contractedvolume
            };

            return line.ToDelimitedString(",");
        }
    }
}
