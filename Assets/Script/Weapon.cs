using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{

    // damage
    public int[] damagePoint = {1,2,3,4,5,6,7,8};
    public float[] pushForce = {2f,2.2f,2.5f,3f,3.2f,3.6f,4f,4.5f};


    // upgrade

    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;


    // swing

    private Animator anim;
    //private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    protected override void Start()
    {
        base.Start();
        //spriteRenderer = GetComponent<SpriteRenderer>();
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
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceivedDamage", dmg);
            
            //Debug.Log(coll.name);

        }
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        // change stats
    }

    // function to use in Loadstate (GM) 
    // not actually good looking
    // because of the weapon data structure
    // and methods use to save and load
    
    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
