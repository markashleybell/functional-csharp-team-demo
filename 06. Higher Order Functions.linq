<Query Kind="Program">
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{
    // Higher Order Functions

    // Sound a bit intimidating; are actually just functions which take other functions as parameters.

    // Again, you can subsitute the word "method" for the word "function" anywhere here.
    
    // The main reason we've had to duplicate so much SelectCommerce code in the past
    // is because every client does everything slightly differently. 
    // So we can't split out most of the extremely complex common logic, just because there 
    // are often just one or two lines of code stuck right in the middle of it which differ.
    
    ClientSpecificFunctions.DoSomethingImportantToOrder(123);
    
    // But what if the bulk of that logic was in a common NuGet package, 
    // and just the client-specific logic was included in the project?

    void doClientSpecificThingsToOrder(Order order)
    {
        order.LinkedThings.Add(1);
        order.LinkedThings.Add(2);
    }
    
    CommonFunctions.DoSomethingImportantToOrder(123, doClientSpecificThingsToOrder);

    // That's a local function being passed in, but there's no reason it couldn't be a
    // method with the same signature...
    
    CommonFunctions.DoSomethingImportantToOrder(123, ClientSpecificFunctions.DoClientSpecificThingsToOrder);
    
    // It could also be a lambda expression; *these* *are* *all* *just* *functions*
    
    CommonFunctions.DoSomethingImportantToOrder(123, order => order.LinkedThings.Add(3));

    // NOTE: Yes, I know: shamefully, the functions we are passing in are not pure functions!
    
    // What I'm trying to show is how we could integrate this with existing code, as rewriting
    // everything functionally is obviously completely impractical.
    
    // This is an example of functional programming working *with* OOP, so we are calling methods 
    // of an immutable Order object which is passed to a function as a parameter
}

public static class ClientSpecificFunctions
{
    public static void DoSomethingImportantToOrder(int orderID)
    {
        // Load order
        var order = new Order(123);
        
        // Do tons of complicated stuff
        
        // Do a thing which only this client does
        
        // Update the order
    }
    
    public static void DoClientSpecificThingsToOrder(Order order)
    {
        order.LinkedThings.Add(1);
        order.LinkedThings.Add(2);
    }
}

public static class CommonFunctions 
{
    public static void DoSomethingImportantToOrder(int orderID, Action<Order> clientSpecificUpdates)
    {
        // Load order
        var order = new Order(123);

        // Do tons of complicated stuff

        // Do a thing which only this client does
        clientSpecificUpdates(order);

        // Update the order
    }
}

public class Order
{
    public Order(int id)
    {
        ID = id;
        LinkedThings = new List<int>();
    }
    
    public int ID { get; }
    
    public List<int> LinkedThings { get; }
}