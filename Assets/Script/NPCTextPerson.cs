using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{

    public string message;
    private float cooldown = 4f;
    private float lastShout;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        lastShout = - cooldown;
    }

    protected override void OnCollide(Collider2D coll)
    {
 
        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(
                message,
                20,
                Color.white,
                transform.localPosition + new Vector3(0,0.16f,0),
                //transform.position + Vector3.up,
                Vector3.zero,
                cooldown
            );
        }
        //base.OnCollide(coll);
        
    }
    

 
}
