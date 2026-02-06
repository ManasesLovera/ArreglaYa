using Application.DTOs.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers.Base
{
    /// <summary>
    /// Custom base controller providing common CRUD operations and utilities.
    /// Includes support for pagination, validation, mapping, and logging.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCreateRequest">The DTO for creating entities.</typeparam>
    /// <typeparam name="TUpdateRequest">The DTO for updating entities.</typeparam>
    /// <typeparam name="TResponse">The response DTO type.</typeparam>
    public abstract class CustomBaseController<TEntity, TCreateRequest, TUpdateRequest, TResponse> : ControllerBase
        where TEntity : class
        where TCreateRequest : class
        where TUpdateRequest : class
        where TResponse : class
    {
        /// <summary>
        /// AutoMapper instance for DTO mappings.
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Logger instance for logging operations.
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBaseController{TEntity, TCreateRequest, TUpdateRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="logger">The logger instance.</param>
        protected CustomBaseController(IMapper mapper, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets a paginated list of entities.
        /// </summary>
        /// <param name="query">The pagination query parameters.</param>
        /// <returns>A paginated response containing the entities.</returns>
        protected abstract Task<PaginatedResponse<TResponse>> GetPaginatedAsync(PaginationQuery query);

        /// <summary>
        /// Gets a single entity by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>An action result containing the entity response.</returns>
        protected abstract Task<ActionResult<TResponse>> GetByIdAsync(string id);

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="request">The creation request DTO.</param>
        /// <returns>An action result indicating the creation result.</returns>
        protected abstract Task<IActionResult> CreateAsync(TCreateRequest request);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="request">The update request DTO.</param>
        /// <returns>An action result indicating the update result.</returns>
        protected abstract Task<IActionResult> UpdateAsync(string id, TUpdateRequest request);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>An action result indicating the deletion result.</returns>
        protected abstract Task<IActionResult> DeleteAsync(string id);

        /// <summary>
        /// Validates a request using FluentValidation.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request to validate.</typeparam>
        /// <param name="request">The request object to validate.</param>
        /// <param name="validator">The validator instance.</param>
        /// <returns>A validation result wrapped in an ApiResponse.</returns>
        protected async Task<ApiResponse<object>> ValidateRequestAsync<TRequest>(
            TRequest request,
            IValidator<TRequest> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.ToDictionary();
                return ApiResponse<object>.Failure("Validation failed.", errors);
            }

            return ApiResponse<object>.Success(null, "Validation passed.");
        }

        /// <summary>
        /// Creates a success response with data.
        /// </summary>
        /// <typeparam name="T">The type of data in the response.</typeparam>
        /// <param name="data">The response data.</param>
        /// <param name="message">The success message.</param>
        /// <returns>An ApiResponse indicating success.</returns>
        protected ApiResponse<T> SuccessResponse<T>(T data, string message = "Operation successful.")
        {
            return ApiResponse<T>.Success(data, message);
        }

        /// <summary>
        /// Creates a failure response with an error message.
        /// </summary>
        /// <typeparam name="T">The type of data in the response.</typeparam>
        /// <param name="message">The error message.</param>
        /// <param name="errors">Optional error details.</param>
        /// <returns>An ApiResponse indicating failure.</returns>
        protected ApiResponse<T> FailureResponse<T>(string message, object? errors = null)
        {
            return ApiResponse<T>.Failure(message, errors);
        }

        /// <summary>
        /// Logs an error with exception details.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="exception">The exception that occurred.</param>
        protected void LogError(string message, Exception exception = null)
        {
            if (exception != null)
            {
                _logger.LogError(exception, message);
            }
            else
            {
                _logger.LogError(message);
            }
        }

        /// <summary>
        /// Logs informational messages.
        /// </summary>
        /// <param name="message">The informational message.</param>
        protected void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
