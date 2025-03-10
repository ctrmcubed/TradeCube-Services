﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DataObjects;

namespace TradeCube_Services.Services
{
    public class CountryLookupService : ICountryLookupService
    {
        private readonly ICountryService countryService;
        private Dictionary<string, CountryDataObject> countryDictionary;

        public CountryLookupService(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        public async Task LoadAsync(string apiJwtToken)
        {
            var countries = await countryService.CountriesAsync(apiJwtToken);
            countryDictionary = countries.Data.ToDictionary(k => k.Country, v => v);
        }

        public CountryDataObject Lookup(string key)
        {
            return countryDictionary.ContainsKey(key)
                ? countryDictionary[key]
                : null;
        }
    }
}