using System.Linq;
using UnityEngine;
using XcelerateGames.IOC;

namespace XcelerateGames
{
    /// <summary>
    /// Base class for all classes. If using IOC framwork, every class must derive from this class.
    /// It handles all dependecy injections
    /// </summary>
    public class BaseBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            InjectBindings.Inject(this);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            MonoBehaviour[] monoBehaviour = GetComponents<MonoBehaviour>();
            if(monoBehaviour.Length == 1 && gameObject.name == "GameObject")
            {
                gameObject.name = this.GetType().ToString().Split('.').Last();
            }
        }
#endif //UNITY_EDIOR
    }
}
