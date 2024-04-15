using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class EventManager
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
                instance = new EventManager();

            return instance;
        }
    }

    private bool IsDebug = false;

    public Action OnEventManagerReloaded;
    public delegate void EventDelegate<T> (T eventParam) where T : IEventParameterBase;
    private delegate void EventDelegate(IEventParameterBase eventParam);

    private Dictionary<Type, EventDelegate> delegateExecuteGroup = new Dictionary<Type, EventDelegate>();
    private Dictionary<Delegate, EventDelegate> delegateLookUpGroup = new Dictionary<Delegate, EventDelegate>();

    public EventManager()
    {
        SceneManager.sceneUnloaded += SceneUnLoaded;
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void SceneUnLoaded(Scene scene)
    {
#if UNITY_EDITOR
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame sf = st.GetFrame(st.FrameCount - 1);
        if (sf.GetMethod().Name == "OnDisable")
        { // Or sf.GetMethod().DeclaringType == typeof(UnityEditor.MaterialEditor) 
            return;
        }
#endif
        //Debug.LogError("Scene UnLoaded - If you see this when play and not change scene, it's bug of unity editor.\nDon't select any gameobject have material of particle when playing!");

        DebugLogWarning("EVENT-MANAGER--Remove all events on unload scene");
        RemoveAllListeners();
    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DebugLogWarning("EVENT-MANAGER--Fire event on scene loaded");

        if (OnEventManagerReloaded != null)
            OnEventManagerReloaded();
    }

    public void AddListener<T>(EventDelegate<T> eventDelegate) where T : IEventParameterBase
    {
        if (delegateLookUpGroup.ContainsKey(eventDelegate))
            return;

        // Add delegates to look up group
        EventDelegate internalDelegate = (eventParam) => eventDelegate((T)eventParam);
        delegateLookUpGroup[eventDelegate] = internalDelegate;

        // Add delegates to exectute group
        EventDelegate tempDelegate;
        if (delegateExecuteGroup.TryGetValue(typeof(T), out tempDelegate))
        {
            tempDelegate += internalDelegate;
            delegateExecuteGroup[typeof(T)] = tempDelegate;
        }
        else
        {
            delegateExecuteGroup[typeof(T)] = internalDelegate;
        }
    }

    public void TriggerEvent(IEventParameterBase eventParam)
    {
        // Get delegate by event type from dictionary
        EventDelegate tempDelegate;
        if (delegateExecuteGroup.TryGetValue(eventParam.GetType(), out tempDelegate) == false)
        {
            DebugLogWarning("EventManager - Trigger Event: No listenner on: " + eventParam.GetType());
            return;
        }

        // If found some, execute this
        if (tempDelegate != null)
            tempDelegate(eventParam);
    }

    public void RemoveListener<T>(EventDelegate<T> eventDelegate) where T : IEventParameterBase
    {
        // Try to found delegate from lookup group
        EventDelegate internalDelegate;

        if (delegateLookUpGroup.TryGetValue(eventDelegate, out internalDelegate) == false)
        {
            DebugLogWarning("EventManager - RemoveListener: No listenner on DelegateLookUpGroup");
            return;
        }

        // If found some on lookup group, then continue found on
        // execute group
        EventDelegate tempDelegate;

        if (delegateExecuteGroup.TryGetValue(typeof(T), out tempDelegate) == false)
        {
            DebugLogWarning("EventManager - RemoveListener: No listenner on DelegateExecuteGroup");
            return;
        }

        // Yeah, found it, now delete this delegate
        tempDelegate -= internalDelegate;

        // If our delegate array is out of element, then we remove it from execute
        // group
        if(tempDelegate == null)
        {
            delegateExecuteGroup.Remove(typeof(T));
        }
        else
        {
            delegateExecuteGroup[typeof(T)] = tempDelegate;
        }

        // Finally remove it from look up group
        delegateLookUpGroup.Remove(eventDelegate);

        DebugLogWarning(string.Format("EventManager - Remove listenner: {0}", eventDelegate.GetType().ToString()));
    }

    public bool IsListenerRegisted<T>(EventDelegate<T> eventDelegate) where T : IEventParameterBase
    {
        return delegateLookUpGroup.ContainsKey(eventDelegate);
    }

    public void RemoveAllListeners()
    {
        delegateExecuteGroup.Clear();
        delegateLookUpGroup.Clear();
    }

    private void DebugLogWarning(string message)
    {
        if (IsDebug == false)
            return;

#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }
}
