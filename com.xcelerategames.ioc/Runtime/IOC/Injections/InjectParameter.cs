using System;

namespace XcelerateGames.IOC
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectParameter : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class InjectParameterOptional : Attribute
    {
    }
}
