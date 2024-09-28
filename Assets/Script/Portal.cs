using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{

    //teleport to a random scene
    public string[] sceneNames;
    protected override void OnCollide(Collider2D coll)
    {
        //base.OnCollide(coll);

        if(coll.name == "Player")
        {
            // Teleport the player

            //GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0,sceneNames.Length)];

            GameManager.instance.SaveState();

            SceneManager.LoadScene(sceneName);
        }
    }



}
