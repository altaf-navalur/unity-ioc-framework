using System;

#pragma warning disable 649
namespace XcelerateGames.IOC
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectMethod : Attribute
    {
    }
}
