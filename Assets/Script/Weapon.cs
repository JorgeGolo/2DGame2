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
    private float cooldown = 0.5f;
    private float lastSwing;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        Debug.Log("swing!");
    }
    
    //

}
