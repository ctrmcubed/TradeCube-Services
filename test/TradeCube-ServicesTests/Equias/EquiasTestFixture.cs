using Equias.Managers;
using Equias.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using System;
using System.Collections.Generic;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.Equias
{
    public class EquiasTestFixture
    {
        public EquiasAuthenticationService EquiasAuthenticationService { get; }
        public ITradeService TradeService { get; }
        public IEquiasService EquiasService { get; }
        public IEquiasManager EquiasManager { get; }

        public EquiasTestFixture()
        {
            var defaultHttpClientFactory = new DefaultHttpClientFactory();

            EquiasAuthenticationService = new EquiasAuthenticationService(defaultHttpClientFactory, new EquiasConfiguration(), new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));
            EquiasService = new EquiasService(defaultHttpClientFactory, new EquiasConfiguration(), new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));
            EquiasManager = new EquiasManager(EquiasAuthenticationService, EquiasService, CreateTradeService(Trades()), new EquiasMappingService());
        }

        public static ITradeService CreateTradeService(IEnumerable<TradeDataObject> trades)
        {
            var service = new Mock<ITradeService>();

            service
                .Setup(c => c.GetTradesAsync(It.IsAny<string>(), It.IsAny<TradeRequest>()))
                .ReturnsAsync((string jwtToken, TradeRequest _) =>
                    new ApiResponseWrapper<IEnumerable<TradeDataObject>> { Data = trades });

            return service.Object;
        }

        private static IEnumerable<TradeDataObject> Trades()
        {
            return new List<TradeDataObject>
            {
                new()
                {
                    TradeReference = "PR000001",
                    TradeLeg = 1,
                    TradeDateTime = new DateTime(2021, 01, 01, 12, 40, 51, 112),
                    TradeStatus = "Test"
                }
            };
        }
    }
}
