using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager: MonoBehaviour
{

    public static PortalManager instance;
    public string leavingScene;
    public string nextScene;
    public Player player;

    void Awake()
    {
        if(PortalManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    
    }


    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        if (leavingScene!= null && leavingScene != "")
        {
            player.transform.position = GameObject.Find(leavingScene).transform.position;   
            Debug.Log("llevando al jugador al sitio de la carga");
        }
        if (SceneManager.GetActiveScene().name != "Main0")
        {
            GameManager.instance.SaveData();
            Debug.Log("Saved");
        }
            // Quitar el listener una vez que la escena ha sido cargada
    }


    void Start()
    {

    }
    
}
