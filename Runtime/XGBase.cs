using XcelerateGames.IOC;

namespace XcelerateGames
{
    public class XGBase
    {
        public XGBase()
        {
            InjectBindings.Inject(this);
        }
    }
}
