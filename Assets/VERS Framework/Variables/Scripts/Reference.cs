using System;
using UnityEngine;

namespace VERS
{
    [Serializable]
    public abstract class Reference
    {

    }

    [Serializable]
    public class Reference<DataType, VariableType> : Reference where VariableType : Variable<DataType>
    {
        [SerializeField]
        private bool m_UseConstant = true;
        [SerializeField]
        private DataType m_ConstantValue;
        [SerializeField]
        private VariableType m_Variable;

        public DataType Value
        {
            get
            {
                return m_UseConstant ? m_ConstantValue : m_Variable.Value;
            }
            set
            {
                if (m_UseConstant)
                    m_ConstantValue = value;
                else
                    m_Variable.Value = value;
            }
        }

        public Reference() { }

        public Reference(DataType value)
        {
            m_ConstantValue = value;
        }

        public static implicit operator Reference<DataType, VariableType>(DataType value)
        {
            return new Reference<DataType, VariableType>(value);
        }
    }
}
