using System;
using System.Collections.Generic;
using UnityEngine;

namespace XcelerateGames.IOC
{
    public class SignalSequencer
    {
        #region Public properties
        public AbstractCommand OnFinish;
        public AbstractCommand OnFail;
        #endregion Public properties

        #region Private properties
        private bool mIsAborted = false;
        private bool mContinueOnAbort = false;
        private bool mExecuteInParallel = false;
        private bool mOnce = false;
        private bool mDoNotPool = false;
        private List<AbstractCommand> mCommands;
        private List<AbstractSignal> mSignals;
        public AbstractSignal mSignal;
        private int mIndex;
        private Dictionary<Type, object> mInjections = null;
        #endregion Private properties

        #region Public Methods
        public SignalSequencer()
        {
            mIndex = 0;
            mCommands = new List<AbstractCommand>();
            mSignals = new List<AbstractSignal>();
        }

        public void Execute(AbstractSignal signal, bool doNotPool)
        {
            mSignal = signal;
            mIndex = 0;
            mIsAborted = false;
            mDoNotPool = doNotPool;
            mInjections = new Dictionary<Type, object>(mSignal.InjectionBindings);
            if (mCommands.Count > 0)
            {
                if (mExecuteInParallel)
                    mCommands.ForEach((AbstractCommand command) => Execute(command, false));
                else
                    Execute(GetCommand(), true);
            }
            else
                DispatchSignals();
        }

        public void AddCommand(AbstractCommand command)
        {
            mCommands.Add(command);
        }

        public void RemoveCommand(Type type)
        {
            mCommands.RemoveAll(e=> e.GetType() == type);
        }

        public void AddSignal(AbstractSignal signal)
        {
            mSignals.Add(signal);
        }

        public void ContinueOnAbort()
        {
            mContinueOnAbort = true;
        }

        public void ExecuteInParallel()
        {
            mExecuteInParallel = true;
        }

        public void Once()
        {
            mOnce = true;
        }

        public SignalSequencer Clone()
        {
            SignalSequencer signalSequencer = new SignalSequencer();
            signalSequencer.mIndex = 0;
            signalSequencer.mCommands = new List<AbstractCommand>(mCommands);
            signalSequencer.mSignals = new List<AbstractSignal>(mSignals);

            signalSequencer.mIsAborted = false;
            signalSequencer.mContinueOnAbort = mContinueOnAbort;
            signalSequencer.mExecuteInParallel = mExecuteInParallel;
            signalSequencer.mOnce = mOnce;
            signalSequencer.mDoNotPool = mDoNotPool;
            if(OnFail != null)
                signalSequencer.OnFail = (AbstractCommand)System.Activator.CreateInstance(OnFail.GetType());
            if(OnFinish != null)
                signalSequencer.OnFinish = (AbstractCommand)System.Activator.CreateInstance(OnFinish.GetType());

            return signalSequencer;
        }

        public void DestroyCommands()
        {
            for(int i = 0; i < mCommands.Count; ++i)
            {
                mCommands[i] = null;
            }
        }
        #endregion Public Methods

        #region Private Methods
        private void SetBindings(Dictionary<Type, object> bindings)
        {
            for (int i = 0; i < mCommands.Count; ++i)
                InjectBindings.Inject(GetCommand(), bindings);
        }

        private void ExecuteNext()
        {
            mIndex++;
            if (mIndex < mCommands.Count)
            {
                Execute(GetCommand(), true);
            }
            else
            {
                Execute(OnFinish, false);
                OnAllExecutionsDone();
            }
        }

        //OnFinal & OnFail commands do not register listeners. Else we will go into recursive loop
        private void Execute(AbstractCommand command, bool addListeners)
        {
            if (command == null)
                return;
            if (addListeners)
            {
                //Add listeners
                command.OnAbort = OnCommandAborted;
                command.OnRelease = OnCommandReleased;
            }

            //Inject Models & Signals (if any)
            InjectBindings.Inject(command);

            //Inject params
            InjectBindings.InjectParameters(command, mInjections);

            //Now finally execute the command
            command.PerformExecution();
        }

        private void OnCommandReleased(AbstractCommand command)
        {
            //Remove listeners
            command.OnAbort = null;
            command.OnRelease = null;
            if (!mIsAborted)
                ExecuteNext();
        }

        private void OnCommandAborted(AbstractCommand command)
        {
            if (mContinueOnAbort)
                ExecuteNext();
            else
            {
                mIsAborted = true;
                Execute(OnFail, false);
                OnAllExecutionsDone();
            }
        }

        private void OnAllExecutionsDone()
        {
            DispatchSignals();
            mInjections?.Clear();
            mInjections = null;
            if (mOnce)
            {
                mCommands.Clear();
                mSignals.Clear();
                OnFail = OnFinish = null;
            }
        }

        private void DispatchSignals()
        {
            mSignals.ForEach((AbstractSignal obj) =>
            {
                if (obj is Signal)
                    (obj as Signal).Dispatch();
                else
                    Debug.LogError("Dispatching of Signals with arguments is not implemented");
            });
        }

        private AbstractCommand GetCommand()
        {
            return mDoNotPool ? (AbstractCommand)Activator.CreateInstance(mCommands[mIndex].GetType()) : mCommands[mIndex];
        }
        #endregion Private Methods
    }
}
