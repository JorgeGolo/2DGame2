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

    }
    public void UpdateMenu()
    {

        weaponSprite.sprite = GameManager.instance.weaponSprites[0];
        upgradeCostText.text = "not implemented";

        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        coinsText.text = GameManager.instance.coins.ToString();
        levelText.text = "not implemented";

        xpText.text = "not implemented";
        xpBar.localScale = new Vector3(0.5f,0,0);

    }
}
