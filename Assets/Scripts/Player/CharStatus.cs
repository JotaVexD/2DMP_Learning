using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;


public class CharStatus : NetworkBehaviour
{
    [SyncVar(hook = nameof(HealthSync))] public float m_CurrentHealth = 0;
    public Slider healthBar;
    public TMP_Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer){
            healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
            healthText = GameObject.Find("TextHealth").GetComponent<TMP_Text>();
            m_CurrentHealth = healthBar.value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer){
            healthText.text = m_CurrentHealth + " / " + healthBar.maxValue;
            healthBar.value = m_CurrentHealth;
        }
    }

    public void TakeDamage(float value){
        m_CurrentHealth -= value;
    }

    void HealthSync(float _Old, float _New)
    {
        healthBar.value = m_CurrentHealth;
    }
}
