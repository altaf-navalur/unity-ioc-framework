using UnityEngine;

namespace XcelerateGames.IOC
{
    public class Command : AbstractCommand
    {
        public override sealed void PerformExecution()
        {
            base.PerformExecution();
            Execute();
        }

        //In derived class make sure to call this funcion at the very end or call Release manually
        public virtual void Execute()
        {
            Release();
        }
    }
}
