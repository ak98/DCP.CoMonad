#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMonad
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            var valueObject = obj as ValueObject;

            if (valueObject is null)
                return false;

            if (GetType() != valueObject.GetType())
                return false;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) => {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null ^ b is null)
            {
                return false;
            }
            return a?.Equals(b) != false;
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

    }
}
