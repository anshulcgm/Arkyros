using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class EventSystem : IClass
{
    public Type MonoScript
    {
        get
        {
            return typeof(EventSystemMono);
        }
    }

    public static List<IUnityEvent> Events { get; internal set; }
    private static List<IUnityEvent> newEvents = new List<IUnityEvent>();

    public EventSystem()
    {
        Events = new List<IUnityEvent>();
    }

    /* Parameters: 
     * e: new event to add
     * 
     * Description:
     * Adds new event to new events list
     */
    public static void AddEvent(IUnityEvent e)
    {
        newEvents.Add(e);
    }

    /* Parameters: 
     * e: event to remove
     * 
     * Description:
     * removes event from the Events list
     */
    public static void CancelEvent(IUnityEvent e)
    {
        Events.Remove(e);
    }


    /* Description:
     * removes old events, adds in the new events. Called on LateUpdate, after all Update functions have finished.
     */
    public void HandleEvents()
    {
        // remove all the events that either don't have a duration or have exceeded their duration.
        int index = 0;
        while(index < Events.Count)
        {
            ITimedEvent timedEvent = (ITimedEvent)Events[index];
            if(timedEvent == null)
            {
                Events.RemoveAt(index);
            }
            else if((DateTime.Now - timedEvent.CreationTime).TotalSeconds > timedEvent.Duration)
            {
                Events.RemoveAt(index);
            }
            else
            {
                index++;
            }            
        }

        // add in all the new events, clear the new events list
        Events.AddRange(newEvents);
        newEvents = new List<IUnityEvent>();
    }
}