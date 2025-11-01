namespace XcelerateGames.IOC
{
    public interface ICommandBinder
    {
        ICommandBinder Do<T>(params object[] executionParameters) where T : AbstractCommand, new();
        ICommandBinder Undo<T>() where T : AbstractCommand, new();
        ICommandBinder Dispatch<T>() where T : Signal, new();
        //This command will be executed if any command fails to execute
        ICommandBinder OnAbort<T>() where T : Command, new();
        //This command will be executed after all commands are successfully executed.
        ICommandBinder OnFinish<T>() where T : Command, new();
        //By default, all Commands that are to be excuted in sequence are aborted if any one command fails. If ContinueOnAbort is set, the commands are executed as if no command failed
        ICommandBinder ContinueOnAbort();
        //By default, all commands are executed in sequence. Calling this function will execute all commands in same frame.
        //If execute in parallel is set, then OnAbort & OnFinal will not be called
        ICommandBinder ExecuteParallel();
        //By default, all commands are executed everytime a signal is fired. Calling this function will execute all commands only once.
        ICommandBinder Once();
        ICommandBinder DoNotPool();
        ICommandBinder Mute<T>(params object[] executionParameters) where T : AbstractSignal, new();
        ICommandBinder UnMute<T>(params object[] executionParameters) where T : AbstractSignal, new();
        T GetSignal<T>() where T : AbstractSignal;
        T BindSignal<T>() where T : AbstractSignal, new();
    }
}