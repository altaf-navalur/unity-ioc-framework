using UnityEngine;
using XcelerateGames.IOC;

//Demo1
namespace XcelerateGames.IOCDemo
{
    public class CmdTest0Arg : Command
    {
        public override void Execute()
        {
            Debug.LogErrorFormat("CmdTest0Arg Execute called");
            base.Execute();
        }
    }
}
