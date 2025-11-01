using System;

#pragma warning disable 649
namespace XcelerateGames.IOC
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectSignal : Attribute
    {
    }
}
