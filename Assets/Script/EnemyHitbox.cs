using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    // Start is called before the first frame update
    public int damage = 1;
    public float pushForce = 3;
    protected override void OnCollide(Collider2D coll)
    {
        //base.OnCollide(coll);
        if(coll.tag =="Fighter" && coll.name == "Player")
        {
            // create new dmg object, 
            // set to the fighter 

            Damage dmg = new Damage
            {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceivedDamage", dmg);

        }
    }
}
