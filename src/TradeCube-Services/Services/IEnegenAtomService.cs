﻿using System.Threading.Tasks;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;

namespace TradeCube_Services.Services
{
    public interface IEnegenAtomService
    {
        Task<ApiResponseWrapper<WebServiceResponse>> Trade(EnegenAtomTradeParameters enegenAtomTradeParameters);
    }
}