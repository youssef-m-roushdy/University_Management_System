namespace University_Management_System.Application.Validations.Common
{
    public static class ValidationMessages
    {
        // ─── General ──────────────────────────────────────────────────────────
        public const string Required = "{PropertyName} is required";
        public const string MaxLength = "{PropertyName} must not exceed {MaxLength} characters";
        public const string MinLength = "{PropertyName} must be at least {MinLength} characters";
        public const string InvalidFormat = "Invalid {PropertyName} format";

        // ─── Email ────────────────────────────────────────────────────────────
        public const string InvalidEmail = "Invalid email format";

        // ─── Phone ────────────────────────────────────────────────────────────
        public const string InvalidPhone = "Invalid phone number format. Use international format (e.g., +1234567890)";

        // ─── Password ─────────────────────────────────────────────────────────
        public const string PasswordMismatch = "Passwords do not match";
        public const string PasswordWeak = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character";

        // ─── File ─────────────────────────────────────────────────────────────
        public const string FileEmpty = "File is empty";
        public const string FileTooLarge = "File size must not exceed {MaxSize}MB";
        public const string InvalidFileType = "Invalid file type. Allowed types: {AllowedTypes}";

        // ─── Role ─────────────────────────────────────────────────────────────
        public const string RoleNameInvalid = "Role name can only contain letters, spaces, and hyphens";

        // ─── URL ──────────────────────────────────────────────────────────────
        public const string InvalidUrl = "Invalid URL format";

        // ─── Date ─────────────────────────────────────────────────────────────
        public const string InvalidDate = "Invalid date";
        public const string DateMustBeFuture = "Date must be in the future";
        public const string DateMustBePast = "Date must be in the past";

        // ─── Enum ─────────────────────────────────────────────────────────────
        public const string InvalidEnumValue = "Invalid {PropertyName} value";

        // ─── Update ───────────────────────────────────────────────────────────
        public const string AtLeastOneField = "At least one field must be provided for update";

        // ─── Pagination ──────────────────────────────────────────────────────
        public const string PageNumberMin = "Page number must be at least 1";
        public const string PageSizeMin = "Page size must be at least 1";
        public const string PageSizeMax = "Page size must not exceed 50";
        public const string InvalidSortDirection = "Invalid sort direction. Must be Ascending or Descending";
        public const string InvalidSortField = "Invalid sort field";
        public const string SearchTermMaxLength = "Search term must not exceed 100 characters";

        // ─── GPA ──────────────────────────────────────────────────────────────
        public const string GpaRange = "GPA must be between 0 and 4";
        public const string GpaMinMax = "Minimum GPA must be less than or equal to Maximum GPA";

        // ─── Credits ──────────────────────────────────────────────────────────
        public const string CreditsMinMax = "Minimum credits must be less than or equal to Maximum credits";
        public const string CreditsPositive = "Credits must be greater than or equal to 0";
        // ─── Id ───────────────────────────────────────────────────────────────
        public const string InvalidId = "{PropertyName} must be greater than 0";

        // ─── Amount ───────────────────────────────────────────────────────────
        public const string AmountPositive = "Amount must be greater than or equal to 0";
        public const string AmountMinMax = "Minimum amount must be less than or equal to Maximum amount";

        // ─── Date Range ──────────────────────────────────────────────────────
        public const string DateRangeMinMax = "Start date must be less than or equal to End date";
    }
}