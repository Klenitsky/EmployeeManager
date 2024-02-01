using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateService.UI.Validators
{
    public class DateRangeValidator
    {
        private DateTime _minDate;
        private DateTime _maxDate;

        public DateRangeValidator(DateTime minDate, DateTime maxDate)
        {
            _minDate = minDate;
            _maxDate = maxDate;
        }

        public bool Validate(DateTime startDate, DateTime endDate)
        {
            if((startDate > endDate) || (startDate< _minDate) || (endDate> _maxDate))
            {
                return false;
            }
            return true;
        }
    }
}
