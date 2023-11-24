using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

using NavMeshAgent = UnityEngine.AI.NavMeshAgent;

namespace NodeCanvas.Tasks.Actions
{

    [Category("✫ Utility")]
    [Description("Assign Navmesh angent speed to CurrentSpeed")]
    public class GetNavmeshSpeed : ActionTask<NavMeshAgent>
    {
        public BBParameter<float> currentSpeed;

        protected override string info
        {
            get { return string.Format("Assign Navmesh angent speed to CurrentSpeed"); }
        }

        protected override void OnExecute()
        {
            currentSpeed.value = agent.velocity.magnitude;
            EndAction();
            //agent.isStopped = true;
        }
    }
}