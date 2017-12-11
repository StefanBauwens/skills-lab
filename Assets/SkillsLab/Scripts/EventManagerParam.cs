// Based on the EventManager from Unity tutorial
// The UnityActions are replaced by System.Actions for faster performance
// This EventManager will be able to pass parameters to the function
// Add custom parameter types at the end of the script

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerParam : MonoBehaviour
{
    private Dictionary<string, Action<EventParam>> eventDictionary;

    private static EventManagerParam eventManager;

    // Check if there is a EventManagerParam script
    public static EventManagerParam instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManagerParam)) as EventManagerParam;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    // Make dictionary if it doesn't exist already
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<EventParam>>();
        }
    }

    // Subscribe listener to an event name
    public static void StartListening(string eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    // Unsubscribe listener from an event name 
    public static void StopListening(string eventName, Action<EventParam> listener)
    {
        if (eventManager == null) return;
        Action<EventParam> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    // Call the function that is attached to the event name
    public static void TriggerEvent(string eventName, EventParam eventParam)
    {
        Action<EventParam> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventParam);
            // OR USE  instance.eventDictionary[eventName](eventParam);
        }
    }
}

//Re-usable structure/ Can be a class to. Add all parameters you need inside it
public struct EventParam
{
    public string param1;
    public int param2;
    public float param3;
    public bool param4;
}

