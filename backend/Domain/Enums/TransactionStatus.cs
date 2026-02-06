namespace Domain.Enums
{
    /// <summary>
    /// Represents the status of a transaction in the system.
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// The transaction is pending and awaiting processing.
        /// </summary>
        Pending,

        /// <summary>
        /// The transaction has been completed successfully.
        /// </summary>
        Completed,

        /// <summary>
        /// The transaction has been cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The transaction has failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The transaction has been refunded.
        /// </summary>
        Refunded
    }
}
