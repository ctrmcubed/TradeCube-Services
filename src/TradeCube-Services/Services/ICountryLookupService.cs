﻿using System.Threading.Tasks;
using TradeCube_Services.DataObjects;

namespace TradeCube_Services.Services
{
    public interface ICountryLookupService
    {
        Task LoadAsync(string apiJwtToken);
        CountryDataObject Lookup(string key);
    }
}