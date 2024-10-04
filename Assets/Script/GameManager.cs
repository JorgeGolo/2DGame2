using System;
using System.Collections;
using System.Collections.Generic;
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

    


    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
          // to start with no data
        // PlayerPrefs.DeleteAll();   

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    void Start()
    {
        player.SetLevel(GetCurrentLevel());
                CharacterMenu.instance.UpdateMenu();

    }



     public void Exit()
    {
        #if UNITY_EDITOR
            // Detener el juego si está en el editor de Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_ANDROID
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
        // "0|10|15" -> 

        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        //weapon.weaponLevel = int.Parse(data[3]);
        weapon.SetWeaponLevel(int.Parse(data[3]));
        /*if(GetCurrentLevel()!=1)
        {
            player.SetLevel(GetCurrentLevel());
        }*/
        //player.SetLevel(GetCurrentLevel());

        
        
        //Debug.Log("Level" + GetCurrentLevel());

    }

    private void Update()
    {
        //Debug.Log(GetCurrentLevel());
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


