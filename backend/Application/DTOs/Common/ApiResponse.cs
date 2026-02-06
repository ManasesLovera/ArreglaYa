namespace Application.DTOs.Common
{
    /// <summary>
    /// Generic API response wrapper for consistent response handling.
    /// Provides a standardized format for success and error responses.
    /// </summary>
    /// <typeparam name="T">The type of data contained in the response.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets the message describing the result of the operation.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets additional error details or validation errors.
        /// </summary>
        public object? Errors { get; set; }

        /// <summary>
        /// Creates a successful response with data.
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <param name="message">Optional success message.</param>
        /// <returns>An ApiResponse indicating success.</returns>
        public static ApiResponse<T> Success(T data, string message = "Operation successful.")
        {
            return new ApiResponse<T> 
            { 
                IsSuccess = true, 
                Data = data, 
                Message = message 
            };
        }

        /// <summary>
        /// Creates a failure response with an error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errors">Optional additional error details.</param>
        /// <returns>An ApiResponse indicating failure.</returns>
        public static ApiResponse<T> Failure(string errorMessage, object? errors = null)
        {
            return new ApiResponse<T> 
            { 
                IsSuccess = false, 
                ErrorMessage = errorMessage,
                Errors = errors
            };
        }

        /// <summary>
        /// Creates a successful response with data (backward compatibility).
        /// </summary>
        /// <param name="data">The response data.</param>
        /// <returns>An ApiResponse indicating success.</returns>
        public static ApiResponse<T> SuccessResponse(T data) =>
           new ApiResponse<T> { IsSuccess = true, Data = data, Message = "Operation successful." };

        /// <summary>
        /// Creates a failure response with an error message (backward compatibility).
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>An ApiResponse indicating failure.</returns>
        public static ApiResponse<T> ErrorResponse(string errorMessage) =>
            new ApiResponse<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
