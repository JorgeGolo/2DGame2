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

    public float randomMovementLenght = 1f;

    private Vector3 randomTargetPoint; // Guardamos el punto objetivo aleatorio
    private float timeToChangeDirection = 2f; // Intervalo de tiempo para cambiar de dirección
    private float lastDirectionChangeTime; // Tiempo desde el último cambio de dirección
    public float speedRandom = 0.01f;
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
        // Actualizar colisiones
        collidingWithPlayer = false;
        hitbox.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i] == null)
                continue;

            if (hits[i].tag == "fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            hits[i] = null;
        }

        // Calcular si está en rango de persecución
        chasing = Vector3.Distance(playerTransform.position, transform.position) < triggerLenght;

        if (chasing)
        {
            // Si está persiguiendo al jugador
            if (!collidingWithPlayer)
            {
                // Perseguir al jugador
                UpdateMotor((playerTransform.position - transform.position).normalized);
            }
        }
        else
        {
            // Si el jugador está fuera del rango de triggerLenght, moverse aleatoriamente
            if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
            {
                // Si aún no hemos asignado un punto objetivo o es hora de cambiar de dirección
                if (randomTargetPoint == Vector3.zero || Time.time - lastDirectionChangeTime > timeToChangeDirection)
                {
                    // Asignar nuevo punto objetivo aleatorio dentro de un rango
                    randomTargetPoint = startingPosition + RandomMovement(randomMovementLenght);
                    lastDirectionChangeTime = Time.time; // Actualizar el tiempo del último cambio
                }

                // Moverse hacia el punto objetivo
                UpdateMotor(randomTargetPoint - transform.position, speedRandom);

                // Si el enemigo ha llegado cerca del punto objetivo, recalcular
                if (Vector3.Distance(transform.position, randomTargetPoint) < 0.5f)
                {
                    randomTargetPoint = startingPosition + RandomMovement(randomMovementLenght);
                }
            }
            else
            {
                // Si el jugador está fuera de chaseLength, volver a la posición inicial
                UpdateMotor(startingPosition - transform.position);
            }
        }
    }

// Método para obtener un vector de movimiento aleatorio dentro del radio chaseLenght
private Vector3 RandomMovement(float randomMovementLenght)
{
    // Genera un valor aleatorio en una esfera (2D en este caso)
    Vector2 randomDirection = Random.insideUnitCircle.normalized;
    
    // Calcular el punto aleatorio dentro de la distancia chaseLength
    return new Vector3(randomDirection.x, randomDirection.y, 0) * randomMovementLenght;
}
    protected override void Death()
    {
        //base.Death();
        //Destroy(gameObject);
        gameObject.SetActive(false);
        GameManager.instance.SaveEnemyDeath(gameObject);
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
