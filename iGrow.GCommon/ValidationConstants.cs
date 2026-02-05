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
    }
}
