using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Chest1 : Collidable
public class Chest1 : Collectable
{

    /* protected override void OnCollide(Collider2D coll)
     {
         //base.OnCollide(coll);
         Debug.Log("grant coins");
     }
     */
    public Sprite emptyChest;
    public int coins = 5;

    protected override void OnCollect()
    {
        if(!collected)
        {
            base.OnCollect(); //collected = true;
            Debug.Log("grant coins");

            // change the sprite
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            Debug.Log("coins granted: " + coins);

            // not needed, because collected was setted to true, by base.OnCollect
            // and cins can not be collected again
            // coins = 0;

            GameManager.instance.coins += coins;

            // showing a text, font 25, yellow, moving up
            GameManager.instance.ShowText(
                "+" + coins + " coins!",
                23,
                Color.yellow,
                transform.position,
                Vector3.up * 25,
                1.5f
            );

        }

    }

}
