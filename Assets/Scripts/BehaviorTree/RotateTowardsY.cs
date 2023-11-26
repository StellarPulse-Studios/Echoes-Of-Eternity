using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Direct")]
    [Description("Rotate the agent towards the target per frame in Y axis")]
    public class RotateTowardsY : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        [SliderField(1, 180)]
        public BBParameter<float> angleDifference = 5;
        public BBParameter<Vector3> upVector = Vector3.up;
        public bool waitActionFinish;

        private float currvel;

        protected override void OnUpdate()
        {
            Vector3 projectedOffset = Vector3.ProjectOnPlane(target.value.transform.position - agent.position, agent.up);
            if (Vector3.Angle(projectedOffset, agent.forward) <= angleDifference.value)
            {
                EndAction();
                return;
            }

            // Debug.DrawRay(agent.position + Vector3.up*5, projectedOffset);
            //Debug.DrawRay(agent.position + Vector3.up * 5, agent.forward, Color.blue);


            //var dir = target.value.transform.position - agent.position;
            var dir = projectedOffset;
            agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(agent.forward, dir, speed.value * Time.deltaTime, 0), upVector.value);

            if (!waitActionFinish)
            {
                EndAction();
            }
        }
    }
}