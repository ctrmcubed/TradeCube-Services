//using Equias.Messages;
//using Equias.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Shared.Constants;
//using Shared.Messages;
//using Shared.Services;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Equias.Controllers
//{
//    [ApiVersion("1.0")]
//    [Produces("application/json")]
//    [ApiController]
//    [Route("[controller]")]
//    [Route("v{version:apiVersion}/[controller]")]
//    public class EboController : Controller
//    {
//        private readonly IEquiasAuthenticationService equiasAuthenticationService;
//        private readonly IEquiasService equiasService;
//        private readonly IVaultService vaultService;
//        private readonly ILogger<EboController> logger;

//        public EboController(IEquiasAuthenticationService equiasAuthenticationService, IEquiasService equiasService, IVaultService vaultService, ILogger<EboController> logger)
//        {
//            this.equiasAuthenticationService = equiasAuthenticationService;
//            this.equiasService = equiasService;
//            this.vaultService = vaultService;
//            this.logger = logger;
//        }

//        [HttpPost("TradeStatus")]
//        public async Task<IActionResult> TradeStatus([FromHeader] string apiJwtToken, [FromBody] EboGetTradeStatusRequest eboGetTradeStatusRequest)
//        {
//            try
//            {
//                var eboUsername = (await vaultService.GetVaultValueAsync(VaultConstants.EquiasEboUsernameKey, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;
//                var eboPassword = (await vaultService.GetVaultValueAsync(VaultConstants.EquiasEboPasswordKey, apiJwtToken))?.Data?.SingleOrDefault()?.VaultValue;
//                var requestTokenRequest = new RequestTokenRequest(eboUsername, eboPassword);
//                var requestTokenResponse = await equiasAuthenticationService.GetAuthenticationToken(requestTokenRequest);
//                var eboGetTradeStatusResponse = await equiasService.EboGetTradeStatus(eboGetTradeStatusRequest.TradeIds, requestTokenResponse);

//                return Ok(eboGetTradeStatusResponse);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, ex.Message);
//                return BadRequest(new ApiResponseWrapper<WebServiceResponse> { Message = ex.Message, Status = ApiConstants.FailedResult });
//            }
//        }
//    }
//}