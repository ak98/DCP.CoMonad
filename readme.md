# DCP.CoMonad
![lang-ext](https://github.com/ak98/DCP.CoMonad/blob/master/comonadlogo256.png?raw=true)

C# Functional Extensions for the 'Happy Path' 
=============================================

Combinatorial Monads for __Result, Task, ValueTask, Linq and IAsyncEnumerable__. 

Inspired by __Scott Wlaschin__ and railway oriented program design principles.

Also includes __Discriminated Union__ (Sum Type) to assist with Domain design.

Uses latest features of C# to adopt new paradigms based on functional monadic design. Retains minimalist design. Easy learning and migration curve.

Differs from other functional libraries in succinctness without loss of power.

## Index

* [Functional](#functional)
* [Introduction](#introduction)
* [No Exception](#no-exceptions)
* [No Option](#no-option)
* [No Complexity](#no-complexity)
* [Failure!](#failure)
* [Worlds Collide](#worlds-collide)
* [Unionize]{#unionize}
* [Monads All the Way](#monads-all-the-way)
* [Reference Videos by others](#reference-videos-by-others)


## Functional

__What is functional programming anyway?__

1. Style

    A __style__ that treats computation as the evaluation of mathematical functions and avoids changing-state and mutable data.
    
    Because it is just a __style__ there is no requirement to use a 'functional language', although these may have defaults to immutability and even compiler restrictions that simplify or enforce the __style__.
    
    The terms 'Pure', 'deterministic', 'referential transparency' are fancy descriptors for this style.

2. First Class Functions and Higher-order functions

    An __ability__ to pass and receive functions as parameters.
    
    .Net and indeed most languages possess this ability. Func&lt;T..n&gt; and delegates represent this ability in .Net
    
The important thing to note is that .Net (C# in particular) is fully capable of functional programming style and technique.



## Introduction ##

Functional programming style primary benefit is enhancing code readability and ability to reason about.

The composition throughout our programs is really important. We need to be confident that each piece of code and the composition of code is correct.

Having one small piece of code that is easy to read and reason about is very easy to achieve in practically any dynamic. 

Dealing with large code bases maintained by multiple developers is intrinsically more difficult.

Functional style shines 😊 in this regard.


## No Exceptions ##

Using the Result&lt;T&gt; struct type. This type represents a discriminated union of either success or error and allows us to adopt railway oriented programming. Chain function calls and reduce code paths to a single railway track. 

Result&lt;T&gt; implements Map, Bind, Combine and their async counter-parts

With these few methods we can transform code to more functional happy paths.

eg Checked Math simple example

```C#
   static Result<int> Divide(int a,int b)=>b==0?RezErr.DivideByZero:Result.Ok(a/b);
    //C# will not overflow unless using checked math
    static Result<int> Add(int a, int b)
    {
        try { return Result.Ok(checked(a + b)); } catch (System.OverflowException) { return RezErr.Overflow; }
    }

   static Result<int> AddDivide(int a,int b,int c)=> Add(a,b).Bind(r=>Divide(r,c));
```

The key thing to recognise is that this code will not throw errors yet will performed checked maths.

Indeed the primary requirement for this design style is that ___'All methods returning Result&lt;T&gt; do not throw errors'___.

Once this requirement is in place we can remove all error handling of exceptions when calling these methods and simply handle the end case result no matter how long the function call chain.

Transitioning to returning Result&lt;T&gt; from functions is simple to implement and peppers your code with ability to chain function calls using the map and bind.

Once using this mechanism with chained function calls your ability to reason about code and follow the code path is enhanced.

Scott Wlaschin describes this as moving 'up' into a new parallel world which is a nice concept.


## No Option ##

What! No Option or Maybe.

This library contains no option or maybe. This is a design decision to enforce transition to use of nullable reference types.

Nullable reference types in effect make the option or maybe types irrelevant. Of course this requires implementing nullable reference types correctly

## No Complexity

Well... Not Really.

This library only requires you to master a simple paradigm.

* Return Result&lt;T&gt; type from your functions. 

* Enforce the rule - 'no exceptions' thrown from these methods.

* Use Map, Bind, Combine, Tee and their async counter-parts to combine your function calls in call chains.


### Map

![Map](images/map.png)

### Bind

 ![Bind](images/bind.png)

### Tee

**Action and return in C#**

![Tee](images/tee.png)

### Combine

Experience over the last year indicated scenarios where this method to combine results into a tuple to be extremely useful.

Eg when T1 and T2 were parameters to a subsequent chained method call.

This avoids the need for paramaterised variations of Map and Bind.

![Combine](images/combine.png)





## Failure

How to indicate failure pathway? There are many other 'functional' libraries with their own methodologies and no standardized way.

This library uses a simple construct that can encapsulate a string and / or an exception. 

You can create your own class for Error handling easily - just derive from the abstract RezErrorBase class.

To return an error, simply return RezErrBase eg RezErr.OverThrow which is implicitly converted to the appropriate Result&lt;T&gt;

Another decision that was made - the Error property is not wrapped with boolean __IsFailure__ etc

This enables compiler checks which results in safer code. The compiler cant help you if you obfuscate the Error from the compiler.

```C#

    if(r.Error is null)
    {
        ...
    } 
    if(r.Error is {})
    {
        string err = r.Error.ToString();//Compiler sees Error not null 😀😀
    }

```

## Worlds Collide

Handling transitions from values to Result

```C#
    var r = Result.Ok("Hello World");
```

Handling transitions from sync to async

```C#
    public async Task<Result<string>> GetWebPage(Uri) {.......}
    public Result<Uri> GetUri(string url) { ......}
     public Result<int> ProcessPage(string content) { ......}
    //within an async method
    var r = GetUri(uri)//in the world of Result now
                .BindAsync(GetWebPage) //in the world of async Result now
                .BindAsync(ProcessPage);//remain in the async Result world
    //Question what is r
    //Answer : Task<Result<string>>
    //no async code has executed - page not retrieved and not processed - C# has become Lazy
    var finalresult = await r;
```

__Complex examples from production code__

```C#
// Azure Function Demonstrates a chain of predictable code that does not stray from functional principles and is predictable, easy to read and easy to reason about
// No complex decision branching - just one path
// Intrinsic error Handling
    public static class Retriever {
        [FunctionName("Retrieve")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Retrieve/{cid}/{itemid}")] HttpRequest req
            , string cid
            , string itemid
           , ILogger log) {
            log = new LoggerWrap(log);

            var rez = await req
                    .FailBadRequest(string.IsNullOrEmpty(itemid) || string.IsNullOrEmpty(cid))
                    .Bind(r => r.VerifyRSAHandshakeResult())
                    .Bind(_ => CosmosDB.GetStreamService(cid))
                    .MapAsync(c=> c.ReadItemStreamAsync(itemid))
                     .BindAsync(crm => crm.ToRsaStreamResult());

            return rez.Terminate(log);

        }

```
__IAsyncEnumerable__

```C#
      //  IAsyncEnumerable available from DotNet Framework 4.5 with Dasync.Collections
       private IAsyncEnumerable<Result<CardIdInfo>> GetCardInfos(Campaign campaign)
        {
            return campaign
                 .Query<Campaign.CampaignCard>()
                 .ListAsync()  //IAsyncEnumerable<CampaignCard>
                 .BindAsync(Convert);

            static async Task<Result<CardIdInfo>> Convert(CampaignCard card)//local static async function
            {
                return await RestV2
                    .Restutil
                    .PostAsync<CardInfo>(CardFunctionUrls.CardInfoFunction(), card.CardJson)
                    .MapAsync(ci => new CardIdInfo(card.id, ci));

            }
        }


```

## Unionize


Discriminated Unions are explained in Scotts Functional Designs talks on Domain modelling

This library includes a discriminated Union (DUnion) type 



```C#
        [TestMethod]
        public void Payment_Model()  //# Scott Wlaschin Domain modelling https://youtu.be/PLFl95c-IiU?t=1169
        {
            Result<Payment> payment = Cash.Create(42)
                .Map(c => new PaymentMethod(c))
                .Combine(pm => PaymentAmount.Create(42))
                .Map(tup => new Payment() { Amount = tup.Item2, Method = tup.Item1, Currency = Currency.USD });
        }
        //# Model classes
        class MasterCard
        {
            public CardNumber CardNumber { get; set; }
        }
        class Visacard
        {
            public CardNumber CardNumber { get; set; }
        }
        class Cheque
        {
            public CheckNumber CheckNumber { get; set; }
        }
        class CreditCardInfo : DUnion2<MasterCard, Visacard>//? Discriminated Union 'Or Type'
        {
            public CreditCardInfo(MasterCard mc) : base(mc) { }
            public CreditCardInfo(Visacard visa) : base(visa) { }
        }
        class PaymentMethod : DUnion3<Cash, Cheque, CreditCardInfo>//? Discriminated Union 'Or Type'
        {
            public PaymentMethod(CreditCardInfo card) : base(card) { }
            public PaymentMethod(Cheque cheque) : base(cheque) { }
            public PaymentMethod(Cash cash) : base(cash) { }
        }
        class Payment
        {
            public PaymentAmount Amount { get; set; }
            public Currency Currency { get; set; }
            public PaymentMethod Method { get; set; }
        }


```



## Code of interest

Scott does a nice example for a card game design in F#

It is possible to do similar in less lines of code in c#
This is runnable - hence the extra lines of code - not just a design see [Domain_Modelling_CardGame_Scott_Wlaschin.cs](https://github.com/ak98/DCP.CoMonad/blob/master/src/CoMonadTest/Examples%20of%20Interest/Domain_Modelling_CardGame_Scott_Wlaschin.cs)

```C#
using Card = System.ValueTuple<Rank, Suit>
static List<Card> OpenNewDeckOfCards()
        {
            var deck = new List<Card>();
            //Fill Deck
            for (Suit i = Suit.Heart; i <= Suit.Club; i++)
            {
                for (Rank j = Rank.Two; j <= Rank.Ace; j++)
                {
                    deck.Add((j,i));
                }
            }
            return deck;
        }
        static ImmutableStack<Card> Shuffle(List<Card> deck)
        {
            var array = deck.ToArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n);
                n--;
                Card temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
            var stack = ImmutableStack<Card>.Empty;
            foreach (var item in array)
            {
                stack = stack.Push(item);
            }
            return stack;
        }
        static void ShowHand((string name, ImmutableList<Card> hand) player)
        {
            Debug.WriteLine($"Player '{player.name}' has these cards.");
            foreach (var item in player.hand.OrderBy(cc => cc.Item1).ThenBy(c2 => c2.Item2))
            {
                Debug.WriteLine(item);
            }
            foreach (var g in player.hand.GroupBy(crd => crd.Item1).Where(z => z.Count() > 1))
            {

                Debug.WriteLine($"Player '{player.name}' has {g.Count()} {g.Key}'s. What a WINNER!");

            }
        }

            List<Card> deck = OpenNewDeckOfCards();//? 3
            ImmutableStack<Card> shuffledDeck = Shuffle(deck);
            var player = (name: "Hard Luck", hand: ImmutableList<Card>.Empty);
            for (int i = 0; i < 5; i++)//? 6
            {
                shuffledDeck = shuffledDeck.Pop(out Card card); //deal
                player.hand = player.hand.Add(card);//pickup
            }
            ShowHand(player);
        




``` 


## Monads all the way ##

![Monad](images/monoid.png)

[Read the basics](https://github.com/ak98/DCP.CoMonad/blob/master/monads.md)



## Reference Videos by others

[Functional Design Patterns - Scott Wlaschin](https://www.youtube.com/watch?v=srQt1NAHYC0&t=2705s) The original.


[Railway-Oriented Programming in C# - Marcus Denny](https://www.youtube.com/watch?v=uM906cqdFWE)

[The Power of Composition - Scott Wlaschin](https://www.youtube.com/watch?v=vDe-4o8Uwl8&t=2980s)

[Scott Wlaschin Domain modelling](https://youtu.be/PLFl95c-IiU?t=716) 

[Vladimir Khomrikov on Pluralsight](https://app.pluralsight.com/library/courses/csharp-applying-functional-principles/table-of-contents)






