using System;
using UnityEngine;

namespace XcelerateGames.IOC
{
    #region Signal with no parameters
    public class Signal : AbstractSignal
    {
        private Action mEvent;

        public void AddListener(Action listener)
        {
            mEvent += listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"AddListener<0>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void RemoveListener(Action listener)
        {
            mEvent -= listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"RemoveListener<0>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void Dispatch()
        {
            if (!IsMuted)
            {
                if (mEvent != null)
                {
                    Delegate[] invocationList = mEvent.GetInvocationList();
                    if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                        Debug.Log($"Dispatch<0> {GetType()} has {invocationList.Length} listeners");
                    for (int i = 0; i < invocationList.Length; ++i)
                    {
                        Delegate dlt = invocationList[i];
                        if (dlt == null || dlt.Target == null)
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:delegate or target is null");
                            continue;
                        }
                        if (dlt.Target != null && dlt.Target.ToString() == "null")
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:Target object is null for : {dlt.Method}");
                            continue;
                        }
                        if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                            Debug.Log($"Dispatch<0> Invoking :{dlt.Target}::{dlt.Method}");
                        Action dlgt = dlt as Action;
                        dlgt();
                    }
                }
                OnDispatch();
            }
        }
    }
    #endregion Signal with no parameters

    #region Signal with one parameter
    public class Signal<T> : AbstractSignal
    {
        private Action<T> mEvent;

        public Signal()
        {
            AddInjectionBinding<T>();
        }

        public void AddListener(Action<T> listener)
        {
            mEvent += listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"AddListener<1>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void RemoveListener(Action<T> listener)
        {
            mEvent -= listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"RemoveListener<1>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void Dispatch(T value)
        {
            if (!IsMuted)
            {
                InjectionBindings[typeof(T)] = value;
                if (mEvent != null)
                {
                    Delegate[] invocationList = mEvent.GetInvocationList();
                    if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                        Debug.Log($"Dispatch<1> {GetType()} has {invocationList.Length} listeners");
                    for (int i = 0; i < invocationList.Length; ++i)
                    {
                        Delegate dlt = invocationList[i];
                        if (dlt == null || dlt.Target == null)
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:delegate or target is null");
                            continue;
                        }
                        if (dlt.Target != null && dlt.Target.ToString() == "null")
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:Target object is null for : {dlt.Method}");
                            continue;
                        }
                        if (DebugLogsOn)
                            Debug.Log($"Dispatch<1> Invoking :{dlt.Target}::{dlt.Method}");
                        Action<T> dlgt = dlt as Action<T>;
                        dlgt(value);
                    }
                }

                OnDispatch();
            }
        }
    }
    #endregion Signal with one parameters

    #region Signal with two parameters
    public class Signal<T, U> : AbstractSignal
    {
        private Action<T, U> mEvent;

        public Signal()
        {
            AddInjectionBinding<T>();
            AddInjectionBinding<U>();
        }

        public void AddListener(Action<T, U> listener)
        {
            mEvent += listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"AddListener<2>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void RemoveListener(Action<T, U> listener)
        {
            mEvent -= listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"RemoveListener<2>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void Dispatch(T value1, U value2)
        {
            if (!IsMuted)
            {
                InjectionBindings[typeof(T)] = value1;
                InjectionBindings[typeof(U)] = value2;

                if (mEvent != null)
                {
                    Delegate[] invocationList = mEvent.GetInvocationList();
                    if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                        Debug.Log($"Dispatch<2> {GetType()} has {invocationList.Length} listeners");
                    for (int i = 0; i < invocationList.Length; ++i)
                    {
                        Delegate dlt = invocationList[i];
                        if (dlt == null || dlt.Target == null)
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:delegate or target is null");
                            continue;
                        }
                        if (dlt.Target != null && dlt.Target.ToString() == "null")
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:Target object is null for : {dlt.Method}");
                            continue;
                        }
                        if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                            Debug.Log($"Dispatch<2> Invoking :{dlt.Target}::{dlt.Method}");
                        Action<T, U> dlgt = dlt as Action<T, U>;
                        dlgt(value1, value2);
                    }
                }
                OnDispatch();
            }
        }
    }
    #endregion Signal with two parameters

    #region Signal with three parameters
    public class Signal<T, U, V> : AbstractSignal
    {
        private Action<T, U, V> mEvent;

        public Signal()
        {
            AddInjectionBinding<T>();
            AddInjectionBinding<U>();
            AddInjectionBinding<V>();
        }

