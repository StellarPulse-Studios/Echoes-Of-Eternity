using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

using NavMeshAgent = UnityEngine.AI.NavMeshAgent;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement/Pathfinding")]
    [Description("Idle")]
    public class Idle : ActionTask<NavMeshAgent>
    {

        protected override string info
        {
            get { return string.Format("Idle"); }
        }

        protected override void OnExecute()
        {
            //agent.SetDestination(agent.transform.position);
            agent.isStopped = true;
            EndAction(true);
        }

        

        protected override void OnPause() { OnStop(); }
        protected override void OnStop()
        {
        
        }
    }
}