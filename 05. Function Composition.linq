<Query Kind="Program">
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{
    // Composing functions

    // When you write pure functions, they will often have several parameters.

    // Most of the time, more than three or four parameters means your function is trying to do too much...

    // But sometimes, you know that every call to a function will require that you pass the same
    // value for one of it's parameters (like an app-specific constant or configuration value).

    const string App_Name = "TestApp";

    // Assume that this function comes from a common library
    string getCopyrightNotice(string appName, DateTime date) => $"© {appName} {date.Year}";
    
    getCopyrightNotice(App_Name, DateTime.Now).Dump();
    
    // Now, within our app, we will never want the appName to be anything other than
    // the name of our app... so we compose a new function which "bakes in" the name
    // and only takes one parameter (the date).
    
    string getTestAppCopyrightNotice(DateTime date) =>
        getCopyrightNotice(App_Name, date);
        
    getTestAppCopyrightNotice(DateTime.Now).Dump();
        
    // It may initially seem like this function isn't pure, because it uses a global variable;
    // however, it *will* always return the same result given the same input, unless the constant
    // is changed and the app is recompiled (in which case any tests that relied on the old
    // constant value will fail).
    
    // Function composition is very useful when you are trying to "fit" the inputs and outputs
    // of functions together, for example in a pipeline of operations (sse 07. WebselectLib.Functional).
}