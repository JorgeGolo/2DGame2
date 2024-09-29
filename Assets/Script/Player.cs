using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Mover
{

    private SpriteRenderer spriteRenderer;
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
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // from Mover    
        UpdateMotor(new Vector3(x,y,0));
    
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

        public void Heal(int healingAmount)
    {
        Debug.Log("EMPIEZA CURACION!");

        if (hitpoint < maxHitPoint)
        { 
           
            Debug.Log("A CURAR!");

            hitpoint += healingAmount;

            GameManager.instance.ShowText(
                "+" + healingAmount.ToString() + " hp",
                15,
                Color.green,
                transform.position,
                Vector3.up *30,
                1f
            );
        }
        else
        {
            Debug.Log("NO CURAR!");
        }
        
    }



}
