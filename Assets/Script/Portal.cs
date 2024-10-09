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

            GameManager.instance.pad.ResetButtonStates();

            // data for PManager

            PortalManager.instance.leavingScene = SceneManager.GetActiveScene().name;
            PortalManager.instance.nextScene = sceneNames[Random.Range(0,sceneNames.Length)];
           
            string sceneName = sceneNames[Random.Range(0,sceneNames.Length)];

            // saving enemys before leave the scene
            // GameManager.instance.SaveEnemyData();

            SceneManager.LoadScene(sceneName);
        }
    }

}
