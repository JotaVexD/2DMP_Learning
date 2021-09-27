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

    [Header("Party")]
    [SyncVar, HideInInspector] public string partyInviteFrom = "";
    [SyncVar, HideInInspector] public Party party;

    public static CharController localPlayer;  

    [Command]
    public void CmdSyncaimDirection(Vector2 cursor){
        aimDirection = cursor;
    } 

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
                TakeDamage(1);
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
            CharController playerInvite = GameObject.Find(partyInviteFrom).GetComponent<CharController>();
            // is in party? then try to add
            if (playerInvite.InParty())
                PartySystem.AddToParty(playerInvite.party.partyId, name);
            // otherwise try to form a new one
            else
                PartySystem.FormParty(playerInvite.name, name);
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
        if(GameObject.Find(otherName) != null){
            CharController player = GameObject.Find(otherName).GetComponent<CharController>();
            // validate: is there someone with that name, and not self?
            if (otherName != name && player)
            {
                // can only send invite if no party yet or party isn't full and
                // have invite rights and other guy isn't in party yet
                // if ((!InParty() || !party.IsFull()) && !other.InParty())
                // {
                    // send a invite
                    player.partyInviteFrom = name;

                    print(name + " invited " + player.name + " to party");
                // }
            }
        }

    }


    
}
