using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    // portal manager

    public PortalManager portalManager;

    // menu references to hide on main0

    public GameObject outercircle;
    public GameObject innercircle;
    public GameObject healthbar1;
    public GameObject healthbar2;

    public GameObject menuButton;
    public GameObject attackButton;

    public Button newGameButton;
    public Button continueGameButton;


    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        //archivoDeGuardado = Application.dataPath + "datosJuego.json";
        
        archivoDeGuardado = Path.Combine(Application.persistentDataPath, "datosJuego.json");

        // to start with no data
        // PlayerPrefs.DeleteAll();   

        DontDestroyOnLoad(gameObject);

        //SceneManager.sceneLoaded += LoadState;
        //SceneManager.sceneLoaded += OnSceneLoaded;

 

    }
    void Start()
    {
        player.SetLevel(GetCurrentLevel());
        CharacterMenu.instance.UpdateMenu();


        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    public void SetTransparences()
    {
       if (SceneManager.GetActiveScene().name == "Main0")
        {
            // Establecer el color transparente (RGBA: 1, 1, 1, 0) => Transparente
            player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            weapon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            innercircle.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            outercircle.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            healthbar1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            healthbar2.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            menuButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            attackButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

        }
        else 
        {
            // Establecer el color blanco (RGBA: 1, 1, 1, 1) => Blanco opaco
            player.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            weapon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            innercircle.GetComponent<Image>().color = new Color(1f, 1f, 1f, 206f / 255f);
            outercircle.GetComponent<Image>().color = new Color(1f, 1f, 1f, 206f / 255f);
            healthbar1.GetComponent<Image>().color = new Color(137f / 255f, 11f/255f, 11f/255f, 1f);
            healthbar2.GetComponent<Image>().color = new Color(217f / 255f, 48f / 255f, 48f / 255f, 1f);
            menuButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            attackButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        }
    }

    public void StartNewGame()
    {
        if (File.Exists(archivoDeGuardado))
        {
            // Borra el archivo de guardado
            File.Delete(archivoDeGuardado);
            // Debug.Log("Archivo de guardado eliminado.");
        }

        // Inicia la coroutine para manejar la animación y la carga de escena
        StartCoroutine(PlayAnimationAndLoadScene());
    }

    private IEnumerator PlayAnimationAndLoadScene()
    {
        // Dispara el trigger de la animación
        newGameButton.GetComponent<Animator>().SetTrigger("Pressbutton");

        // Espera hasta que la animación se haya completado (ajusta según la duración de tu animación)
        yield return new WaitForSeconds(1f); // Ajusta el tiempo de espera según la duración de tu animación

        // Ahora carga la escena principal
        SceneManager.LoadScene("Main");
    }

    private IEnumerator ContinueAnimationAndLoadScene()
    {
        // Dispara el trigger de la animación
        continueGameButton.GetComponent<Animator>().SetTrigger("Pressbutton");

        // Espera hasta que la animación se haya completado (ajusta según la duración de tu animación)
        yield return new WaitForSeconds(1f); // Ajusta el tiempo de espera según la duración de tu animación
        if (File.Exists(archivoDeGuardado))
        {
            LoadData();
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo de guardado. No se puede continuar el juego.");
            // Opcional: Redirigir a otra escena o mostrar un mensaje en la interfaz de usuario
        }
 
    }

    public void ContinueGame()
    {
        StartCoroutine(ContinueAnimationAndLoadScene());

    }



    public void Exit()
    {

        #if UNITY_ANDROID
            // Cierra la aplicación completamente en Android
            SaveData();
            AndroidJavaObject activity = new AndroidJavaObject("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = activity.GetStatic<AndroidJavaObject>("currentActivity");
            context.Call("finish");
        #else
            // Cierra la aplicación en plataformas de escritorio
            SaveData();
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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        player.Respawn();
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        //player.transform.position = GameObject.Find("Spawn").transform.position;   
        SetTransparences();
        if (SceneManager.GetActiveScene().name == "Main0")
        {
            if (continueGameButton != null && !File.Exists(archivoDeGuardado))
            {
                continueGameButton.interactable = false; // Si es un botón de UI
                // desactivar botón
            }
        }
    }

    public void SaveData()
    {

        // Asigna los valores actuales a GameData
        gameData.currentScene = SceneManager.GetActiveScene().name;
        gameData.coins = coins; // Suponiendo que tienes una variable coins en GameManager
        gameData.experience = experience; // Suponiendo que tienes una variable experience
        gameData.weaponLevel = weapon.weaponLevel; // Suponiendo que tienes un objeto weapon en GameManager
        gameData.hitpoint = player.hitpoint;
        gameData.maxHitPoint = player.maxHitPoint;
        //gameData.posicionRespawn =  player.transform.position;

        //SaveEnemyData();

         // Recorremos todos los objetos de la escena (activos e inactivos)
        /*foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            // Solo procesamos aquellos que tienen el componente Item
            Enemy enemy = obj.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Creamos un nuevo objeto DatosItem para almacenar su información
                EnemyData enemyData = new EnemyData();
                enemyData.name = enemy.gameObject.name;
                enemyData.isAlive = enemy.isAlive;
                // Añadimos a la lista de inventarioItems
                gameData.enemyList.Add(enemyData);
            }
        }
        */
        // Serializa el estado del juego a JSON
        string json = JsonUtility.ToJson(gameData, true);
        // Guarda el JSON en un archivo
        File.WriteAllText(archivoDeGuardado, json);
        //Debug.Log("Partida guardada." + gameData.hitpoint);

    }

    public void LoadData()
    {
        // Verifica si el archivo existe
        if (File.Exists(archivoDeGuardado))
        {        

            // Lee el contenido del archivo JSON
            string json = File.ReadAllText(archivoDeGuardado);
            // Deserializa el estado del juego
            gameData = JsonUtility.FromJson<GameData>(json);

             // Restaurar los datos del jugador
            coins = gameData.coins;
            experience = gameData.experience;
            weapon.SetWeaponLevel(gameData.weaponLevel);
            player.hitpoint = gameData.hitpoint;
            player.maxHitPoint = gameData.maxHitPoint;   

        //Debug.Log("Partida cargada.");
            // Asigna la escena que se cargará
            string sceneToLoad = gameData.currentScene;

            // Cargar la escena y esperar a que se complete
            StartCoroutine(LoadSceneAndRestoreData(sceneToLoad));
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de guardado.");
        }
    }

    private IEnumerator LoadSceneAndRestoreData(string sceneToLoad)
    {
        // Cargar la nueva escena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Esperar hasta que la carga de la escena haya finalizado
        while (!asyncLoad.isDone)
        {
            yield return null; // Esperar hasta el siguiente frame
        }

        // Ahora que la escena está cargada, restaurar los datos del jugador
        RestorePlayerData();
    }

    private void RestorePlayerData()
    {
       

        // Llevar al player a la posición del objeto con el tag "ContinuePoint"
        GameObject continuePoint = GameObject.FindWithTag("continuepoint");
        if (continuePoint != null)
        {
            player.transform.position = continuePoint.transform.position; // Mover al jugador a la posición del continuePoint
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto con el tag 'ContinuePoint'. Asegúrate de que está en la escena.");
            // Puedes optar por usar una posición de respawn predeterminada aquí, si lo deseas
        }

        // Actualizar el menú y los puntos de vida
        CharacterMenu.instance.UpdateMenu();
        OnHitPointChange();
    }
    public void LoadEnemyData()
    {
        // Buscar todos los objetos de tipo Enemy en la escena actual
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        // Iterar sobre los datos de los enemigos guardados en gameData
        foreach (EnemyData enemyData in gameData.enemyList)
        {
            //Debug.Log("Cargando estado de enemigo: " + enemyData.name + " - Vivo: " + enemyData.isAlive);

            // Buscar el enemigo que coincida con el nombre guardado
            foreach (Enemy enemy in allEnemies)
            {
                if (enemy.name == enemyData.name)
                {
                    // Activar o desactivar el enemigo según su estado guardado
                    enemy.gameObject.SetActive(enemyData.isAlive);
                    enemy.isAlive = enemyData.isAlive;
                    //Debug.Log("Enemigo encontrado: " + enemy.name + " - Estado: " + enemyData.isAlive);
                    break;  // Salir del bucle una vez encontrado el enemigo
                }
            }
        }
    }

    public void SaveEnemyDeath(GameObject obj)
    {
        if (File.Exists(archivoDeGuardado))
        {
            // Si ya tienes enemigos guardados, asegúrate de que no los sobrescribes
            if (gameData.enemyList == null)
            {
                gameData.enemyList = new List<EnemyData>();
            }
            
            EnemyData enemyData = new EnemyData
            {
                name = obj.name,
                isAlive = false
            };
            
            gameData.enemyList.Add(enemyData);  // Añadir el estado del enemigo a la lista
        }
    }

    private void Update()
    {
    // Guardar la partida cuando se presiona F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            //Debug.Log("Partida guardada");
            SaveData();
        }

        // Cargar la partida cuando se presiona F6
        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadData();
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


