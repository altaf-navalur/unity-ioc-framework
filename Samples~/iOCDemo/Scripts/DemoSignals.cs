using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class SigClickOkay : Signal { }
    public class Sig1Arg : Signal<int> { }
    public class Sig2Arg : Signal<int, bool> { }
    public class Sig3Arg : Signal<int, string, long> { }
    public class Sig4Arg : Signal<int, float, bool, string> { }
    public class SigSequence : Signal<int> { }
}
