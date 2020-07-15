# DCP.CoMonad
![lang-ext](https://github.com/ak98/DCP.CoMonad/blob/master/comonadlogo256.png?raw=true)

Minimal C# Functional Extensions for the 'Happy Path' 
=============================================

## Index

* [Functional](#functional)
* [Introduction](#introduction)
* [Monads](#monads)

## Functional

__What is functional programming anyway?__

1. Style

    A __style__ that treats computation as the evaluation of mathematical functions and avoids changing-state and mutable data.
    
    Because it is just a __style__ there is no requirement to use a 'functional language', although these may have defaults to immutability and even compiler restrictions that simplify or enforce the __style__.
    
    The terms 'Pure', 'deterministic', 'referential transparency' are fancy descriptors for this style.

2. First Class Functions and Higher-order functions

    An __ability__ to pass and recieve functions as parameters.
    
    .Net and indeed most languages possess this ability. Func&lt;T..n&gt; and delegates represent this ability in .Net
    
The important thing to note is that .Net (C# in particular) is fully capable of functional programming style and technique.




## Introduction

Inspired by Scott Wlaschin and and railway oriented program design principles, with the requirement to eradicate exceptions from code paths we focus on using functional mechanisms.

The latest features of C sharp enable us to adopt new paradigms based on functional monadic design.

__Eradicate Exceptions__

Using the Result&lt;T&gt; class to represent the discriminated union of either result or error allows us to adopt railway oriented programming, chaining function calls and reducing code paths to a single railway track. 

Result&lt;T&gt; implements Map, Bind, MapAsync, BindAsync

With these few methods we can transform code to more functional happy paths.

eg Checked Math simple example

```C#
    Result<int> Divide(int a,int b)=>b==0?RezErr.DivideByZero:Result.Ok(a/b);
    //C# will not overflow unless using checked math
    Result<int> Add(int a,int b)=>b==try{return Result.Ok(checked(a+b));catch(System.OverflowException){return RezErr.Overflow;}

    Result<int> AddDivide(int a,int b,int c)=> Add(a,b).Bind(r=>Divide(r,c));
```

The key thing to recognise is that this code will not throw errors yet will performed checked maths.

Indeed the primary requirement is that all methods that return Result&lt;T&gt; will not throw errors.

Once this requirement is in place we can remove all error handling of exceptions when calling these methods and simply handle the end case result no matter how long the function call chain.

__New World__

Transitioning to returning Result&lt;T&gt; from functions is simple to implement and peppers your code with ability to chain function calls using the map and bind.

Once using this mechanism with chained function calls your ability to reason about code and follow the code path is enhanced.

Scott Wlaschin describes this as moving into a new parallel world which is a nice concept.

__What! No Option or Maybe__

This library contains no option or maybe. This is a design decision to transition to use of nullable reference types.

Nullable reference types in effect make the option or maybe types irrelevant. Of course this requires implementing nullable reference types correctly

## Monads ##

A Monad is 'a box of something'. 






# ROP Development ideas

## Railway oriented programming

[Functional Design Patterns - Scott Wlaschin](https://www.youtube.com/watch?v=srQt1NAHYC0&t=2705s) The original.


[Railway-Oriented Programming in C# - Marcus Denny](https://www.youtube.com/watch?v=uM906cqdFWE)

[The Power of Composition - Scott Wlaschin](https://www.youtube.com/watch?v=vDe-4o8Uwl8&t=2980s)

[Vladimir Khomrikov on Pluralsight](https://app.pluralsight.com/library/courses/csharp-applying-functional-principles/table-of-contents)


### Map

**Select in C#**

![Map](images/map.png)

### Bind

**SelectMany in C#**

![Bind](images/bind.png)

### Tee

**Action and return in C#**

![Tee](images/tee.png)



# ![Monoid](images/monoid.PNG) Monadic Theory 



