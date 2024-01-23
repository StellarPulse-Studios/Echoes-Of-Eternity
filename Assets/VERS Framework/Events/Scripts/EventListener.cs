using UnityEngine;
using UnityEngine.Events;

namespace VERS
{
    public class EventListener : MonoBehaviour
    {
        public Event Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            if (Event != null)
            {
                Event.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            if (Event != null)
            {
                Event.UnregisterListener(this);
            }
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}