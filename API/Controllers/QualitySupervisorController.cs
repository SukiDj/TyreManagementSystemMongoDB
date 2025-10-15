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
        public async Task<IActionResult> RegisterTyreSale(RegisterTyreSaleDto sale)
        {
            RegisterTyreSaleDto dto = new RegisterTyreSaleDto
            {
                TyreId = "68eecb6d1a19272260eb3c58",
                ClientId = "68eecb6d1a19272260eb3c5b",
                PricePerUnit = 100,
                QuantitySold = 20,
                SaleDate = sale.SaleDate,
                UnitOfMeasure = "kom",
                ProductionOrderId = "68eed719f84e2a41ce5b04bb"
            };
            var command = new RegisterTyreSale.Command
            {
                Sale = sale
            };
            return HandleResult(await Mediator.Send(command));
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