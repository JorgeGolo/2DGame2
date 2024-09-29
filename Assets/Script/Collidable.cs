using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// [RequireComponent(typeof(BoxCollider2D))]

// We use virtual functions because we will use inheritance
// In the first example, with the Chest1
// 
// There: we will use protected override with Oncollide
// and the autocomplete will write base.Onllide
// that calls to the "base" inherit
public class Collidable : MonoBehaviour
{

    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        // collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i] == null)
                continue;

            //Debug.Log(hits[i].name);
            //if (this.tag == "NPC_0") ( ... ) // say something...
            //if (this.tag == "modenas") ( ... ) // grant monedas...

            OnCollide(hits[i]);

            // clean hits
            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        // Debug.Log(coll.name);
        // Debug.Log("Basic function from Collidable for" + coll.name);

    }
}
