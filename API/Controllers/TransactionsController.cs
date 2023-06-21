using BLL.DTO.Adding;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO.Plain;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "CEO, Manager")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await transactionService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transaction = await transactionService.GetByIdAsync(id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TransactionAddingDTO transactionToAdd)
        {
            var addedTransaction = await transactionService.AddAsync(transactionToAdd);

            if (addedTransaction is null)
                return BadRequest("Transaction creation failed.");

            return CreatedAtAction(nameof(GetById), new { id = addedTransaction.TransactionId }, addedTransaction);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TransactionPlainDTO transactionToUpdate)
        {
            var updatedTransaction = await transactionService.UpdateAsync(transactionToUpdate);
            if (updatedTransaction == null)
                return NotFound();

            return Ok(updatedTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await transactionService.DeleteAsync(id);
            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}
