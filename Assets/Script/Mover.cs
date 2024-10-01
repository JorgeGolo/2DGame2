using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// no se puede adjuntar a nada, por eso abstracrt
public abstract class Mover : Fighter
{
    private Vector3 originalSize;
    private Vector3 moveDelta;
    private BoxCollider2D boxcollider;
    private RaycastHit2D hit;

    public float ySpeed = 0.75f;
    public float xSpeed = 1f;
    private bool isMoving = false;
private Vector3 targetPosition;

    protected virtual void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
        originalSize = transform.localScale;

    }

    private void OnCollection()
    {

    }

public virtual void MoveTo(Vector3 clickPosition)
{
    // Establecer la posición objetivo a la que el jugador debe moverse
    targetPosition = clickPosition;
    isMoving = true;
}
void Update()
{
    if (isMoving)
    {
        // Llamar a la función para mover al jugador hacia la posición objetivo
        MoveTowardsTarget();
    }
}

    private void MoveTowardsTarget()
    {
        // Obtenemos la posición actual del jugador
        Vector3 currentPosition = transform.position;

        // Calculamos la dirección hacia la que nos movemos
        Vector3 moveDirection = (targetPosition - currentPosition).normalized;
   float distanceToMove = ySpeed * Time.deltaTime;

    // Usar Physics2D.BoxCast para verificar colisiones en el camino
    RaycastHit2D hit = Physics2D.Raycast(
        currentPosition,           // Origen del BoxCast
                    // Ángulo (para 2D es 0)
        moveDirection,             // Dirección del movimiento
        distanceToMove,            // Distancia a verificar
        LayerMask.GetMask("Blocking") // Capa de objetos bloqueantes
    );

    // Si hay colisión, mover al borde del obstáculo
    if (hit.collider != null)
    {
        // Moverse hasta el punto más cercano al obstáculo
        transform.position = Vector3.MoveTowards(currentPosition, hit.point, distanceToMove);
    }
    else
    {
        // Si no hay colisión, mover al objetivo
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, distanceToMove);
    }

        // Cambiar la dirección del sprite en función de hacia dónde nos estamos moviendo
        if (moveDirection.x > 0)
        {
            transform.localScale = originalSize; // Mover a la derecha
        }
        else if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z); // Mover a la izquierda
        }

        // Comprobar si hemos llegado al objetivo
        if (Vector3.Distance(currentPosition, targetPosition) < 0.1f)
        {
            // Hemos llegado a la posición objetivo, dejar de movernos
            isMoving = false;
        }
    }

    public virtual void UpdateMotor(Vector3 input)
    {
        
        // Reset moveDelta
        //moveDelta = new Vector3(x,y,0);
        //moveDelta = input;
        moveDelta = new Vector3(input.x * xSpeed,input.y * ySpeed,0);
       
        // sprite direction, throught localScale
        if (moveDelta.x > 0)
        {
            //transform.localScale = Vector3.one;
            transform.localScale = originalSize;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(originalSize.x * -1, originalSize.y, originalSize.z);
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

}
