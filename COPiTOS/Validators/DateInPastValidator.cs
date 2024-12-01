using System.ComponentModel.DataAnnotations;

namespace COPiTOS.Validators;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DateInPastValidator: ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) 
        {
            return false;
        }

        if (DateTime.TryParse(value.ToString(), out DateTime date))
        {
            return date.Date < DateTime.Now.Date;
        }
        else
        {
            return false;
        }
    }
}
