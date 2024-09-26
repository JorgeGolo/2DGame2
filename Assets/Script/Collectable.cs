using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{

    // protected es como private, pero los hijos tienen acceso
    // protected basically means that is private,
    // but childrens can have access to it

    // Chest is children of Collectable,
    // and Collectable is children of Collidable
    protected bool collected;

    protected override void OnCollide(Collider2D coll)
    {
        //base.OnCollide(coll);
        if(coll.name == "Player")
        {
            OnCollect();
        }
    }

    protected virtual void OnCollect()
    {
        collected = true;
    }

}
