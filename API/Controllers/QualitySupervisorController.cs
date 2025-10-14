using Application.Productions;
using Application.Sales;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class QualitySupervisorController : BaseApiController
    {
        [HttpPost("registerTyreSale")]
        public async Task<IActionResult> RegisterTyreSale([FromForm]RegisterTyreSaleDto sale)
        {
            return HandleResult(await Mediator.Send(new RegisterTyreSale.Command{ Sale = sale}));
        }

        [HttpGet("submissionHistory")]
        public async Task<IActionResult> GetSubmissionHistory()
        {
            var productionHistory = await Mediator.Send(new ListAllProductionHistory.Query());
            var salesHistory = await Mediator.Send(new ListSalesHistory.Query());

            return Ok(new 
            {
                ProductionHistory = productionHistory.Value,
                SalesHistory = salesHistory.Value
            });
        }

        [HttpGet("productionHistory")]
        public async Task<IActionResult> GetProductionHistory()
        {
            return HandleResult(await Mediator.Send(new ListAllProductionHistory.Query()));
        }
        
        [HttpGet("saleHistory")]
        public async Task<IActionResult> GetSaleHistory()
        {
            return HandleResult(await Mediator.Send(new ListSalesHistory.Query()));
        }

        [HttpPut("updateProduction/{id}")]
        public async Task<IActionResult> UpdateProduction(string id, int shift, int quantityProduced, DateTime date)
        {
            var command = new UpdateProduction.Command
            {
                Id = id,
                Shift = shift,
                QuantityProduced = quantityProduced,
                Date = date
            };
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPut("updateSale/{id}")]
        public async Task<IActionResult> UpdateSale(string id, string tyreId, string clientId, int quantitySold, double pricePerUnit, DateTime saleDate)
        {
            var command = new UpdateSale.Command
            {
                Id = id,
                TyreId = tyreId,
                ClientId = clientId,
                QuantitySold = quantitySold,
                PricePerUnit = pricePerUnit,
                SaleDate = saleDate
            };
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("deleteProduction/{id}")]
        public async Task<IActionResult> DeleteProduction(string id)
        {
            return HandleResult(await Mediator.Send(new DeleteProduction.Command { Id = id }));
        }

        [HttpDelete("deleteSale/{id}")]
        public async Task<IActionResult> DeleteSale(string id)
        {
            return HandleResult(await Mediator.Send(new DeleteSale.Command { Id = id }));
        }
    }
}