using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyMember
{
    // public Sprite CharPhoto;
    public TMP_Text CharName;
    public TMP_Text healthBar;
    public TMP_Text manaBar;
    public TMP_Text staminaBar;

    public PartyMember(string name, float maxHealth,float currentHealth,float maxMana,float currentMana,float maxSta,float currentSta){
        // CharPhoto = photo;
        CharName.text = name;
        healthBar.text = currentHealth + " / " + maxHealth;
        manaBar.text = currentMana + " / " + maxMana;
        staminaBar.text = currentSta + " / " + maxSta;
    }
}
