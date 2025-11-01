using UnityEngine;
using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class CmdTestSequence1 : Command
    {
        [InjectParameter] private int mInt = 0;

        public override void Execute()
        {
            Debug.Log($"{this} Execute called {mInt}");
            base.Execute();
        }
    }
}
