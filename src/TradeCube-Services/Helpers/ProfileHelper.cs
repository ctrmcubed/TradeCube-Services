using System;
using System.Collections.Generic;
using System.Linq;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Exceptions;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Helpers
{
    public static class ProfileHelper
    {
        public static IEnumerable<TradeProfile> Combine(IEnumerable<TradeProfileResponse> profileResponses, ILookup<(string TradeReference, int TradeLeg), TradeDataObject> lookup)
        {
            static DateTime LocalDateTime(DateTime dt, TradeProfileResponse tpr, ILookup<(string TradeReference, int TradeLeg), TradeDataObject> lookup)
            {
                var trade = lookup[(tpr.TradeReference, tpr.TradeLeg)].SingleOrDefault();
                if (trade == null)
                {
                    throw new TradeProfileException($"Trade '{tpr.TradeReference},{tpr.TradeLeg}' not found'");
                }

                var timezone = trade.Product?.Commodity?.Timezone;
                if (timezone == null)
                {
                    throw new TradeProfileException("Trade timezone not set");
                }

                return DateTimeHelper.GetLocalDateTime(dt, timezone);
            }

            var periodCounter = new PeriodCounter();

            foreach (var profileResponse in profileResponses)
            {
                var volumeProfile = profileResponse.VolumeProfile.ToList();
                var priceProfile = profileResponse.PriceProfile.ToList();

                if (volumeProfile.Count != priceProfile.Count)
                {
                    throw new TradeProfileException("Volume/Price number mismatch");
                }

                for (var vp = 0; vp < volumeProfile.Count; vp++)
                {
                    var volume = volumeProfile[vp];
                    var price = priceProfile[vp];

                    var localDateTime = LocalDateTime(volume.UtcStartDateTime, profileResponse, lookup);

                    yield return volume.UtcStartDateTime == price.UtcStartDateTime
                        ? new TradeProfile
                        {
                            TradeReference = profileResponse.TradeReference,
                            TradeLeg = profileResponse.TradeLeg,
                            Utc = volume.UtcStartDateTime,
                            Local = localDateTime,
                            Volume = volume.Value,
                            Price = price.Value,
                            PeriodNumber = periodCounter.Count(localDateTime)
                        }
                        : throw new TradeProfileException("Volume/Price date/time mismatch");
                }
            }
        }
    }
}
