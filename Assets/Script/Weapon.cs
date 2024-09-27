using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{

    // damage
    public int damagePoint = 1;
    public float pushForce = 2f;

    // upgrade

    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;


    // swing

    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            lastSwing = Time.time;
            Swing();
        }
    }

    private void Swing()
    {
        //Debug.Log("swing!");
        anim.SetTrigger("Swing");
    }

    //

    protected override void OnCollide(Collider2D coll)
    {
        //base.OnCollide(coll);
        if (coll.tag == "Fighter") 
        {
            if (coll.name == "Player")
            {
                return;
            }
            
            // Create a new Famage Objet, 
            // set to the fighter 

            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceivedDamage", dmg);
            
            Debug.Log(coll.name);

        }
    }
}
