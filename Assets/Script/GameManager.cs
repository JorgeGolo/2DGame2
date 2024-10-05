using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    

    // References

    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    //public Weapon weapon...
    public RectTransform hitPointBar;
    public Animator defMenuAnimator;

    public MoveByButtons pad;

    //Logic

    public int coins;
    public int experience;

    // saving to JSON

    public GameData gameData = new GameData();

    public string archivoDeGuardado;

    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        archivoDeGuardado = Application.dataPath + "datosJuego.json";


          // to start with no data
        PlayerPrefs.DeleteAll();   

        DontDestroyOnLoad(gameObject);

        //SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    void Start()
    {
        player.SetLevel(GetCurrentLevel());
        CharacterMenu.instance.UpdateMenu();

    }



    public void Exit()
    {

        #if UNITY_ANDROID
            // Cierra la aplicación completamente en Android
            AndroidJavaObject activity = new AndroidJavaObject("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = activity.GetStatic<AndroidJavaObject>("currentActivity");
            context.Call("finish");
        #else
            // Cierra la aplicación en plataformas de escritorio
            Application.Quit();
        #endif
    }

    public void ToggleButtonMenu()
    {

    }



    public void ShowText(
        string msg, 
        int fontsize, 
        Color color, 
        Vector3 position, 
        Vector3 motion, 
        float duration)    
    {
        floatingTextManager.Show(msg, fontsize, color, position, motion, duration);
    }

    public bool TryUpgradeWeapon()
    {
        if(weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }

        if(coins >= weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
        

    }

    public void OnHitPointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitPoint;
        hitPointBar.localScale = new Vector3(1,ratio,1);
    }

    public void Respawn()
    {
        defMenuAnimator.SetTrigger("hide");
        SceneManager.LoadScene("Main");
        player.Respawn();
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("Spawn").transform.position;   
    }
    public void SaveState()
    {


        //Debug.Log("Save");

        string es = "";

        es += "0" + "|"; 
        es += coins.ToString() + "|";
        es += experience.ToString() + "|";
        es += weapon.weaponLevel.ToString(); 

        PlayerPrefs.SetString("SaveState", es);
    }

 


    public void LoadState(Scene s, LoadSceneMode mode)
    {

        SceneManager.sceneLoaded += LoadState;

        if(!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }

        // this time diferent ''
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);

        weapon.SetWeaponLevel(int.Parse(data[3]));
 
    }

    public void GuardarDatos()
    {
        // Asigna los valores actuales a GameData
        gameData.coins = coins; // Suponiendo que tienes una variable coins en GameManager
        gameData.experience = experience; // Suponiendo que tienes una variable experience
        gameData.weaponLevel = weapon.weaponLevel; // Suponiendo que tienes un objeto weapon en GameManager
        gameData.hitpoint = player.hitpoint;
        gameData.maxHitPoint = player.maxHitPoint;
        // Serializa el estado del juego a JSON
        string json = JsonUtility.ToJson(gameData, true);

        // Guarda el JSON en un archivo
        File.WriteAllText(archivoDeGuardado, json);

        Debug.Log("Partida guardada." + gameData.hitpoint);
    }

    private void CargarDatos()
    {
        // Verifica si el archivo existe
        if (File.Exists(archivoDeGuardado))
        {
            // Lee el contenido del archivo JSON
            string json = File.ReadAllText(archivoDeGuardado);

            // Deserializa el estado del juego
            gameData = JsonUtility.FromJson<GameData>(json);

            // Restaura los datos del jugador
            coins = gameData.coins;
            experience = gameData.experience;
            weapon.SetWeaponLevel(gameData.weaponLevel);
            player.hitpoint = gameData.hitpoint;
            player.maxHitPoint = gameData.maxHitPoint;   

            Debug.Log("Partida cargada.");
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de guardado.");
        }
        CharacterMenu.instance.UpdateMenu();
        OnHitPointChange();
    }

    private void Update()
    {
    // Guardar la partida cuando se presiona F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //Debug.Log("Partida guardada");
            GuardarDatos();
        }

        // Cargar la partida cuando se presiona F6
        if (Input.GetKeyDown(KeyCode.F6))
        {
            CargarDatos();
        }
    }

    public int GetXpToLovel(int level)
    {
        int r = 0;
        int xp = 0;
        while(r<level)
        {
            xp+=xpTable[r];
            r++;
        }
        return xp;
    }
    // Experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(experience >= add)
        {
            add += xpTable[r];
            r++;

            if(r == xpTable.Count)
            {
                return r;
            }
        };
        
        return r;
    }
    public void GrantXP(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if(currLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        //Debug.Log("levelup");
        player.OnLevelUp();
        OnHitPointChange();
    }
}


