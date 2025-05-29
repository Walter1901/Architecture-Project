using Microsoft.AspNetCore.Mvc;
using PrintSystem.BLL.Interfaces;
using PrintSystem.Models;
using WebAPI_PrintSystem.Models;

namespace WebAPI_PrintSystem.Controllers
{
    /// <summary>
    /// Controller responsible for managing print quotas for users
    /// Handles quota allocation, retrieval, and management operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QuotaController : ControllerBase
    {
        private readonly IQuotaService _quotaService;

        /// <summary>
        /// Constructor - Initializes the controller with quota service dependency
        /// </summary>
        /// <param name="quotaService">Business logic service for quota operations</param>
        public QuotaController(IQuotaService quotaService)
        {
            _quotaService = quotaService ?? throw new ArgumentNullException(nameof(quotaService));
        }

        /// <summary>
        /// Adds print quota amount to a user's account
        /// Used by faculty members to allocate quota to students
        /// </summary>
        /// <param name="request">Request containing username and quota amount</param>
        /// <returns>API response indicating success or failure</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddAmount([FromBody] AddAmountRequest request)
        {
            try
            {
                // Validate request data at controller level
                if (request == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Request cannot be empty"
                    });
                }

                // Additional controller-level validation for API constraints
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Username is required"
                    });
                }

                if (request.Quotas <= 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Quota amount must be positive"
                    });
                }

                // Call business logic service to handle quota addition
                // The BLL will handle business rules, validation, and data persistence
                var result = await _quotaService.AddQuotaAsync(request.Username, request.Quotas);

                // Return appropriate HTTP response based on business logic result
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument validation errors
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Invalid request: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors and log them (logging would be added here)
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "An internal server error occurred while processing the quota addition"
                });
            }
        }

        /// <summary>
        /// Retrieves the available quota amount for a specific user
        /// Used to check how much print credit a user currently has
        /// </summary>
        /// <param name="username">Username to check quota for</param>
        /// <returns>Available quota amount as float value</returns>
        [HttpGet("available/{username}")]
        public async Task<IActionResult> GetAvailableAmount(string username)
        {
            try
            {
                // Validate username parameter
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Username parameter is required"
                    });
                }

                // Call business logic service to retrieve available quota
                // BLL handles business rules and data access
                var availableAmount = await _quotaService.GetAvailableQuotaAsync(username);

                // Return the quota amount directly (matches existing API contract)
                return Ok(availableAmount);
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument validation errors
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Invalid username: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "An internal server error occurred while retrieving quota information"
                });
            }
        }

        /// <summary>
        /// Deducts print quota from a user's account
        /// Used when a user performs a print operation
        /// </summary>
        /// <param name="username">Username to deduct quota from</param>
        /// <param name="amount">Amount to deduct from user's quota</param>
        /// <returns>API response indicating success or failure of the deduction</returns>
        [HttpPost("deduct/{username}")]
        public async Task<IActionResult> DeductAmount(string username, [FromBody] float amount)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Username is required"
                    });
                }

                if (amount <= 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Deduction amount must be positive"
                    });
                }

                // Additional business constraint at API level
                if (amount > 100) // Maximum single deduction limit
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Single deduction cannot exceed 100 CHF"
                    });
                }

                // Call business logic service to handle quota deduction
                // BLL will check sufficient funds and handle the deduction logic
                var result = await _quotaService.DeductQuotaAsync(username, amount);

                // Return appropriate response based on business logic result
                if (result.Success)
                {
                    return Ok(result);
                }

                // Business logic determined the operation failed (e.g., insufficient funds)
                return BadRequest(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Invalid request: {ex.Message}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "An internal server error occurred while processing the quota deduction"
                });
            }
        }

        /// <summary>
        /// Gets quota summary information for a user including recent transactions
        /// Provides comprehensive quota information for user dashboards
        /// </summary>
        /// <param name="username">Username to get quota summary for</param>
        /// <returns>Detailed quota information including balance and recent activity</returns>
        [HttpGet("summary/{username}")]
        public async Task<IActionResult> GetQuotaSummary(string username)
        {
            try
            {
                // Validate username parameter
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        ErrorMessage = "Username parameter is required"
                    });
                }

                // Get current available quota from business logic
                var availableQuota = await _quotaService.GetAvailableQuotaAsync(username);

                // Create summary response object
                var quotaSummary = new
                {
                    Username = username,
                    AvailableQuota = availableQuota,
                    LastUpdated = DateTime.UtcNow,
                    Status = availableQuota > 0 ? "Active" : "No Credit"
                };

                return Ok(new ApiResponse
                {
                    Success = true,
                    Data = quotaSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "An internal server error occurred while retrieving quota summary"
                });
            }
        }
    }
}