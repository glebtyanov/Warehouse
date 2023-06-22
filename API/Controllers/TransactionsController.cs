using BLL.DTO.Adding;
using BLL.DTO.Plain;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "CEO, Manager")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService transactionService;
        private readonly ILogger<TransactionsController> logger;

        public TransactionsController(TransactionService transactionService, ILogger<TransactionsController> logger)
        {
            this.transactionService = transactionService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Fetching all transactions");

            return Ok(await transactionService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            logger.LogInformation("Fetching transaction by ID: {Id}", id);
            var transaction = await transactionService.GetByIdAsync(id);

            return Ok(transaction);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            logger.LogInformation("Fetching transaction details by ID: {Id}", id);
            var foundTransaction = await transactionService.GetDetailsByIdAsync(id);

            return Ok(foundTransaction);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TransactionAddingDTO transactionToAdd)
        {
            logger.LogInformation("Adding a new transaction");
            var addedTransaction = await transactionService.AddAsync(transactionToAdd);

            logger.LogInformation("Transaction created successfully");

            return CreatedAtAction(nameof(GetById), new { id = addedTransaction.TransactionId }, addedTransaction);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TransactionPlainDTO transactionToUpdate)
        {
            logger.LogInformation("Updating transaction with ID: {Id}", transactionToUpdate.TransactionId);
            var updatedTransaction = await transactionService.UpdateAsync(transactionToUpdate);

            logger.LogInformation("Transaction updated successfully");

            return Ok(updatedTransaction);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            logger.LogInformation("Deleting transaction with ID: {Id}", id);
            transactionService.DeleteAsync(id);

            logger.LogInformation("Transaction with ID {Id} deleted", id);

            return Ok("Transaction successfully deleted");
        }
    }
}
