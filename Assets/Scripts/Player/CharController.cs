using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Mirror;
using Cinemachine;
using TMPro;

public class CharController : NetworkBehaviour
{
    [Header("Player Info")]
    [SyncVar(hook = nameof(OnDisplayNameChanged))] public string displayName;

    //Max
    public float healthMax = 100;
    public float manaMax = 100;
    public float staMax = 100;

    // Current
    [SyncVar] public float c_health = 1;
    public float health
    {
        get { return Mathf.Min(c_health, healthMax); } // min in case hp>hpmax after buff ends etc.
        set { c_health = Mathf.Clamp(value, 0, healthMax); }
    }
    public float c_mana;
    public float c_sta;

    [Header("Animations")]
    public Animator playerAnimator;
    public Animator WeaponAnimator;
    [SyncVar] public int lookDirection = 1;
    [SyncVar, HideInInspector] public Vector2 aimDirection;

    [Header("Auxiliar")]
    public GameObject weapon;
    public TMP_Text displayNameUI;
    [SyncVar, HideInInspector] public double nextRiskyActionTime = 0;

    [Header("Party")]
    [SyncVar, HideInInspector] public string partyInviteFrom = "";
    [SyncVar, HideInInspector] public Party party;
    public float partyInviteWaitSeconds = 3;

    public static CharController localPlayer;  
    

    public override void OnStartLocalPlayer()
    {
        // Set Name
        name = UserConfig.Instance.charData.username;
        displayName = UserConfig.Instance.charData.username;
        displayNameUI.text = UserConfig.Instance.charData.username;
        
        // onlinePlayers.Add(displayName, this);
        // Set Values
        CmdSetValues(displayName,healthMax);
        // Set local
        localPlayer = this;
        

        // Camera
        FindObjectOfType<CinemachineVirtualCamera>().Follow = transform;
    }

    void Start(){
       
    }
    

    void OnDestroy()
    {

         if (NetworkServer.active) // isServer
        {
            // leave party (if any)
            if (InParty())
            {
                // dismiss if master, leave otherwise
                if (party.master == displayName)
                    PartyDismiss();
                else
                    PartyLeave();
            }
        }

        if(isLocalPlayer){
            localPlayer = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimDirection = new Vector2(mousePos.x - transform.position.x,mousePos.y - transform.position.y).normalized;

            if(Input.GetMouseButtonDown(0)){
                gameObject.GetComponent<CharAttack>().Attack();
            }
            if(Input.GetKeyDown(KeyCode.Space)){    
                // TakeDamage(1);
                Debug.Log(party.members == null);
            }

            gameObject.GetComponent<CharMovement>().HandleMovement(aimDirection,playerAnimator);

        }
    }
   

    void SetWeaponRotation(){
        if(isLocalPlayer){
            Vector2 spritePosition = new Vector2(weapon.transform.position.x,weapon.transform.position.y);

            var dir = aimDirection - spritePosition;
            
            if(lookDirection >= 0){
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                angle = Mathf.Clamp(angle,-92,92);
                Quaternion weaponRot = Quaternion.AngleAxis(angle,Vector3.forward);

                // Debug.Log(weaponRot.eulerAngles.z);
            }
        }
    }

    
    private void OnDisplayNameChanged(string oldValue, string newValue)
    {
        displayNameUI.text = newValue;
        transform.name = newValue;
    }

    public bool InParty()
    {
        // 0 means no party, because default party struct's partyId is 0.
        return party.partyId > 0;
    }


    // Command Functions

    [Command]
    public void TakeDamage(float damage){
        this.c_health -= damage; 
    }

    [Command]
    private void CmdSetValues(string nameA,float health)
    {
        displayName = nameA;
        c_health = health;
    }

    [Command]
    public void CmdPartyInviteAccept()
    {
        if (!InParty() && partyInviteFrom != "")
        {
            foreach (var item in NetworkServer.spawned)
            {
                if(item.Value.tag == "Player"){
                    if(partyInviteFrom == item.Value.gameObject.GetComponent<CharController>().displayName){
                        CharController other = item.Value.gameObject.GetComponent<CharController>();
                        
                        // is in party? then try to add
                        if (other.InParty())
                            PartySystem.AddToParty(other.party.partyId, displayName);
                        
                        else
                            PartySystem.FormParty(other.displayName, displayName);
                    }
                }
            }
        }

        // reset party invite in any case
        partyInviteFrom = "";
    }

    [Command]
    public void CmdPartyInviteDecline()
    {
        partyInviteFrom = "";
    }

    [Command]
    public void CmdPartyInvite(string otherName)
    {
        foreach (var item in NetworkServer.spawned)
        {
            if(item.Value.tag == "Player"){
                if(otherName == item.Value.gameObject.GetComponent<CharController>().displayName){
                    CharController other = item.Value.gameObject.GetComponent<CharController>();

                    if (otherName != displayName  && NetworkTime.time >= nextRiskyActionTime){

                        if ((!InParty() || !party.IsFull()) && !other.InParty())
                        {
                            other.partyInviteFrom = displayName;

                            print(displayName + " invited " + other.displayName + " to party");
                        }
                    }

                }
            }            
        }
        nextRiskyActionTime = NetworkTime.time + partyInviteWaitSeconds;
    }

    // version without cmd because we need to call it from the server too
    public void PartyLeave()
    {
        // try to leave. party system will do all the validation.
        PartySystem.LeaveParty(party.partyId, displayName);
    }
    [Command]
    public void CmdPartyLeave() { PartyLeave(); }

    // version without cmd because we need to call it from the server too
    public void PartyDismiss()
    {
        // try to dismiss. party system will do all the validation.
        PartySystem.DismissParty(party.partyId, displayName);
    }
    [Command]
    public void CmdPartyDismiss() { PartyDismiss(); }
}
