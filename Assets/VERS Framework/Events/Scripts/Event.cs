using System.Collections.Generic;
using UnityEngine;

namespace VERS
{
    [CreateAssetMenu(fileName = "New Event", menuName = "Event")]
    public class Event : ScriptableObject
    {
        private List<EventListener> m_Listeners = new List<EventListener>();

        public void RegisterListener(EventListener listener)
        {
            m_Listeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener)
        {
            m_Listeners.Remove(listener);
        }

        public void Raise()
        {
            for (int i = m_Listeners.Count - 1; i >= 0; i--)
            {
                m_Listeners[i].OnEventRaised();
            }
        }
    }
}
