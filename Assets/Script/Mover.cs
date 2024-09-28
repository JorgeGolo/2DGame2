using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// no se puede adjuntar a nada, por eso abstracrt
public abstract class Mover : Fighter
{
    
    private Vector3 moveDelta;
    private BoxCollider2D boxcollider;
    private RaycastHit2D hit;

    protected float ySpeed = 0.75f;
    protected float xSpeed = 1f;

    protected virtual void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollection()
    {

    }


    protected virtual void UpdateMotor(Vector3 input)
    {
        
        // Reset moveDelta
        //moveDelta = new Vector3(x,y,0);
        //moveDelta = input;
        moveDelta = new Vector3(input.x * xSpeed,input.y * ySpeed,0);
       
        // sprite direction, throught localScale
        if (moveDelta.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        // add push vector if any
        moveDelta += pushDirection;

        // reduce the push force every frame
        // base off the recovery speed

        pushDirection = Vector3.Lerp(pushDirection,Vector3.zero,pushRecoverySpeed);


        // Make sure we can move in this direction by casting a box there
        hit = Physics2D.BoxCast(
            transform.position, 
            boxcollider.size, 0, 
            new Vector2(0,moveDelta.y), 
            Math.Abs(moveDelta.y * Time.deltaTime), 
            LayerMask.GetMask("Actor", "Blocking")
        );

        if (hit.collider == null)
        {
            // Moving
            //transform.Translate(moveDelta * Time.deltaTime);
            transform.Translate(0,moveDelta.y * Time.deltaTime, 0);
        }
        hit = Physics2D.BoxCast(
            transform.position, 
            boxcollider.size, 0, 
            new Vector2(moveDelta.x,0), 
            Math.Abs(moveDelta.x * Time.deltaTime), 
            LayerMask.GetMask("Actor", "Blocking")
        );

        if (hit.collider == null)
        {
            // Moving
            //transform.Translate(moveDelta * Time.deltaTime);
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        
    
    }
    public void Heal(int healingAmount)
    {
        if (hitpoint > maxHitPoint)
        { 
            return;
        }
        hitpoint += healingAmount;
        if(hitpoint > maxHitPoint)
        {
            hitpoint = maxHitPoint;
        }
        else
        {
        GameManager.instance.ShowText(
            "+" + healingAmount.ToString() + " HP",
            25,
            Color.green,
            transform.position,
            Vector3.up *30,
            1f
        );
        }
    }
}
