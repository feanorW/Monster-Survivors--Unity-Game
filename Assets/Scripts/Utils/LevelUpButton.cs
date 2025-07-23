using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{
    public TMP_Text weaponName;
    public TMP_Text weaponDescription;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    public void ActivateButton(Weapon weapon){
        weaponName.text = weapon.name + "\nLevel"+ (weapon.weaponLevel +1);
        weaponDescription.text = weapon.stats[weapon.weaponLevel].description;
        weaponIcon.sprite = weapon.weaponImage;
        assignedWeapon = weapon;
        gameObject.SetActive(true);
    }

    public void SelectUpgrade()
    {
        assignedWeapon.LevelUp();
        UIController.Instance.UpdateWeaponSlot(assignedWeapon); // Update the weapon slot panel in the UI
        UIController.Instance.LevelUpPanelClose();
        AudioManager.Instance.PlaySound(AudioManager.Instance.selectUpgrade);
    }
}
