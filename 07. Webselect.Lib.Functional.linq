<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>Microsoft.Data.SqlClient</NuGetReference>
  <NuGetReference>Webselect.Lib.Functional</NuGetReference>
  <Namespace>Dapper</Namespace>
  <Namespace>Microsoft.Data.SqlClient</Namespace>
  <Namespace>static Webselect.Lib.Functional.Prelude</Namespace>
  <Namespace>unit = System.ValueTuple</Namespace>
  <Namespace>Webselect.Lib.Functional</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{
    // Webselect.Lib.Functional

    // Our functional library contains a few types and functions
    // which can be very helpful when modelling workflows.

    // Using statements
    
    // This library makes use of static using statements, which expose static methods as if
    // they were methods of the current class (so we can use Success() rather than Operation.Success())
    
    // using Webselect.Lib.Functional;
    // using static Webselect.Lib.Functional.Operation;
    // using static Webselect.Lib.Functional.Prelude;

    // Operation
    
    // An Operation is a value type which represents an action that can either succeed or fail. 
    // Success can be typed (return a result) or untyped (just return the outcome of the operation).
    
    Success().Dump("Success (untyped)");
    Success("OK").Dump("Success (typed)");
    
    // Failure must include some information about why the operation failed. This can be a
    
    Failure("IT BROKE").Dump("Basic failure");
    Failure(new Exception("NOPE")).Dump("Failure with Exception");
    Failure(new Error("OOPS")).Dump("Failure with custom Error");
    
    // Failures can also include a result, for example if you want to return the result state
    // as it was at the point of failure (for logging, perhaps)
    
    Failure(new State(), new Error("BOOM")).Dump("Failure with result type");
    
    // Finally, failures are also generically typed; this is so that you can use them in 
    // expressions without the compiler complaining about implicit type conversions
    
    var tmp = 1;
    
    var result = tmp == 0
        ? Failure<string>("FAILED")
        : Success("OK");
    
    // Note that we've omitted the Operation. prefix here; we can do this because there's
    // a "using static Webselect.Lib.Operation" in this script.
    
    // Errors
    
    // Error is a basic error type, with virtual properties which can be overridden.
    // The idea here is to create specific error types which accurately represent what has
    // actually gone wrong, and that include all necessary information about the specific error.
    
    Operation<unit> updateSomethingUsingApi(string apiKey)
    {
        try
        {
            // Try and do something
            var succeeded = true;
            
            return !succeeded
                ? Failure(new InvalidApiKeyError(apiKey))
                : Success();
        }
        catch (Exception ex)
        {
            return Failure(new ApiConnectionError(ex));
        }
    }

    // Try

    // The Try function wraps a non-Operational routine in an Operation; if everything succeeds
    // you'll get a Success containing the result, if it fails you'll get a Failure containing the exception.
    // This is useful for including existing or third-pary code in a pipeline which assumes everything returns 
    // an Operation.

    var op = Try(() => {
        // Do something which may throw an exception
        throw new Exception("THIS BROKE");
        return 100;
    });
    
    op.Dump("Try");

    // First and Then

    // These are used to compose a workflow by combining many individual steps, which all return either
    // Success or Failure. A Failure at any point means the rest of the steps are skipped.

    Operation<unit> successfulTask1() => Success();
    Operation<unit> successfulTask2() => Success();
    Operation<unit> failedTask() => Failure("TEST");
    Operation<unit> exceptionalTask() => Try(() => throw new Exception("EXCEPTION"));
    
    var op1 = First(successfulTask1).Then(successfulTask2);
    var op2 = First(failedTask).Then(successfulTask2);
    var op3 = First(successfulTask2).Then(exceptionalTask).Then(successfulTask2);
    
    op1.Dump("Pipeline (success)");
    op2.Dump("Pipeline (failure)");
    op3.Dump("Pipeline (exception)");
    
    // Workflows
    
    // So now, we can break our workflow up into nice, testable pieces,
    // then combine them in any way we need to.
    
    string connectionString = "DOESN'T WORK, JUST AN EXAMPLE";
    
    Operation<SqlConnection> connectToDatabase() => 
        Try(() => new SqlConnection(connectionString));

    Operation<(SqlConnection conn, Order order)> findOrder(SqlConnection conn, int orderID) =>
        Try(() => {
            var sql = "SELECT * FROM tOrder WHERE OrderID = @OrderID";
            return conn.QuerySingleOrDefault<Order>(sql, new { orderID });
        }).Then(order => order == null 
            ? Failure<(SqlConnection, Order)>($"Couldn't find order {orderID}") 
            : Success((conn, order)));

    Operation<T> closeConnectionAndReturn<T>((SqlConnection conn, T val) x) =>
        Try(() => x.conn.Close()).Then(() => Success(x.val));
        
        
    Operation<Order> getOrder(int orderID) =>
        First(connectToDatabase)
            .Then(conn => findOrder(conn, orderID))
            .Then(closeConnectionAndReturn);
}

public class ApiConnectionError : Error 
{
    public ApiConnectionError(Exception ex)
        : base($"Couldn't connect to API.", ex) {}
}

public class InvalidApiKeyError : Error 
{
    public InvalidApiKeyError(string apiKey)
        : base($"Invalid API key: {apiKey}.") => ApiKey = apiKey;

    public string ApiKey { get; }
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

public class State
{
}