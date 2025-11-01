using System;
using System.Collections.Generic;
using UnityEngine;

namespace XcelerateGames.IOC
{
    public class BindingManager : MonoBehaviour
    {
        [SerializeField] bool _DontDestroyOnLoad = false;
        public static BindingManager Instance { get; protected set; }
        public Dictionary<Type, object> _Bindings
        {
            get
            {
                return mCommandBinder._Bindings;
            }
        }

        CommandBinder mCommandBinder = null;

        #region Signal Bindings
        /// <summary>
        /// Returns a reference to signal by class name. The class name must be a fully qualified name: namespace.classname
        /// EX: XcelerateGames.SigEngineReady
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeName"></param>
        /// <returns>Instance of the signal</returns>
        public virtual T GetSignal<T>(string typeName) where T : class
        {
            Type type = Type.GetType(typeName);
            if (type != null)
            {
                if (_Bindings.ContainsKey(type))
                    return _Bindings[type] as T;
            }
            return null;
        }

        //public static T GetSignal<T>() where T : class
        //{
        //    Type type = typeof(T);
        //    if (_Bindings.ContainsKey(type))
        //        return _Bindings[type] as T;
        //    return default(T);
        //}

        //public static T GetModel<T>() where T : class
        //{
        //    Type type = typeof(T);
        //    if (_Bindings.ContainsKey(type))
        //        return _Bindings[type] as T;
        //    return default(T);
        //}

        /// <summary>
        /// Binds the signal.
        /// </summary>
        /// <returns>The signal.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T BindSignal<T>() where T : AbstractSignal, new()
        {
            return mCommandBinder.BindSignal<T>();
        }
        #endregion Signal Bindings

        #region Model Bindings
        /// <summary>
        /// Bind this instance.
        /// </summary>
        /// <returns>The bind.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T BindModel<T>() where T : XGBase, new()
        {
            return mCommandBinder.BindModel<T>();
        }
        #endregion Model Bindings

        #region Command Bindings

        protected ICommandBinder On<T>() where T : AbstractSignal
        {
            return mCommandBinder.On<T>();
        }

        public ICommandBinder Do<T>() where T : AbstractCommand, new()
        {
            return mCommandBinder.Do<T>();
        }

        #endregion Command Bindings

        protected virtual void Awake()
        {
            Instance = this;
            mCommandBinder = new CommandBinder();
            //Set all bindings
            SetBindings();
            //All bindings complete, call binding complete function
            OnBindingsComplete();
            //Set the flow of signal to command(s)
            SetFlow();
            //This must be called at the end 
            UpdateBindings();
            if (_DontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        //This function must be called at the end 
        private void UpdateBindings()
        {
            mCommandBinder.AddListeners();
        }

        protected virtual void SetBindings()
        {
        }

        protected virtual void SetFlow()
        {

        }

        protected virtual void OnBindingsComplete()
        {

        }

        protected virtual void OnDestroy()
        {
            if (mCommandBinder != null)
                mCommandBinder.OnDestroy();
            mCommandBinder = null;
        }
    }
}
