using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalBlackboard : MonoBehaviour
{
    [SerializeField] protected GameObject target;
    [SerializeField] protected bool gotHit;
    [SerializeField] protected bool amIDead;
    [SerializeField] protected bool isAlert;

    public bool GotHit
    {
        get
        {
            return gotHit;
        }
        set
        {
            gotHit = value;
        }
    }

    public bool AmIDead
    {
        get
        {
            return amIDead;
        }
        set
        {
            amIDead = value;
        }
    }
    public bool IsAlert
    {
        get
        {
            return isAlert;
        }
        set
        {
            isAlert = value;
        }
    }



    public GameObject Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }
}
