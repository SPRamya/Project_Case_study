using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Case_study_PayXpert.exception;

namespace Case_study_PayXpert.util
{
    public static class ValidationService
    {
        public static void ValidateRequiredField(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidInputException($"{fieldName} is required.");
        }

        public static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new InvalidInputException("Invalid email format.");
        }

        public static void ValidatePhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || !Regex.IsMatch(phone, @"^\+?\d{10,15}$"))
                throw new InvalidInputException("Invalid phone number format.");
        }

        public static void ValidateDate(DateTime date, string fieldName)
        {
            if (date > DateTime.Now)
                throw new InvalidInputException($"{fieldName} cannot be in the future.");
        }

        public static void ValidateGender(string gender)
        {
            if (gender != "M" && gender != "F" && gender != "O")
                throw new InvalidInputException("Gender must be M (Male), F (Female), or O (Other).");
        }
    }
}