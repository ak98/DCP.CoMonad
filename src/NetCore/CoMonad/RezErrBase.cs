#nullable enable

using System;
using System.Text;
using System.Threading.Tasks;

namespace CoMonad
{
    public abstract class RezErrBase
    {
        protected string _value;
        private RezErrBase() { _value = string.Empty; }

        protected RezErrBase(string value)
        {
            _value = value ?? throw new ArgumentException($"{nameof(value)} is null.", nameof(value));
        }
        protected RezErrBase(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentException($"{nameof(exception)} is null.", nameof(exception));
            }
            _value = exception.GetType().Name;
            Exception = exception;
        }
        protected RezErrBase(RezErrBase errBase)// for EmbelishedError
        {
            if (errBase is null)
            {
                throw new ArgumentNullException(nameof(errBase));
            }
            this.Exception = errBase.Exception;
            _value = errBase._value;
        }
        protected RezErrBase(string value, Exception exception)
        {
            _value = value ?? throw new ArgumentException($"{nameof(value)} is null.", nameof(value));
            if (exception is null)
            {
                throw new ArgumentException($"{nameof(exception)} is null.", nameof(exception));
            }
            Exception = exception;
        }
        public readonly Exception? Exception;

        //# value as string when string expected . Note ToString() may return additional info
        public static implicit operator string(RezErrBase value)
            => value._value;

        //# for when implicit conversion is not enough eg TypeParamers not inferred by compiler
        public Result<T> AsFail<T>() => Result<T>.Fail(this);
        public Task<Result<T>> AsTaskFail<T>() => Task.FromResult(Result<T>.Fail(this));
        public Result<T> AsFailIf<T>(T t, bool isfail)
              => isfail ? Result<T>.Fail(this) : Result.Ok(t);
        public RezErrBase Embelish(string extraErrorInfo)
    => new RezEmbelishedError(this, extraErrorInfo);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_value);
            if (Exception is { })
            {
                sb.AppendLine();
                sb.Append("Exception:");
                sb.Append(Exception.Message);
            }

            return sb.ToString();
        }

        public override int GetHashCode()
            => _value?.GetHashCode() ?? 0;
        public override bool Equals(object? obj)
            => obj is RezErrBase se && string.Compare(se._value, _value, System.StringComparison.Ordinal) == 0;
        public static bool operator ==(RezErrBase a, RezErrBase b)
            => ReferenceEquals(a, b) || (!(a is null || b is null) && string.Compare(a._value, b._value, System.StringComparison.Ordinal) == 0);
        public static bool operator !=(RezErrBase a, RezErrBase b)
        {
            return !(a == b);
        }
    }
}
