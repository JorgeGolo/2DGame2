using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text levelText, hitpointText, coinsText, upgradeCostText, xpText;

    private int currectCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    public static CharacterMenu instance;

    // Start is called before the first frame update


    void Awake()
    {
        if(CharacterMenu.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnArrowClick(bool right)
    {
        if(right)
        {
            currectCharacterSelection++;
            if(currectCharacterSelection == GameManager.instance.playerSprites.Count)
            {
                currectCharacterSelection = 0;
            }
            OnSelectionChange();
        }
        else
        {
            currectCharacterSelection--;
            if(currectCharacterSelection < 0)
            {
                currectCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }
            OnSelectionChange();
        }
    }

    public void OnSelectionChange()
    
    {
        characterSelectionSprite.sprite =  GameManager.instance.playerSprites[currectCharacterSelection];
        GameManager.instance.player.SwapSprite(currectCharacterSelection);
    }

    public void OnUpgradeClick()
    {
        if(GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }
    public void UpdateMenu()
    {

        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if(GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
        {
            upgradeCostText.text = "MAX";
        }
        else
        {
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        }
        

        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        
        coinsText.text = GameManager.instance.coins.ToString();

        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        int currLevel = GameManager.instance.GetCurrentLevel();
        if(GameManager.instance.GetCurrentLevel() == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;

        } 
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLovel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLovel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float ratio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(ratio,1,1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;


        }
        
        //xpText.text = "not implemented";
        //xpBar.localScale = new Vector3(0.5f,0,0);

    }


}
