using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;


public class CharHUD : NetworkBehaviour
{
    [Header("PlayerInfo")]
    [SyncVar(hook = nameof(HealthSync))] public float m_CurrentHealth = 0;
    public Slider healthBar;
    public TMP_Text healthText;

    [Header("PartyInvite")]
    private Button inviteParty;
    private Button sendInvite;
    private TMP_InputField username;

    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer){
            healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
            healthText = GameObject.Find("TextHealth").GetComponent<TMP_Text>();
            inviteParty = GameObject.Find("AddParty").GetComponent<Button>();
            inviteParty.onClick.AddListener(partyInvite);
            GameObject.Find("Username").GetComponent<TMP_Text>().text = UserConfig.Instance.charData.username;
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

    public void partyInvite(){
        
        GameObject partyInviteUI = inviteParty.transform.Find("PartyInviteUI").gameObject;
        Debug.Log(NetworkClient.localPlayer.name);

        if(!partyInviteUI.activeSelf){
            partyInviteUI.SetActive(true);
            username = partyInviteUI.transform.Find("UsernameInput").GetComponent<TMP_InputField>();
            sendInvite = partyInviteUI.transform.Find("InviteButton").GetComponent<Button>();
            sendInvite.onClick.AddListener(delegate {inviteSend(username.text); });
        }else if(partyInviteUI.activeSelf){
            username = null;
            sendInvite = null;
            partyInviteUI.SetActive(false);
            
        }

    }

    public void inviteSend(string name){
        Debug.Log(name);
    }
}
