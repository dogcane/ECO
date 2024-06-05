namespace ECO.UnitTests.Utils.Foos;

public class EntityFooOfInt : Entity<int>
{
    public EntityFooOfInt()
    {
    }

    public EntityFooOfInt(int identity) : base(identity)
    {
    }
}

public class AnotherEntityFooOfInt : Entity<int>
{
    public AnotherEntityFooOfInt()
    {
    }

    public AnotherEntityFooOfInt(int identity) : base(identity)
    {
    }
}

public class EntityFooOfString : Entity<string>
{
    public EntityFooOfString()
    {
    }

    public EntityFooOfString(string identity) : base(identity)
    {
    }
}

public class VersionableEntityFooOfInt : VersionableEntity<int>
{
    public VersionableEntityFooOfInt() : base()
    {
    }

    public VersionableEntityFooOfInt(int identity, int version) : base(identity, version)
    {
    }
}
