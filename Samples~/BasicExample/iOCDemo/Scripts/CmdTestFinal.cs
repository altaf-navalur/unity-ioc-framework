using UnityEngine;
using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class CmdTestFinal : Command
    {
        public override void Execute()
        {
            Debug.Log($"CmdTestFinal called {this} {Time.frameCount}");
            base.Execute();
        }
    }
}
