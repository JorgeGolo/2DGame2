using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    protected override void Death()
    {
        //base.Death();
        Destroy(gameObject);
    }

}
