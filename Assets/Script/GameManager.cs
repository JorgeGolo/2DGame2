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
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    //public Weapon weapon...

    //Logic

    public int coins;
    public int experience;

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

    // so we can use GameManager.instance.ShoeText()

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

        // change player skin

        //

        // change coins, xp parsing

        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        //weapon.weaponLevel = int.Parse(data[3]);
        weapon.SetWeaponLevel(int.Parse(data[3]));
        if(GetCurrentLevel()!=1)
        {
            player.SetLevel(GetCurrentLevel());
        }
        
        // change weapon level

        //

        player.transform.position = GameObject.Find("Spawn").transform.position;
        
        Debug.Log("Load");
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
    }
}


