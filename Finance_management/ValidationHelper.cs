using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Finance_management
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return Regex.IsMatch(email.Trim(),
                @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        public static bool IsValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) &&
                   Regex.IsMatch(phone.Trim(), @"^\d{10}$");
        }

        public static bool IsValidAmount(string amount, out double value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(amount))
            {
                return false;
            }

            if (!double.TryParse(amount.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value) &&
                !double.TryParse(amount.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out value))
            {
                return false;
            }

            return value > 0;
        }

        public static bool IsValidDate(string dateText, out DateTime date)
        {
            date = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(dateText))
            {
                return false;
            }

            return DateTime.TryParse(dateText, out date);
        }

        public static bool IsCategorySelected(string selectedValue)
        {
            return !string.IsNullOrWhiteSpace(selectedValue) &&
                   !selectedValue.Equals("Select", StringComparison.OrdinalIgnoreCase);
        }
    }
}
