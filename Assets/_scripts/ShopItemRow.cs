using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopItemRow : MonoBehaviour {

    private Inventory playerInventory;      //Reference to players inventory
    private PlayerManager player;           //Reference to player
    public Text info;                       //Reference to ui text of info box
    public Text lowMoneyText;               //Reference to ui text of "failed to upgrade" popup
    public Text upgradeLevel;               //Reference to ui text that shows the upgrades progress
    public Text upgradePriceText;           //Reference to ui text of the price of upgrade
    public Animator buyAnimator;            //Reference to buy buttons popup animator
    public Animator infoAnimator;           //Reference to info buttons popup animator
    public Upgrade upgrade;                 //The upgrade of this row
    public PopUpController lowMoney;
    public PopUpController infoBox;


    public string infoText;

	// Use this for initialization
	void Start () {
        player = Initializer.instance.player;
        playerInventory = player.inventory;
        infoAnimator.Play("default");
	}
	
    /// <summary>
    /// Updates the texts on the popup windows
    /// </summary>
	void Update () {
        if (info.IsActive())
            infoBox.UpdateText(infoText);
        if (lowMoneyText.IsActive()) {
            lowMoney.UpdateText("You need $" + (playerInventory.GetUpgradePrice(upgrade) - player.Money).ToString() +" more to upgrade");
        }
        upgradeLevel.text = playerInventory.GetUpgradeLevel(upgrade);
        int price = playerInventory.GetUpgradePrice(upgrade);
        upgradePriceText.text = price == 0 ? "FULL" : "$" + price.ToString();
	}

    /// <summary>
    /// Disables the info popup
    /// </summary>
    public void HideInfoBox() {
        infoBox.HidePopup();
        //infoAnimator.Play("hidepopup");
    }

    /// <summary>
    /// Enables the info popup
    /// </summary>
    public void ShowInfoBox() {
        infoBox.ShowPopup();
        //infoAnimator.Play("popup");
    }

    /// <summary>
    /// Hides the "failed to upgrade" window
    /// </summary>
    public void HideLowMoney() {
        lowMoney.HidePopup();
        //buyAnimator.Play("hidepopup");
    }

    /// <summary>
    /// Upgrades the item of this row if player has enough money
    /// Else shows the popup
    /// </summary>
    public void BuyUpgrade() {
        if (!playerInventory.UpgradeItem(upgrade)) {
            lowMoney.ShowPopup();
            //buyAnimator.Play("popup");
        }
    }
}
