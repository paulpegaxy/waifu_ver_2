using System;

[AttributeUsage(AttributeTargets.Class)]
public class LogicExecutorAttribute : Attribute
{
    public object type;

    public LogicExecutorAttribute(object type)
    {
        this.type = type;
    }
}

