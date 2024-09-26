using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // to start with no data
        // PlayerPrefs.DeleteAll();

        instance = this;
        SceneManager.sceneLoaded += LoadState;

        DontDestroyOnLoad(gameObject);
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    

    // References

    public Player player;
    //public Weapon weapon...

    //Logic

    public int coins;
    public int experience;

    // save state

    /*
    
        INT preferedSkin
        INt coins
        INT experience
        INT weaponLevel
    
    */
    public void SaveState()
    {

        Debug.Log("Save");

        string es = "";

        es += "0" + "|"; 
        es += coins.ToString() + "|";
        es += experience.ToString() + "|";
        es += "0"; 

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

        // change player skin

        //

        // change coins, xp parsing

        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        
        // change weapon level

        //
        
        Debug.Log("Load");
    }
}


