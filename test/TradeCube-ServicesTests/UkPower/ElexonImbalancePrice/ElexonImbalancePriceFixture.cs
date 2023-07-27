﻿using System.Collections.Generic;
using System.Linq;
using Moq;
using Shared.DataObjects;
using Shared.Managers;
using Shared.Services;
using TradeCube_ServicesTests.Helpers;
using TradeCube_ServicesTests.Shared;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonImbalancePriceFixture
{
    public readonly IElexonImbalancePriceManager ElexonImbalancePriceManager;
    
    private readonly IList<ElexonImbalancePriceTestType> expectedResults;

    public ElexonImbalancePriceFixture()
    {
        expectedResults = FileHelper.ReadJsonFile<IList<ElexonImbalancePriceTestType>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonElexonImbalancePrice/expected_results_elexon_imbalance_price.json"));

        var elexonSystemData = FileHelper.ReadJsonFile<IList<ElexonImbalancePriceTestType>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonElexonImbalancePrice/mock_api_DERSYSDATA.json"));
        var elexonSettlementPeriodData = FileHelper.ReadJsonFile<IList<ElexonImbalancePriceTestType>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonElexonImbalancePrice/mock_api_ElexonSettlementPeriod.json"));
        
        var cubeDataObjects = FileHelper.ReadBsonFile<IList<CubeDataObject>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonElexonImbalancePrice/mock_cube.ejson"));
        var cubeTypeDataObjects = FileHelper.ReadBsonFile<IList<CubeTypeDataObject>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonElexonImbalancePrice/mock_cubetype.ejson"));
        var dataItemDataObjects = FileHelper.ReadBsonFile<IList<DataItemDataObject>>(TestHelper.GetTestDataFolder("TestData/UkPower/ElexonElexonImbalancePrice/mock_dataitem.ejson"));

        var cubeService = MockService.CreateCubeService(cubeDataObjects);
        var cubeTypeService = MockService.CreateCubeTypeService(cubeTypeDataObjects);
        var dataItemService = MockService.CreateDataItemService(dataItemDataObjects);
        
        ElexonImbalancePriceManager = new ElexonImbalancePriceManager(
            Mock.Of<IVaultService>(),
            Mock.Of<ISettingService>(),
            Mock.Of<IElexonService>(),
            cubeService,
            TestHelper.CreateNullLogger<ElexonImbalancePriceManager>());
    }
    
    public ElexonImbalancePriceTestType GetExpectedResult(string testDescription) => 
        expectedResults.SingleOrDefault(t => t.Description == testDescription);
}