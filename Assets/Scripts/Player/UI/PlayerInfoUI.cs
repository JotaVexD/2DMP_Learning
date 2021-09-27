using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    public GameObject panel;
    public Slider healthSlider;
    public TMP_Text healthStatus;
    public TMP_Text username;

    void Update()
    {
        CharController player = CharController.localPlayer;
        if (player)
        {
            // panel.SetActive(true);
            healthSlider.maxValue = player.healthMax;
            username.text = player.displayName;

            healthSlider.value = player.c_health;
            healthStatus.text = player.health + " / " + player.healthMax;
        }
        // else panel.SetActive(false);
    }
}
