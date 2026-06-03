<Query Kind="Program">
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{   
    // Immutability
    
    // If types are mutable, you have *no way* of preventing objects being created
    // in invalid states or having their properties set to invalid values. 
    
    // On top of that, you can't tell whether a mutable object is manipulated by a 
    // method just by looking at the method signature, so you have no idea what state 
    // it's in after the method has been called (unless you read the whole body of the 
    // method, and probably also step through it to see what it does...)
    
    var mutable = new MutableType { ID = 100, Name = "MUTABLE" };
    
    Functions.SomethingUsingMutableType(mutable);
    
    // Without looking at the method body: what state is mutable in now?
    
    var immutable = new ImmutableType(id: 100, name: "IMMUTABLE");
    
    Functions.SomethingUsingImmutableType(immutable);
 
    // ImmutableType has no public setters, so it *cannot* have been changed.
    // If we *do* want to change it, we'll have to return a new copy from the method
    
    var immutableUpdated = Functions.UpdateName(immutable, "NEW NAME");
    
    immutableUpdated.Dump();

    // Immutability does make some things more difficult in OOP, like updating a single property value
    // 'Real' functional languages have constructs to help with this, but in C# we
    // have to write code to handle it. This is normally in the form of a With method,
    // which gives us a *copy* of the current object instance with the updated property.

    var updatedName = immutable.With(name: "UPDATED");
    
    immutable.Dump();
    updatedName.Dump();
}

public class MutableType
{
    public int ID { get; set; }
    
    public string Name { get; set; }
}

public class ImmutableType
{
    public ImmutableType(int id, string name)
    {
        ID = id;
        Name = name;
    }
    
    public int ID { get; }
    
    public string Name { get; }
    
    public ImmutableType With(string name = null)
    {
        return new ImmutableType(ID, name ?? Name);
    }
}

public static class Functions
{
    public static void SomethingUsingMutableType(MutableType m) { }
    
    public static void SomethingUsingImmutableType(ImmutableType m) { }
    
    public static ImmutableType UpdateName(ImmutableType m, string name) =>
        new ImmutableType(m.ID, name);
}