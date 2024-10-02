using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // public fields
    public int hitpoint;
    public int maxHitPoint;
    public float pushRecoverySpeed;

    // inmunity

    protected float inmuneTime = 1f;
    protected float lastInmune;

    // push
    protected Vector3 pushDirection;

    // all fighters can receive damage and die

    protected virtual void ReceivedDamage(Damage dmg)
    {
        if(Time.time - lastInmune > inmuneTime)
        {
            // allow to receive damage
            lastInmune = Time.time; // now
            hitpoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin). normalized * dmg.pushForce;

            //visual damage
            GameManager.instance.ShowText(
                dmg.damageAmount.ToString(),
                20,
                Color.red,
                transform.position + new Vector3(0,0.12f,0),
                Vector3.up,
                0.5f
            );

            // death
            if (hitpoint <= 0)
            {
                hitpoint = 0;
                Death();
            }
        }
    }
    protected virtual void Death()
    {

    }
}
