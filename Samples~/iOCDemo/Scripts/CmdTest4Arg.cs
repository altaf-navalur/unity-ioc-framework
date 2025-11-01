using UnityEngine;
using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class CmdTest4Arg : Command
    {
        [InjectParameter] private int mInt = 0;
        [InjectParameter] private float mFloat = 0;
        [InjectParameter] private bool mBool = false;
        [InjectParameter] private string mString = null;

        public override void Execute()
        {
            Debug.Log($"CmdTest4Arg Execute called {mInt}, {mFloat}, {mBool}, {mString} {Time.frameCount}");
            base.Execute();
        }
    }
}
