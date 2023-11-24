using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Conditions
{

    [Category("✫ Blackboard")]
    public class DefaultNode : ConditionTask
    {

        [BlackboardOnly]
        public bool valueA = true;

        protected override string info
        {
            get { return valueA + "";  }
        }

        protected override bool OnCheck()
        {

            return valueA;
        }
    }
}