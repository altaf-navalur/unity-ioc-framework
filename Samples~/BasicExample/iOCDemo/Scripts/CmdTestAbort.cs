using UnityEngine;
using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class CmdTestAbort : Command
    {
        public override void Execute()
        {
            Debug.Log($"CmdTestAbort called {this} {Time.frameCount}");
            base.Execute();
        }
    }
}
