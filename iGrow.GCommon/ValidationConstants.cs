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
        public const string RoleSeedingFailure = "There was an error while trying to seed roles.";
        public const string AdminUserSeedingNotFound = "Admin Email not found in configuration.";
        public const string AdminUserSeedingPasswordNotFound = "Admin Password not found in configuration.";
        public const string AdminUserSeedingException = "There was an error while trying to seed Admin user.";
        public const string StartDateMustBeBeforeEndDate = "Habit Start Date must be before End Date.";

        //Application User error messages
        public const string UserRoleAssignmentIdentityErrorMessage = "There was an error assigning role {0} to the user! Ensure that user and role exists and user is not assigned to the role!";
        public const string UserRoleAssignmentFailureMessage = "There was an error {1} role {0} to the user! Please review log information!";
        public const string UserRoleRemoveIdentityErrorMessage = "There was an error removing role {0} from the user! Ensure that user and role exists and user is assigned to the role!";
        public const string UserDeleteNotExistMessage = "There was an error deleting the user! Ensure that the user exists!";
        public const string UserDeleteFailureErrorMessage = "There was an error deleting the user! Please review log information!";

        //Admin area error messages
        public const string UnableToDeleteErrorMessage = "Unable to delete. Item is in use.";
        public const string ItemExistsErrorMessage = "An item with the same name already exists.";
    }
}
