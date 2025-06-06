namespace WebAPI.Validation.Common
{
    /// <summary>
    /// Contains reusable validation constants and methods for FluentValidation rules.
    /// </summary>
    public static class CommonValidators
    {
        #region Properties

        /// <summary>
        /// Regular expression that defines a valid username.
        /// Allows only letters, digits, underscores, dots, and hyphens.
        /// </summary>
        public const string UserNamePattern = "^[a-zA-Z0-9_.-]*$";

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the provided password contains at least one uppercase letter.
        /// </summary>
        public static bool ContainsUppercase(string? password) =>
            !string.IsNullOrEmpty(password) && password.Any(char.IsUpper);

        /// <summary>
        /// Checks if the provided password contains at least one lowercase letter.
        /// </summary>
        public static bool ContainsLowercase(string? password) =>
            !string.IsNullOrEmpty(password) && password.Any(char.IsLower);

        /// <summary>
        /// Checks if the provided password contains at least one numeric digit.
        /// </summary>
        public static bool ContainsDigit(string? password) =>
            !string.IsNullOrEmpty(password) && password.Any(char.IsDigit);

        /// <summary>
        /// Checks if the provided password contains at least one special character.
        /// Special characters are defined as non-letter and non-digit characters.
        /// </summary>
        public static bool ContainsSpecialCharacter(string? password) =>
            !string.IsNullOrEmpty(password) && password.Any(ch => !char.IsLetterOrDigit(ch));

        /// <summary>
        /// Validates whether the given input is a valid phone number.
        /// Accepts international formats by allowing 10 to 15 numeric digits after cleaning.
        /// </summary>
        public static bool BeValidPhoneNumber(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            var digitsOnly = new string(input.Where(char.IsDigit).ToArray());
            return digitsOnly.Length >= 10 && digitsOnly.Length <= 15;
        }

        #endregion
    }
}
