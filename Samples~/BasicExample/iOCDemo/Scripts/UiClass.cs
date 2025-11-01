using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class UiClass : BaseBehaviour
    {
        [InjectSignal] private SigClickOkay SigClickOkay = null;
        [InjectSignal] private Sig1Arg mSig1Arg = null;
        [InjectSignal] private Sig2Arg mSig2Arg = null;
        [InjectSignal] private Sig3Arg mSig3Arg = null;
        [InjectSignal] private Sig4Arg mSig4Arg = null;
        [InjectSignal] private SigSequence mSigSequence = null;

        public void OnClickSend()
        {
            SigClickOkay.Dispatch();
            mSig1Arg.Dispatch(5);
            mSig2Arg.Dispatch(69, true);
            mSig3Arg.Dispatch(555, "Hello", 89898989);
            mSig4Arg.Dispatch(UnityEngine.Random.Range(1, 1000), 56.9f, true, "Last one");
            mSigSequence.Dispatch(56);
        }
    }
}
