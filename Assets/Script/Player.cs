using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Mover
{

    private SpriteRenderer spriteRenderer;

    public bool isAlive = true;
    public static Player instance;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(Player.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    protected override void Death()
    {
        //base.Death();
        isAlive = false;
        GameManager.instance.defMenuAnimator.SetTrigger("show");
    }

    public void Respawn()
    {
        Heal(maxHitPoint);
        isAlive = true;
        lastInmune = Time.time;
        pushDirection = Vector3.zero;
    }
    protected override void ReceivedDamage(Damage dmg)
    {
        if (isAlive)
        {
            base.ReceivedDamage(dmg);
            GameManager.instance.OnHitPointChange();
        }    
        
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // from Mover    
        if (isAlive)
        {
            UpdateMotor(new Vector3(x,y,0));
        }
    
    }

    public void SwapSprite(int skinID)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinID];    
    }

    public void OnLevelUp()
    {
        maxHitPoint++;
        hitpoint = maxHitPoint;
    }

    public void SetLevel(int level)
    {
        for (var i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }
    public void SetLevel1()
    {
        maxHitPoint = 10;
        hitpoint = 10;
        GameManager.instance.weapon.weaponLevel = 0;
        GameManager.instance.experience = 0;
        GameManager.instance.coins = 0;
        
        PlayerPrefs.DeleteAll();   

        CharacterMenu.instance.UpdateMenu();
    }


    public void Heal(int healingAmount)
    {
        //Debug.Log("EMPIEZA CURACION!");

        if (hitpoint < maxHitPoint)
        { 
           
           // Debug.Log("A CURAR!");

            hitpoint += healingAmount;

            GameManager.instance.ShowText(
                "+" + healingAmount.ToString() + " hp",
                15,
                Color.green,
                transform.position,
                Vector3.up *30,
                1f
            );
            GameManager.instance.OnHitPointChange();
        }
        else
        {
           // Debug.Log("NO CURAR!");
        }
        
    }

    

}
