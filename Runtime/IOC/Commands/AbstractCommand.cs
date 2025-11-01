using System;

namespace XcelerateGames.IOC
{
    public abstract class AbstractCommand
    {
        public Action<AbstractCommand> OnRelease;
        public Action<AbstractCommand> OnAbort;

        protected bool isReleased { get; private set; }
        protected bool isAborted { get; private set; }

        /// <summary>
        /// Sets the execution parameters.
        /// </summary>
        public virtual void SetParameters(params object[] parameters) { }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public virtual void PerformExecution()
        {
            Reset();
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public virtual void PerformRevertion()
        {
            Reset();
        }

        /// <summary>
        /// Call this when done executing or reverting over time to continue command chain.
        /// </summary>
        protected void Release()
        {
            if (isReleased) { return; }
            isReleased = true;
            if (OnRelease != null)
            {
                OnRelease(this);
            }
        }

        /// <summary>
        /// Call this method to stop the current command chain, release this command and potentially revert all released commands.
        /// </summary>
        protected void Abort()
        {
            if (isAborted) { return; }
            isAborted = true;
            if (OnAbort != null)
            {
                OnAbort(this);
            }
            Release();
        }

        private void Reset()
        {
            isReleased = false;
            isAborted = false;
        }

    }

}