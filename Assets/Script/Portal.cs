using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{

    //teleport to a random scene
    public string[] sceneNames;

    public Player player;
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
           
            //GameManager.instance.SaveState();
            string sceneName = sceneNames[Random.Range(0,sceneNames.Length)];


            //GameManager.instance.SaveState();
            GameManager.instance.GuardarDatos();

            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("Spawn").transform.position;   
    }

}
