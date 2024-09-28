using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text levelText, hitpointText, coinsText, upgradeCostText, xpText;

    private int currectCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

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
        levelText.text = "not implemented";

        xpText.text = "not implemented";
        xpBar.localScale = new Vector3(0.5f,0,0);

    }
}
