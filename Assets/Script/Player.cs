using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Vector3 moveDelta;
    private BoxCollider2D boxcollider;
    private RaycastHit2D hit;


    private void Start()
    {
        boxcollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollection()
    {

    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Reset moveDelta
        moveDelta = new Vector3(x,y,0);
       
        // sprite direction, throught localScale
        if (moveDelta.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (moveDelta.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

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
