using UnityEngine;
using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class CmdTestSequence2 : Command
    {
        [InjectParameter] private int mInt = 0;

        public override void Execute()
        {
            Debug.Log($"{this} Execute called {mInt}");
            if (Random.Range(0, 100) > 50)
                Abort();
            else
                Release();
        }
    }
}