        public void AddListener(Action<T, U, V> listener)
        {
            mEvent += listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"AddListener<3>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void RemoveListener(Action<T, U, V> listener)
        {
            mEvent -= listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"RemoveListener<3>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void Dispatch(T value1, U value2, V value3)
        {
            if (!IsMuted)
            {
                InjectionBindings[typeof(T)] = value1;
                InjectionBindings[typeof(U)] = value2;
                InjectionBindings[typeof(V)] = value3;

                if (mEvent != null)
                {
                    Delegate[] invocationList = mEvent.GetInvocationList();
                    if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                        Debug.Log($"Dispatch<3> {GetType()} has {invocationList.Length} listeners");
                    for (int i = 0; i < invocationList.Length; ++i)
                    {
                        Delegate dlt = invocationList[i];
                        if (dlt == null || dlt.Target == null)
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:delegate or target is null");
                            continue;
                        }
                        if (dlt.Target != null && dlt.Target.ToString() == "null")
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:Target object is null for : {dlt.Method}");
                            continue;
                        }
                        if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                            Debug.Log($"Dispatch<3> Invoking :{dlt.Target}::{dlt.Method}");
                        Action<T, U, V> dlgt = dlt as Action<T, U, V>;
                        dlgt(value1, value2, value3);
                    }
                }
                OnDispatch();
            }
        }
    }
    #endregion Signal with three parameters

    #region Signal with four parameters
    public class Signal<T, U, V, W> : AbstractSignal
    {
        private Action<T, U, V, W> mEvent;

        public Signal()
        {
            AddInjectionBinding<T>();
            AddInjectionBinding<U>();
            AddInjectionBinding<V>();
            AddInjectionBinding<W>();
        }

        public void AddListener(Action<T, U, V, W> listener)
        {
            mEvent += listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"AddListener<4>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void RemoveListener(Action<T, U, V, W> listener)
        {
            mEvent -= listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"RemoveListener<4>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void Dispatch(T value1, U value2, V value3, W value4)
        {
            if (!IsMuted)
            {
                InjectionBindings[typeof(T)] = value1;
                InjectionBindings[typeof(U)] = value2;
                InjectionBindings[typeof(V)] = value3;
                InjectionBindings[typeof(W)] = value4;

                if (mEvent != null)
                {
                    Delegate[] invocationList = mEvent.GetInvocationList();
                    if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                        Debug.Log($"Dispatch<4> {GetType()} has {invocationList.Length} listeners");
                    for (int i = 0; i < invocationList.Length; ++i)
                    {
                        Delegate dlt = invocationList[i];
                        if (dlt == null || dlt.Target == null)
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:delegate or target is null");
                            continue;
                        }
                        if (dlt.Target != null && dlt.Target.ToString() == "null")
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:Target object is null for : {dlt.Method}");
                            continue;
                        }
                        if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                            Debug.Log($"Dispatch<4> Invoking :{dlt.Target}::{dlt.Method}");
                        Action<T, U, V, W> dlgt = dlt as Action<T, U, V, W>;
                        dlgt(value1, value2, value3, value4);
                    }
                }
                OnDispatch();
            }
        }
    }
    #endregion Signal with four parameters

    #region Signal with five parameters
    public class Signal<T, U, V, W, X> : AbstractSignal
    {
        private Action<T, U, V, W, X> mEvent;

        public Signal()
        {
            AddInjectionBinding<T>();
            AddInjectionBinding<U>();
            AddInjectionBinding<V>();
            AddInjectionBinding<W>();
        }

        public void AddListener(Action<T, U, V, W, X> listener)
        {
            mEvent += listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"AddListener<5>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void RemoveListener(Action<T, U, V, W, X> listener)
        {
            mEvent -= listener;
            if (DebugLogsOn)
            {
                int count = 0;
                if (mEvent != null && mEvent.GetInvocationList() != null)
                    count = mEvent.GetInvocationList().Length;
                Debug.Log($"RemoveListener<5>: {GetType()} -> {listener.Target} : {listener.Method}, Listeners: {count}");
            }
        }

        public void Dispatch(T value1, U value2, V value3, W value4, X value5)
        {
            if (!IsMuted)
            {
                InjectionBindings[typeof(T)] = value1;
                InjectionBindings[typeof(U)] = value2;
                InjectionBindings[typeof(V)] = value3;
                InjectionBindings[typeof(W)] = value4;
                InjectionBindings[typeof(X)] = value5;

                if (mEvent != null)
                {
                    Delegate[] invocationList = mEvent.GetInvocationList();
                    if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                        Debug.Log($"Dispatch<5> {GetType()} has {invocationList.Length} listeners");
                    for (int i = 0; i < invocationList.Length; ++i)
                    {
                        Delegate dlt = invocationList[i];
                        if (dlt == null || dlt.Target == null)
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:delegate or target is null");
                            continue;
                        }
                        if (dlt.Target != null && dlt.Target.ToString() == "null")
                        {
                            if (DebugLogsOn)
                                Debug.LogWarning($"{GetType()}:Target object is null for : {dlt.Method}");
                            continue;
                        }
                        if (DebugLogsOn || (XDebug.CanLog(XDebug.Mask.IOC)))
                            Debug.Log($"Dispatch<5> Invoking :{dlt.Target}::{dlt.Method}");
                        Action<T, U, V, W, X> dlgt = dlt as Action<T, U, V, W, X>;
                        dlgt(value1, value2, value3, value4, value5);
                    }
                }
                OnDispatch();
            }
        }
    }
    #endregion Signal with five parameters
}