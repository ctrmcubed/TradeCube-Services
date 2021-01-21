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
        public IEquiasService EquiasService { get; }
        public IEquiasManager EquiasManager { get; }

        public EquiasTestFixture()
        {
            var defaultHttpClientFactory = new DefaultHttpClientFactory();

            EquiasAuthenticationService = new EquiasAuthenticationService(defaultHttpClientFactory, new EquiasConfiguration(), new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));
            EquiasService = new EquiasService(defaultHttpClientFactory, new EquiasConfiguration(), new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));

            EquiasManager = new EquiasManager(EquiasAuthenticationService, EquiasService, CreateTradeService(Trades()),
                new EquiasMappingService(CreateMappingService(Mappings())), CreateTradeSummaryService(), CreateCashflowService(), CreateProfileService());
        }

        private static ITradeService CreateTradeService(IEnumerable<TradeDataObject> trades)
        {
            var service = new Mock<ITradeService>();

            service
                .Setup(c => c.GetTradeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string _, string _, int _) =>
                    new ApiResponseWrapper<IEnumerable<TradeDataObject>> { Data = trades });

            return service.Object;
        }

        private static IMappingService CreateMappingService(IEnumerable<MappingDataObject> mappings)
        {
            var service = new Mock<IMappingService>();

            service
                .Setup(c => c.GetMappingsAsync(It.IsAny<string>()))
                .ReturnsAsync((string _) =>
                    new ApiResponseWrapper<IEnumerable<MappingDataObject>> { Data = mappings });

            return service.Object;
        }

        private static ITradeSummaryService CreateTradeSummaryService()
        {
            var service = new Mock<ITradeSummaryService>();

            service
                .Setup(c => c.TradeSummaryAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((string _, int _, string _) =>
                    new ApiResponseWrapper<IEnumerable<TradeSummaryResponse>>
                    {
                        Data = new List<TradeSummaryResponse> { new TradeSummaryResponse { TotalVolume = 5000 } }
                    });

            return service.Object;
        }

        private static ICashflowService CreateCashflowService()
        {
            var service = new Mock<ICashflowService>();

            service
                .Setup(c => c.CashflowAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((string _, int _, string _) =>
                    new ApiResponseWrapper<IEnumerable<CashflowType>>
                    {
                        Data = new List<CashflowType>
                        {
                            new CashflowType {SettlementDate = new DateTime(2021, 1, 21, 15, 0, 0)},
                            new CashflowType {SettlementDate = new DateTime(2021, 2, 21, 15, 0, 0)}
                        }
                    });

            return service.Object;
        }

        private static IProfileService CreateProfileService()
        {
            var service = new Mock<IProfileService>();

            service
                .Setup(c => c.ProfileAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string _, int _, string _) =>
                    new ApiResponseWrapper<IEnumerable<ProfileResponse>>
                    {
                        Data = new List<ProfileResponse>
                        {
                            new ProfileResponse{} ,
                            new ProfileResponse {}
                        }
                    });

            return service.Object;
        }

        private static IEnumerable<TradeDataObject> Trades()
        {
            return new List<TradeDataObject>
            {
                new()
                {
                    TradeReference = "ET000040",
                    TradeLeg = 1,
                    TradeDateTime = new DateTime(2020, 05, 12, 10, 07, 23, 538),
                    TradeStatus = "Test",
                    Contract = new ContractDataObject
                    {
                        AgreementType = new AgreementTypeDataObject
                        {
                            AgreementType = "Other"
                        }
                    },
                    Product = new ProductDataObject
                    {
                        Commodity = new CommodityDataObject
                        {
                            Commodity = "UK Gas",
                            Country = "GB",
                            DeliveryArea = new DeliveryAreaDataObject
                            {
                                Eic = "55Y11NG-NBP--11D"
                            }
                        },
                        ContractType = "Forward",
                        ShapeDescription = "Baseload"
                    },
                    Buyer = new PartyDataObject
                    {
                        Eic = new EnergyIdentificationCodeDataObject
                        {
                            Eic = "Dummy"
                        }
                    },
                    Seller = new PartyDataObject
                    {
                        Eic = new EnergyIdentificationCodeDataObject
                        {
                            Eic = "Dummy"
                        }
                    },
                    Quantity = new QuantityDataObject
                    {
                        QuantityUnit = new QuantityUnitDataObject
                        {
                            QuantityUnit = "Therms/day (UK)",
                            EnergyUnit = new EnergyUnitDataObject
                            {
                                EnergyUnit = "Therm (UK)"
                            }
                        }
                    },
                    Price = new PriceDataObject
                    {
                        PriceUnit = new PriceUnitDataObject
                        {
                            Currency = "GBP",
                            CurrencyExponent = 2,
                            PerEnergyUnit = new EnergyUnitDataObject
                            {
                                EnergyUnit = "Therm (UK)"
                            }
                        }
                    },
                    InternalTrader = new ContactDataObject
                    {
                        Contact = "DEMO_JVT27722_BRJI"
                    }
                }
            };
        }

        private static IEnumerable<MappingDataObject> Mappings()
        {
            return new List<MappingDataObject>
            {
                new()
                {

                }
            };
        }
    }
}
