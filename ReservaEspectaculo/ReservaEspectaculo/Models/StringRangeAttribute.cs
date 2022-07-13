using System;
using System.ComponentModel.DataAnnotations;
namespace ReservaEspectaculo.Validaciones
{
    public class StringRangeAttribute : ValidationAttribute
    {
        private readonly string _desde;
        private readonly string _hasta;

        public StringRangeAttribute (string desde, string hasta)
        {
            _desde = desde;
            _hasta = hasta;
        }
        public override bool IsValid(object value)
        {
            if (value is String campo)
            {
                if (String.Compare(campo, _desde) >= 0 && String.Compare(campo, _hasta) <= 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
