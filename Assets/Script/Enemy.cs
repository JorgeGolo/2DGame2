using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // state enemys

    public bool isAlive = true;
        // Nombre de la escena donde está el enemigo
    public string sceneName;


   /*private void Awake()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        // el hitbox es el sprite - go hijo del enemy...
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();

    }*/

    protected override void Start()
    {


        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        // el hitbox es el sprite - go hijo del enemy...
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        // Asignar el nombre de la escena
        sceneName = SceneManager.GetActiveScene().name;
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
                RandomMovement();
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

    private void RandomMovement()
    {
        //Debug.Log("random mov.");
    }

    protected override void Death()
    {
        //base.Death();
        //Destroy(gameObject);
        gameObject.SetActive(false);
        isAlive = false;
        //GameManager.instance.experience += xpValue;
        GameManager.instance.GrantXP(xpValue);
        GameManager.instance.ShowText(
            "+" + xpValue + " xp",
            FloatingTextManager.instance.floatingTextSize,
            Color.magenta,
            transform.position,
            Vector3.up*40,
            1f
        );
    }
}
