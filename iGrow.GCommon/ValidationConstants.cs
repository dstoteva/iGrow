namespace iGrow.GCommon
{
    public static class ValidationConstants
    {
        // Task
        public const int TaskTitleMinLength = 3;
        public const int TaskTitleMaxLength = 80;

        public const int TaskNoteMaxLength = 1000;

        public const int TaskPriorityMinValue = 1;
        public const int TaskPriorityMaxValue = 5;

        // Category
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 20;
        public const int CategoryImageUrlMaxLength = 2048;

        //Habit
        public const int HabitTitleMinLength = 3;
        public const int HabitTitleMaxLength = 80;

        public const int HabitNoteMaxLength = 1000;

        public const int HabitPriorityMinValue = 1;
        public const int HabitPriorityMaxValue = 5;

        //RecurringType
        public const int RecurringTypeNameMinLength = 1;
        public const int RecurringTypeNameMaxLength = 10;

        //Amount
        public const int AmountNameMinLength = 1;
        public const int AmountNameMaxLength = 10;

        //Generic error messages
        public const string RequiredErrorMessage = "The {0} field is required.";
        public const string StringLengthErrorMessage = "The {0} must be between {2} and {1} characters long.";
        public const string RangeErrorMessage = "The {0} must be between {1} and {2}.";
        public const string MaxLengthErrorMessage = "The {0} must be at most {1} characters long.";

        //Database persistence error messages
        public const string TaskPersistenceErrorMessage = "An error occurred while creating the new task. Please try again later.";
        public const string HabitPersistenceErrorMessage = "An error occurred while creating the new habit. Please try again later.";
        public const string UnexpectedErrorMessage = "An unexpected error occurred. Please try again later.";

        //Error messages
        public const string StartDateMustBeBeforeEndDate = "Habit Start Date must be before End Date.";
    }
}
