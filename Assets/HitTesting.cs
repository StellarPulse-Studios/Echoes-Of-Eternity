using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class HitTesting : MonoBehaviour, IDamagable
{
    public Animator m_Animator;

    public void GotHit()
    {
        m_Animator.SetTrigger("Hit");
        m_Animator.SetInteger("HitID", Random.Range(1, 6));
    }

    public void OnDamage()
    {
        GotHit();
    }
}
