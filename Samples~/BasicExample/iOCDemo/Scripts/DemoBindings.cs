using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class DemoBindings : BindingManager
    {
        protected override void SetBindings()
        {
            base.SetBindings();

            BindSignal<SigClickOkay>();
            BindSignal<Sig1Arg>();
            BindSignal<Sig2Arg>();
            BindSignal<Sig3Arg>();
            BindSignal<Sig4Arg>();
            BindSignal<SigSequence>();
        }

        protected override void SetFlow()
        {
            base.SetFlow();

            On<Sig1Arg>().Do<CmdTest1Arg>();
            On<Sig2Arg>().Do<CmdTest2Arg>();
            On<Sig3Arg>().Do<CmdTest3Arg>();
            On<Sig4Arg>().Do<CmdTest4Arg>().OnFinish<CmdTestFinal>();
            On<SigSequence>().Do<CmdTestSequence1>().Do<CmdTestSequence2>().Do<CmdTestSequence3>();
        }
    }
}
