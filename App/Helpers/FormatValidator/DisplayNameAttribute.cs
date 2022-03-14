
using System.ComponentModel.DataAnnotations;

namespace App.Services
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DisplayNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return false;
            }
            else
            {
                return value.ToString() != "";
            }
        }
    }
}