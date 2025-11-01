using UnityEngine;
using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class CmdTest3Arg : Command
    {
        [InjectParameter] private int mInt = 0;
        [InjectParameter] private string mString = null;
        [InjectParameter] private long mLong = 0;

        public override void Execute()
        {
            Debug.Log($"CmdTest3Arg Execute called {mInt}, {mString}, {mLong}");
            base.Execute();
        }
    }
}
