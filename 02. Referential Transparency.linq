<Query Kind="Program">
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{   
    // Terms
    
    // You can subsitute the word "method" for the word "function" anywhere here;
    // methods are just functions which are attached to types.
    
    // I'll variously use "enviroment", "world" and "program state" to describe the
    // current state of the application environment.
    
    
    // Referential Transparency, or "Pure" Functions
    
    // An expression is said to be referentially transparent if it can be replaced 
    // with its corresponding return value without changing the program’s behaviour.
    
    // These functions are pure:
    int add(int a, int b) => a + b;
    int multiply(int a, int b) => a * b;
    
    add(2, multiply(3, 4)).Dump("Result if we pass multiply function");
    add(2, 12).Dump("Result if we pass the equivalent return value");

    // Pure functions are easily testable, because the output for any given input will *always
    // be the same*, no matter how many times the function is called or what the state of 
    // the world/environment is.

    // One other advantage of pure functions is that they can be memoized; this means that the
    // runtime can cache the result and use it again wherever that function is called with the
    // same parameters.


    // "Impure" functions

    // If a function body relies on a value from outside its scope (i.e. not declared
    // in the scope or passed in as a parameter), it is no longer considered pure.
    // We can't tell what it will return unless we also know the complete state of 
    // the environment it's being run in.
    
    // This makes testing very difficult, or in some cases (like the example below) impossible!
    
    // These just write out milliseconds
    string getTimeStampImpure() => DateTime.Now.ToString("fff");
    string getTimeStampPure(DateTime dt) => dt.ToString("fff");
    
    // What will the expected result be when you call this?
    getTimeStampImpure().Dump("getTimeStampImpure call 1");
    Thread.Sleep(250);
    // And now?
    getTimeStampImpure().Dump("getTimeStampImpure call 2");
    
    var now = new DateTime(2018, 1, 23, 6, 4, 1, 100);
    
    // We know exactly what this will return every time, no matter what the outside world is doing.
    getTimeStampPure(now).Dump("getTimeStampPure call 1");
    Thread.Sleep(250);
    getTimeStampPure(now).Dump("getTimeStampPure call 2");
    
    // If we need the current time, just pass it in:
    getTimeStampPure(DateTime.Now).Dump("getTimeStampPure call 3");

    // Similarly: if a function causes side effects, it is no longer pure. 
    // If we substitute its return value, then the side effects won't occur, 
    // hence the resulting program state is different.
    
    int globalCount = 0;

    int multiplyImpure(int a, int b)
    {
        globalCount++;
        return a * b;
    }

    add(2, multiplyImpure(2, 5)).Dump("globalCount was incremented and is now " + globalCount);
    add(2, 10).Dump("globalCount wasn't incremented and is still " + globalCount);
}