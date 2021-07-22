using System;
using System.Collections.Generic;
using System.Linq;
using Equias.Managers;
using Equias.Services;
using Fidectus.Managers;
using Fidectus.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using TradeCube_ServicesTests.Helpers;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.Fidectus
{
    public class FidectusTestFixture
    {
        public FidectusAuthenticationService FidectusAuthenticationService { get; }
        public IFidectusService FidectusService { get; }
        public IFidectusManager FidectusManager { get; }

        public FidectusTestFixture()
        {
            Configuration.SetEnvironmentVariables();

            var defaultHttpClientFactory = new DefaultHttpClientFactory();

            FidectusAuthenticationService = new FidectusAuthenticationService(defaultHttpClientFactory, new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));
            FidectusService = new FidectusService(defaultHttpClientFactory, new Logger<ApiService>(LoggerFactory.Create(l => l.AddConsole())));

            var vaultDataObjects = new List<VaultDataObject>
            {
                new()
                {
                    VaultKey = VaultConstants.FidectusClientId,
                    VaultValue = Environment.GetEnvironmentVariable("FIDECTUS_CLIENT_ID")
                },
                new()
                {
                    VaultKey = VaultConstants.FidectusClientSecret,
                    VaultValue = Environment.GetEnvironmentVariable("FIDECTUS_SECRET")
                }
            };

            FidectusManager = new FidectusManager(
                FidectusAuthenticationService,
                FidectusService,
                CreateSettingService(),
                CreateVaultService(vaultDataObjects), new Logger<FidectusManager>(LoggerFactory.Create(l => l.AddConsole())));
        }

        private static ISettingService CreateSettingService()
        {
            var service = new Mock<ISettingService>();

            service
                .Setup(c => c.GetSettingViaJwtAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string _, string _) =>
                    new ApiResponseWrapper<IEnumerable<SettingDataObject>> { Data = new List<SettingDataObject> { new() { SettingValue = "https://staging--fidectus.eu.auth0.com" } } });

            return service.Object;
        }

        private static IVaultService CreateVaultService(IEnumerable<VaultDataObject> data)
        {
            var service = new Mock<IVaultService>();

            service
                .Setup(c => c.GetVaultValueAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string vaultKey, string _) =>
                    new ApiResponseWrapper<IEnumerable<VaultDataObject>> { Data = data.Where(v => v.VaultKey == vaultKey) });

            return service.Object;
        }
    }
}
