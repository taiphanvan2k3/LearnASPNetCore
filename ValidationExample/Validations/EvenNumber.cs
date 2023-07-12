using System.ComponentModel.DataAnnotations;

namespace ValidationExample.Validations
{
    /// <summary>
    /// Attribute tự tạo để chỉ chấp nhận các số chẵn
    /// </summary>
    public class EvenNumber : ValidationAttribute
    {
        public EvenNumber() => ErrorMessage = "{0} phải là số chẵn";

        public override bool IsValid(object value)
        {
            if (value == null || value.GetType() != typeof(int))
            {
                return false;
            }

            int number = Convert.ToInt32(value);
            return number % 2 == 0;
        }
    }
}