using UnityEngine.UI;
using XcelerateGames.IOC;

namespace XcelerateGames.IOCDemo
{
    public class UiListenerClass : BaseBehaviour
    {
        public Text _Text;
        [InjectSignal] private SigClickOkay mSigClickOkay = null;

        int mCount = 0;
        // Start is called before the first frame update
        void Start()
        {
            mSigClickOkay.AddListener(OnClickedOkay);
        }

        private void OnClickedOkay()
        {
            mCount++;
            _Text.text = "Count : " + mCount;
        }
    }
}
