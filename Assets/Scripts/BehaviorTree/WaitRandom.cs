using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("✫ Utility")]
    public class WaitRandom : ActionTask
    {

        public int waitMin = 0;
        public int waitMax = 1;
        public float multiplier = 1f;
        public CompactStatus finishStatus = CompactStatus.Success;
        private float waitTime;

        protected override string info
        {
            get { return string.Format("Wait Random sec.", waitTime); }
        }
        protected override void OnExecute()
        {
            waitTime = Random.Range(waitMin, waitMax) * multiplier;
        }
        protected override void OnUpdate()
        {

            if (elapsedTime >= waitTime)
            {
                EndAction(finishStatus == CompactStatus.Success ? true : false);
            }
        }
        
    }
}
