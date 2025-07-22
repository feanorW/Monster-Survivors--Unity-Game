using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseHealthButton : MonoBehaviour
{
    public TMP_Text textName;
    public TMP_Text textDescription;
    public Image hpIcon;

    private int healthIncrease;

    public void ActivateButton(int healthIncrease)
    {
        this.healthIncrease = healthIncrease;
        textName.text = "Increase Max Health";
        textDescription.text = "+" + healthIncrease + " HP";
        hpIcon.sprite = UIController.Instance.healthIcon; // Assuming you have a health icon in your UIController
    }

    public void SelectUpgrade()
    {
        PlayerController.Instance.IncreaseMaxHealth(healthIncrease);
        UIController.Instance.LevelUpPanelClose();
        AudioManager.Instance.PlaySound(AudioManager.Instance.selectUpgrade);
    }
}
