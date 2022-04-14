using System.ComponentModel.DataAnnotations;

namespace App.Helpers.FormatValidator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AmountAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return false;
            }
            else if (int.TryParse(value.ToString(), out int intValue))
            {
                return intValue > 0;
            }
            else
            {
                return false;
            }
        }
    }
}