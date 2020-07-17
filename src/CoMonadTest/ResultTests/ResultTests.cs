#nullable enable
using CoMonad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace CoMonadTest
{
    /// <summary>
    /// Basic tests only. For demo fo code mainly
    /// Code in production 1 year!
    /// </summary>
    [TestClass]
    public class ResultTests
    {


        [TestMethod]
        public void Nullable_Is_Ok_With_Null()
        {
            var result = Result.Ok<string?>(null);
            Assert.IsNull(result.Error);
        }
        [TestMethod]
        public void NonNullable_Complains_with_NUll()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type. //Remove this to see your squigly
            var result = Result.Ok<string>(null);//either 1. should see a squigly under null or 2. this will not compile (much preferable)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public void Implicit_Error_Conversion()
        {
            Result<int> result = RezErr.Overflow;
            Assert.IsNotNull(result.Error, "result.Error :: Failed IsNotNull assertion");
            Assert.AreEqual("Overflow", result.Error!.ToString());
        }

        [TestMethod]
        public void Map()
        {
            Assert.IsNull(Add(1, 2).Error, "Add(1, 2).Error :: Failed IsNull assertion");
            Result<int> r1 = Result.Ok(1);
            Assert.IsNull(r1.Error, "r1.Error :: Failed IsNull assertion");
            Result<int> rmax = Result.Ok(int.MaxValue);
            Assert.IsNull(rmax.Error, "rmax.Error  :: Failed IsNull assertion");

            Result<int> r2 = r1.Map(r => Plus(r, 1));
            Assert.IsNull(r2.Error, "r2.Error :: Failed IsNull assertion");
            Assert.AreEqual(2, r2.Value);

            Result<int> rerr2 = rmax.Map(r => Plus(r, 1));
            Assert.IsNotNull(rerr2.Error, "rerr2.Error :: Failed IsNotNull assertion");


        }
        [TestMethod]
        public void Bind()
        {

            Result<int> rmax = Result.Ok(int.MaxValue);
            var rerr = rmax.Bind(r => Add(r, 1));
            Assert.IsNotNull(rerr.Error, "result.Error :: Failed IsNotNull assertion");
            Assert.AreEqual("Overflow", rerr.Error!.ToString());
            Result<int> addiv = AddDivide(10, 20, 6);
            Assert.IsNull(addiv.Error, "addiv.Error  :: Failed IsNull assertion");
            Assert.AreEqual(5, addiv.Value);

            static Result<int> AddDivide(int a, int b, int c) => Add(a, b).Bind(r => Divide(r, c));
        }

        [TestMethod]
        public void Combine()
        {
            var r1 = Result.Ok(1);
            var r2 = Result.Ok(2);

            var tup = r1.Combine(r2);
            Assert.IsNull(tup.Error, "tup.Error :: Failed IsNull assertion");


            var r5 =r1.Combine(r => Add(r, 3))
                .Map(t=>t.Item1+t.Item2);

            Assert.IsNull(r5.Error, "r4.Error :: Failed IsNull assertion");

            Assert.AreEqual(5, r5.Value);

        }



        [TestMethod]
        public async Task Map_Async()
        {
            Result<int> r1 = Result.Ok(1);
            Result<int> rmax = Result.Ok(int.MaxValue);

            Result<int> r2 = await r1.MapAsync(r => PlusAsync(r, 1));
            Assert.IsNull(r2.Error, "r2.Error :: Failed IsNull assertion");
            Assert.AreEqual(2, r2.Value);

            Result<int> rerr2 = await rmax.MapAsync(r => PlusAsync(r, 1));
            Assert.IsNotNull(rerr2.Error, "rerr2.Error :: Failed IsNotNull assertion");

            var rerr3 =await  r1.MapAsync(r => DivAsync(r,0));
            Assert.IsNotNull(rerr3.Error, "rerr3.Error :: Failed IsNotNull assertion");



        }


        [TestMethod]
        public async Task Bind_Async()
        {

            Result<int> rmax = Result.Ok(int.MaxValue);


            var rerr = await rmax.BindAsync(r => AddAsync(r, 1));
            Assert.IsNotNull(rerr.Error, "result.Error :: Failed IsNotNull assertion");
            Assert.AreEqual("Overflow", rerr.Error!.ToString());


            Result<int> addiv = AddDivide(10, 20, 6);

            Assert.IsNull(addiv.Error, "addiv.Error  :: Failed IsNull assertion");
            Assert.AreEqual(5, addiv.Value);

            static Result<int> AddDivide(int a, int b, int c) => Add(a, b).Bind(r => Divide(r, c));
        }


        [TestMethod]
        public async Task Combine_Async()
        {
            var r1 = Result.Ok(1);
            var r2 = Result.Ok(2);


            var r5 =await r1.CombineAsync(r => AddAsync(r, 3))
                .MapAsync(t => t.Item1 + t.Item2);

            Assert.IsNull(r5.Error, "r4.Error :: Failed IsNull assertion");

            Assert.AreEqual(5, r5.Value);

        }
        //for test map catch
        static int Div(int a, int b)
        {
            if (b == 0)
            {
                throw new ArithmeticException("DivideByZero");
            }
            return (a / b);
        }
        static async Task<int> DivAsync(int a, int b)
        {
            await Task.Delay(1);
            if (b == 0)
            {
                throw new ArithmeticException("DivideByZero");
            }
            return (a / b);
        }
        static int Plus(int a, int b)
        {
            return checked(a + b);
        }
        static async Task<int> PlusAsync(int a, int b)
        {
            await Task.Delay(1);
            return checked(a + b);
        }
        static Result<int> Divide(int a, int b) => b == 0 ? RezErr.DivideByZero : Result.Ok(a / b);
        static async Task<Result<int>> DivideAsync(int a, int b)
        {
            await Task.Delay(1);
            return b == 0 ? RezErr.DivideByZero : Result.Ok(a / b);
        }

        //C# will not overflow unless using checked math
        static Result<int> Add(int a, int b)
        {
            try { return Result.Ok(checked(a + b)); } catch (System.OverflowException) { return RezErr.Overflow; }
        }
        static async Task<Result<int>> AddAsync(int a, int b)
        {
            await Task.Delay(1);
            try { return Result.Ok(checked(a + b)); } catch (System.OverflowException) { return RezErr.Overflow; }
        }

    }
}