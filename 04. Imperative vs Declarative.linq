<Query Kind="Program">
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{   
    // Imperative vs. Declarative programming
    
    var numbers = new List<int> { 10, 20, 30 };

    // We already do a lot of declarative (and indeed functional) programming 
    // when we use LINQ: you just may not recognise the term. SQL code is also 
    // (mostly) declarative (SELECT, ORDER BY etc). Loosely:
    
    // Imperative = code where we're writing instructions for the computer on *how* to do something
    
    var doubledImp = new List<int>();
    
    foreach (var n in numbers)
    {
        doubledImp.Add(n * 2);
    }

    // Declarative = code where we're telling the computer *what* we want and hiding the *how* (implementation details)
    
    // This is exactly what LINQ does. However, it's not magic! 
    // The complexity/loops etc still have to be implemented *somewhere*, but this way they aren't *re*-implemented at 
    // every single call-site, and can be heavily tested.
    
    // Select is also an example of a *higher-order function*, which is just a function which takes one or more 
    // other functions as parameters (the lambda expression "n => n * 2" is just a function).
    
    var doubledDec = numbers.Select(n => n * 2);
    
    // SuperAwesomeDeclarative (not an official term) = making it reusable and more readable, using C# local functions
    
    int numberTimesTwo(int i) => i * 2;
    
    var doubledSuper = numbers.Select(numberTimesTwo);
    
    doubledImp.Dump();
    doubledDec.Dump();
    doubledSuper.Dump();

    // Here's an (admittedly convoluted) example: getting a sorted list of animal names grouped by occurrence

    var animals = new List<string> { "Cat", "DogFish", "Fish", "Dog", "Cat", "Catfish", "Dog" };
    
    // Imperative:
    
    var grouped = new Dictionary<string, int>();
    
    foreach (var a in animals)
    {
        if (!grouped.ContainsKey(a)) { grouped.Add(a, 0); };
        grouped[a]++;
    }

    var sorted = grouped.Keys.ToList();

    bool madeChanges;
    int itemCount = sorted.Count;
    do
    {
        madeChanges = false;
        itemCount--;
        for (int i = 0; i < itemCount; i++)
        {
            if (sorted[i].CompareTo(sorted[i + 1]) > 0)
            {
                string temp = sorted[i + 1];
                sorted[i + 1] = sorted[i];
                sorted[i] = temp;
                madeChanges = true;
            }
        }
    } while (madeChanges);
    
    var output = new List<(string, int)>();
    
    foreach (var k in sorted)
    {
        output.Add((k, grouped[k]));
    }
    
    output.Dump();
    
    // Declarative
    
    animals
        .GroupBy(animalName => animalName)
        .OrderBy(group => group.Key)
        .Select(group => (group.Key, group.Count()))
        .ToList()
        .Dump();
}
