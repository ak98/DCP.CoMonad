using System.Collections.Generic;
using CoMonad;

namespace CoMonadTest
{
    sealed class PaymentAmount : ValueObject
    {
        private readonly decimal _value;
        private PaymentAmount(decimal checknum) => _value = checknum;
        public static Result<PaymentAmount> Create(decimal amount)
        {
            if (amount < 0)
                return CashErr.CashErrNegative;


            return Result.Ok(new PaymentAmount(amount));
        }

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { _value };

        public static implicit operator decimal(PaymentAmount amount) => amount._value;
        public static explicit operator PaymentAmount(decimal amount) => Create(amount).Value;
        public override string ToString() => _value.ToString("C");
    }
}
