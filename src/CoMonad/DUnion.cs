
using System;
using System.Text;
using System.Reflection;
namespace CoMonad
{

    public struct DUnion<T1, T2> : IDUnion
    {
         public static implicit operator DUnion<T1, T2>(T1 t) => new DUnion<T1, T2>(1, t);
         public static implicit operator DUnion<T1, T2>(T2 t) => new DUnion<T1, T2>(2, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(DUnion<T1, T2> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
            }
        }
    }

    public struct DUnion<T1, T2, T3> : IDUnion
    {
         public static implicit operator DUnion<T1, T2, T3>(T1 t) => new DUnion<T1, T2, T3>(1, t);
         public static implicit operator DUnion<T1, T2, T3>(T2 t) => new DUnion<T1, T2, T3>(2, t);
         public static implicit operator DUnion<T1, T2, T3>(T3 t) => new DUnion<T1, T2, T3>(3, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(T3 thirdValue)
            : this(3, thirdValue)
        {
        }


         DUnion(DUnion<T1, T2, T3> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 
        public bool IsThird => Discriminator == 3; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 
        public T3 Third => IsThird ? GetCoproductValue<T3>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                case 3: return ifThird(Third);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
                case 3: ifThird?.Invoke(Third); break;
            }
        }
    }

    public struct DUnion<T1, T2, T3, T4> : IDUnion
    {
         public static implicit operator DUnion<T1, T2, T3, T4>(T1 t) => new DUnion<T1, T2, T3, T4>(1, t);
         public static implicit operator DUnion<T1, T2, T3, T4>(T2 t) => new DUnion<T1, T2, T3, T4>(2, t);
         public static implicit operator DUnion<T1, T2, T3, T4>(T3 t) => new DUnion<T1, T2, T3, T4>(3, t);
         public static implicit operator DUnion<T1, T2, T3, T4>(T4 t) => new DUnion<T1, T2, T3, T4>(4, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(T3 thirdValue)
            : this(3, thirdValue)
        {
        }


         DUnion(T4 fourthValue)
            : this(4, fourthValue)
        {
        }


         DUnion(DUnion<T1, T2, T3, T4> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 
        public bool IsThird => Discriminator == 3; 
        public bool IsFourth => Discriminator == 4; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 
        public T3 Third => IsThird ? GetCoproductValue<T3>() : default; 
        public T4 Fourth => IsFourth ? GetCoproductValue<T4>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                case 3: return ifThird(Third);
                case 4: return ifFourth(Fourth);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
                case 3: ifThird?.Invoke(Third); break;
                case 4: ifFourth?.Invoke(Fourth); break;
            }
        }
    }

