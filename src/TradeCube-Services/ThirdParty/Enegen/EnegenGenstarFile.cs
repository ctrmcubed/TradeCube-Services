using System.Collections.Generic;
using TradeCube_Services.ThirdParty.Enegen.Extensions;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenGenstarFile
    {
        public EnegenGenstarHeader EnegenGenstarHeader { get; set; }
        public IEnumerable<EnegenGenstarDetail> EnegenGenstarDetails { get; set; }

        public IEnumerable<string> CreateFileStructure()
        {
            yield return EnegenGenstarHeader.ToDelimitedString();

            foreach (var enegenGenstarDetail in EnegenGenstarDetails)
            {
                yield return enegenGenstarDetail.ToDelimitedString();
            }
        }
    }
}