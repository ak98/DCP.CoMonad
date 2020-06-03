using System.Collections.Generic;
using CoMonad;

namespace CoMonadTest
{
    sealed class Cash : ValueObject
    {
        private readonly decimal _value;
        private Cash(decimal checknum) => _value = checknum;
        public static Result<Cash> Create(decimal amount)
        {
            if (amount < 0)
                return CashErr.CashErrNegative;


            return Result.Ok(new Cash(amount));
        }

        protected override IEnumerable<object> GetEqualityComponents() => new object[] { _value };

        public static implicit operator decimal(Cash cash) => cash._value;
        public static explicit operator Cash(decimal cash) => Create(cash).Value;
        public override string ToString() => _value.ToString("C");
    }
}
