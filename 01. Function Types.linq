<Query Kind="Program">
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{   
    // Function Types
    
    // There are many useful functional types which we can use alongside OOP.
    
    // The basic Func<> and Action<> types are just aliases for C# delegates, 
    // which enable us to pass functions around as objects. If you haven't used delegates, 
    // just substitute the word "callbacks" and you're on (mostly) the right track.
    
    // They look a bit weird, but once you get used to them they do make sense! The function bodies are just
    // lambda expressions, like you would pass to a LINQ method (items.Select(i => i.ID > 100)).
    
    // A function which takes two integer parameters and returns an integer result
    Func<int, int, int> add = (a, b) => a + b;

    // A string and an int param, returns a string (the last type in the generic definition is always the return type)
    Func<string, int, string> format = (s, i) => $"{s}: {i}";

    // Action is a function which returns void, so all the types are parameters
    Action<int, string> stuff = (i, s) => Console.Write($"{i} - {s}");

    // So, looking at our add() function above, we could also write it as:

    int add2(int a, int b) 
    {
        return a + b;
    };
    
    // Or:
    
    int add3(int a, int b) => a + b;
    
    // And thanks to C# Local Functions, we actually can! (I've had to make    
    // the names different just so the LINQPad compiler doesn't moan).
    
    // A method also 'counts' as a function, so you can pass one in anywhere the signatures match.
    
    // So, examples (obviously you would never actually do this because it's pointless, but I'm trying to keep things simple):
    
    int getAdditionResult(Func<int, int, int> addFunction, int a, int b)
    {
        return addFunction(a, b);
    }
    
    getAdditionResult(add, 1, 2).Dump();
    getAdditionResult(add2, 1, 2).Dump();
    getAdditionResult(add3, 1, 2).Dump();
    getAdditionResult(Functions.Add, 1, 2).Dump();
    getAdditionResult((a, b) => a + b, 1, 2).Dump();
}

public static class Functions
{
    public static int Add(int a, int b) => a + b;
}