    public struct DUnion<T1, T2, T3, T4, T5> : IDUnion
    {
         public static implicit operator DUnion<T1, T2, T3, T4, T5>(T1 t) => new DUnion<T1, T2, T3, T4, T5>(1, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5>(T2 t) => new DUnion<T1, T2, T3, T4, T5>(2, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5>(T3 t) => new DUnion<T1, T2, T3, T4, T5>(3, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5>(T4 t) => new DUnion<T1, T2, T3, T4, T5>(4, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5>(T5 t) => new DUnion<T1, T2, T3, T4, T5>(5, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(T3 thirdValue)
            : this(3, thirdValue)
        {
        }


         DUnion(T4 fourthValue)
            : this(4, fourthValue)
        {
        }


         DUnion(T5 fifthValue)
            : this(5, fifthValue)
        {
        }


         DUnion(DUnion<T1, T2, T3, T4, T5> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 
        public bool IsThird => Discriminator == 3; 
        public bool IsFourth => Discriminator == 4; 
        public bool IsFifth => Discriminator == 5; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 
        public T3 Third => IsThird ? GetCoproductValue<T3>() : default; 
        public T4 Fourth => IsFourth ? GetCoproductValue<T4>() : default; 
        public T5 Fifth => IsFifth ? GetCoproductValue<T5>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                case 3: return ifThird(Third);
                case 4: return ifFourth(Fourth);
                case 5: return ifFifth(Fifth);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
                case 3: ifThird?.Invoke(Third); break;
                case 4: ifFourth?.Invoke(Fourth); break;
                case 5: ifFifth?.Invoke(Fifth); break;
            }
        }
    }

    public struct DUnion<T1, T2, T3, T4, T5, T6> : IDUnion
    {
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6>(T1 t) => new DUnion<T1, T2, T3, T4, T5, T6>(1, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6>(T2 t) => new DUnion<T1, T2, T3, T4, T5, T6>(2, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6>(T3 t) => new DUnion<T1, T2, T3, T4, T5, T6>(3, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6>(T4 t) => new DUnion<T1, T2, T3, T4, T5, T6>(4, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6>(T5 t) => new DUnion<T1, T2, T3, T4, T5, T6>(5, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6>(T6 t) => new DUnion<T1, T2, T3, T4, T5, T6>(6, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(T3 thirdValue)
            : this(3, thirdValue)
        {
        }


         DUnion(T4 fourthValue)
            : this(4, fourthValue)
        {
        }


         DUnion(T5 fifthValue)
            : this(5, fifthValue)
        {
        }


         DUnion(T6 sixthValue)
            : this(6, sixthValue)
        {
        }


         DUnion(DUnion<T1, T2, T3, T4, T5, T6> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 
        public bool IsThird => Discriminator == 3; 
        public bool IsFourth => Discriminator == 4; 
        public bool IsFifth => Discriminator == 5; 
        public bool IsSixth => Discriminator == 6; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 
        public T3 Third => IsThird ? GetCoproductValue<T3>() : default; 
        public T4 Fourth => IsFourth ? GetCoproductValue<T4>() : default; 
        public T5 Fifth => IsFifth ? GetCoproductValue<T5>() : default; 
        public T6 Sixth => IsSixth ? GetCoproductValue<T6>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                case 3: return ifThird(Third);
                case 4: return ifFourth(Fourth);
                case 5: return ifFifth(Fifth);
                case 6: return ifSixth(Sixth);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
                case 3: ifThird?.Invoke(Third); break;
                case 4: ifFourth?.Invoke(Fourth); break;
                case 5: ifFifth?.Invoke(Fifth); break;
                case 6: ifSixth?.Invoke(Sixth); break;
            }
        }
    }

    public struct DUnion<T1, T2, T3, T4, T5, T6, T7> : IDUnion
    {
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7>(T1 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7>(1, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7>(T2 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7>(2, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7>(T3 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7>(3, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7>(T4 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7>(4, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7>(T5 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7>(5, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7>(T6 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7>(6, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7>(T7 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7>(7, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(T3 thirdValue)
            : this(3, thirdValue)
        {
        }


         DUnion(T4 fourthValue)
            : this(4, fourthValue)
        {
        }


         DUnion(T5 fifthValue)
            : this(5, fifthValue)
        {
        }


         DUnion(T6 sixthValue)
            : this(6, sixthValue)
        {
        }


         DUnion(T7 seventhValue)
            : this(7, seventhValue)
        {
        }


         DUnion(DUnion<T1, T2, T3, T4, T5, T6, T7> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 
        public bool IsThird => Discriminator == 3; 
        public bool IsFourth => Discriminator == 4; 
        public bool IsFifth => Discriminator == 5; 
        public bool IsSixth => Discriminator == 6; 
        public bool IsSeventh => Discriminator == 7; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 
        public T3 Third => IsThird ? GetCoproductValue<T3>() : default; 
        public T4 Fourth => IsFourth ? GetCoproductValue<T4>() : default; 
        public T5 Fifth => IsFifth ? GetCoproductValue<T5>() : default; 
        public T6 Sixth => IsSixth ? GetCoproductValue<T6>() : default; 
        public T7 Seventh => IsSeventh ? GetCoproductValue<T7>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                case 3: return ifThird(Third);
                case 4: return ifFourth(Fourth);
                case 5: return ifFifth(Fifth);
                case 6: return ifSixth(Sixth);
                case 7: return ifSeventh(Seventh);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
                case 3: ifThird?.Invoke(Third); break;
                case 4: ifFourth?.Invoke(Fourth); break;
                case 5: ifFifth?.Invoke(Fifth); break;
                case 6: ifSixth?.Invoke(Sixth); break;
                case 7: ifSeventh?.Invoke(Seventh); break;
            }
        }
    }

    public struct DUnion<T1, T2, T3, T4, T5, T6, T7, T8> : IDUnion
    {
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(1, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T2 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(2, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T3 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(3, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T4 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(4, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T5 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(5, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T6 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(6, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T7 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(7, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T8 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8>(8, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(T3 thirdValue)
            : this(3, thirdValue)
        {
        }


         DUnion(T4 fourthValue)
            : this(4, fourthValue)
        {
        }


         DUnion(T5 fifthValue)
            : this(5, fifthValue)
        {
        }


         DUnion(T6 sixthValue)
            : this(6, sixthValue)
        {
        }


         DUnion(T7 seventhValue)
            : this(7, seventhValue)
        {
        }


         DUnion(T8 eighthValue)
            : this(8, eighthValue)
        {
        }


         DUnion(DUnion<T1, T2, T3, T4, T5, T6, T7, T8> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 
        public bool IsThird => Discriminator == 3; 
        public bool IsFourth => Discriminator == 4; 
        public bool IsFifth => Discriminator == 5; 
        public bool IsSixth => Discriminator == 6; 
        public bool IsSeventh => Discriminator == 7; 
        public bool IsEighth => Discriminator == 8; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 
        public T3 Third => IsThird ? GetCoproductValue<T3>() : default; 
        public T4 Fourth => IsFourth ? GetCoproductValue<T4>() : default; 
        public T5 Fifth => IsFifth ? GetCoproductValue<T5>() : default; 
        public T6 Sixth => IsSixth ? GetCoproductValue<T6>() : default; 
        public T7 Seventh => IsSeventh ? GetCoproductValue<T7>() : default; 
        public T8 Eighth => IsEighth ? GetCoproductValue<T8>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                case 3: return ifThird(Third);
                case 4: return ifFourth(Fourth);
                case 5: return ifFifth(Fifth);
                case 6: return ifSixth(Sixth);
                case 7: return ifSeventh(Seventh);
                case 8: return ifEighth(Eighth);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
                case 3: ifThird?.Invoke(Third); break;
                case 4: ifFourth?.Invoke(Fourth); break;
                case 5: ifFifth?.Invoke(Fifth); break;
                case 6: ifSixth?.Invoke(Sixth); break;
                case 7: ifSeventh?.Invoke(Seventh); break;
                case 8: ifEighth?.Invoke(Eighth); break;
            }
        }
    }

    public struct DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IDUnion
    {
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(1, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T2 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(2, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T3 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(3, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T4 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(4, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T5 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(5, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T6 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(6, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T7 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(7, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T8 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(8, t);
         public static implicit operator DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T9 t) => new DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(9, t);

         DUnion(T1 firstValue)
            : this(1, firstValue)
        {
        }


         DUnion(T2 secondValue)
            : this(2, secondValue)
        {
        }


         DUnion(T3 thirdValue)
            : this(3, thirdValue)
        {
        }


         DUnion(T4 fourthValue)
            : this(4, fourthValue)
        {
        }


         DUnion(T5 fifthValue)
            : this(5, fifthValue)
        {
        }


         DUnion(T6 sixthValue)
            : this(6, sixthValue)
        {
        }


         DUnion(T7 seventhValue)
            : this(7, seventhValue)
        {
        }


         DUnion(T8 eighthValue)
            : this(8, eighthValue)
        {
        }


         DUnion(T9 ninthValue)
            : this(9, ninthValue)
        {
        }


         DUnion(DUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9> source)
            : this(source.Discriminator, source.CoproductValue)
        {
        }

         DUnion(byte discriminator, object value)
        {     
            Discriminator = discriminator;
            CoproductValue = value;
        }
         T GetCoproductValue<T>() => (CoproductValue is T t) ? t : default;
         public object CoproductValue { get; }
        public byte Discriminator { get; }
        public bool IsFirst => Discriminator == 1; 
        public bool IsSecond => Discriminator == 2; 
        public bool IsThird => Discriminator == 3; 
        public bool IsFourth => Discriminator == 4; 
        public bool IsFifth => Discriminator == 5; 
        public bool IsSixth => Discriminator == 6; 
        public bool IsSeventh => Discriminator == 7; 
        public bool IsEighth => Discriminator == 8; 
        public bool IsNinth => Discriminator == 9; 

        public T1 First => IsFirst ? GetCoproductValue<T1>() : default; 
        public T2 Second => IsSecond ? GetCoproductValue<T2>() : default; 
        public T3 Third => IsThird ? GetCoproductValue<T3>() : default; 
        public T4 Fourth => IsFourth ? GetCoproductValue<T4>() : default; 
        public T5 Fifth => IsFifth ? GetCoproductValue<T5>() : default; 
        public T6 Sixth => IsSixth ? GetCoproductValue<T6>() : default; 
        public T7 Seventh => IsSeventh ? GetCoproductValue<T7>() : default; 
        public T8 Eighth => IsEighth ? GetCoproductValue<T8>() : default; 
        public T9 Ninth => IsNinth ? GetCoproductValue<T9>() : default; 

        public R Match<R>(
            Func<T1, R> ifFirst,
            Func<T2, R> ifSecond,
            Func<T3, R> ifThird,
            Func<T4, R> ifFourth,
            Func<T5, R> ifFifth,
            Func<T6, R> ifSixth,
            Func<T7, R> ifSeventh,
            Func<T8, R> ifEighth,
            Func<T9, R> ifNinth)
        {
            switch (Discriminator)
            {
                case 1: return ifFirst(First);
                case 2: return ifSecond(Second);
                case 3: return ifThird(Third);
                case 4: return ifFourth(Fourth);
                case 5: return ifFifth(Fifth);
                case 6: return ifSixth(Sixth);
                case 7: return ifSeventh(Seventh);
                case 8: return ifEighth(Eighth);
                case 9: return ifNinth(Ninth);
                default: return default;
            }
        }

        public void Switch(
            Action<T1> ifFirst = null,
            Action<T2> ifSecond = null,
            Action<T3> ifThird = null,
            Action<T4> ifFourth = null,
            Action<T5> ifFifth = null,
            Action<T6> ifSixth = null,
            Action<T7> ifSeventh = null,
            Action<T8> ifEighth = null,
            Action<T9> ifNinth = null)
        {
            switch (Discriminator)
            {
                case 1: ifFirst?.Invoke(First); break;
                case 2: ifSecond?.Invoke(Second); break;
                case 3: ifThird?.Invoke(Third); break;
                case 4: ifFourth?.Invoke(Fourth); break;
                case 5: ifFifth?.Invoke(Fifth); break;
                case 6: ifSixth?.Invoke(Sixth); break;
                case 7: ifSeventh?.Invoke(Seventh); break;
                case 8: ifEighth?.Invoke(Eighth); break;
                case 9: ifNinth?.Invoke(Ninth); break;
            }
        }
    }

}


