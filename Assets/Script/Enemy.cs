using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;

    // Logic // danger // chasing
    public float triggerLenght = 1;
    public float chaseLenght = 5;
    public bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox - enemy weapon

    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    // we can not heritate from collide
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        // el hitbox es el sprite - go hijo del enemy...
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();

    }

    private void FixedUpdate()
    {
        // is the player in range?
        if(Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            /*if(Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght)
            {
                chasing = true;
            }*/
            chasing = Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght;
            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            
            else
            {
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

    // chacking

    collidingWithPlayer = false;
    // uopdate from Collidable

    hitbox.OverlapCollider(filter, hits);
    for (int i = 0; i < hits.Length; i++)
    {
        if(hits[i] == null)
            continue;

        //OnCollide(hits[i]);
        if (hits[i].tag == "fighter" && hits[i].name == "Player")
        {
            collidingWithPlayer = true;
        }

        hits[i] = null;
     }
    }

    protected override void Death()
    {
        //base.Death();
        Destroy(gameObject);
        //GameManager.instance.experience += xpValue;
        GameManager.instance.GrantXP(xpValue);
        GameManager.instance.ShowText(
            "+" + xpValue + " xp",
            20,
            Color.magenta,
            transform.position,
            Vector3.up*40,
            1f
        );
    }
}
