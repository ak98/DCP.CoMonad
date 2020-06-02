using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CoMonad
{
   public static  class ComonadImplementationExtensions
    {

        internal static int Hash(this IEnumerable items)
        {
            long _combinedHash64 = 0;
            int count = 1;
            foreach (var e in items)
            {
                switch (e)
                {
                    case null:
                        AddI(0);
                        break;
                    default:
                        Add(e);

                        break;
                }
                AddI(count++);
            }
            return _combinedHash64.GetHashCode();
            void Add(object o)
            {
                var hashCode = (o != null) ? o.GetHashCode() : 0;
                AddI(hashCode);
            }
            void AddI(int i)
            {
                _combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;

            }

        }

        //# Type ==> string
        internal static string SimpleName(this Type t)
        {
            var simpleName = t.Name;
            var backtickIndex = simpleName.IndexOf("`");
            if (backtickIndex > 0)
            {
                simpleName = simpleName.Substring(0, backtickIndex);
            }
            return simpleName;
        }



        //# Exception ==> Result<R>
        public static Result<R> AsFail<R>(this Exception ex) => Result<R>.Fail(ex);


        //#  Func<T1, T2> ==> T1 ==> Result<T2>
        public static Result<T2> TryResult<T1, T2>(this Func<T1, T2> func, T1 t1)
        {
            try
            {
                T2 t2 = func(t1);
                return Result.Ok(t2);
            }
            catch (Exception ex)
            {
                return ex.AsFail<T2>();
            }
        }
        //#  Func<T1> ==> Result<T1>
        public static Result<T1> TryResult<T1>(this Func<T1> func)
        {
            try
            {
                T1 t1 = func();
                return Result.Ok(t1);
            }
            catch (Exception ex)
            {
                return ex.AsFail<T1>();
            }
        }
        //# Func<A, B, C> ==> Func<A, Func<B, C>>
        public static Func<A, Func<B, C>> Curry<A, B, C>(this Func<A, B, C> func)
               => a => b => func(a, b);
    }
}
