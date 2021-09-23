using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using TMPro;

public class CharController : NetworkBehaviour
{
    [Header("Player Info")]
    [SyncVar(hook = nameof(OnDisplayNameChanged))]
    private string displayName;
    
    public Animator playerAnimator;
    public Animator WeaponAnimator;
    [SyncVar] public int lookDirection = 1;
    [SyncVar, HideInInspector] public Vector2 aimDirection;

    [Header("Auxiliar")]
    public GameObject weapon;
    public TMP_Text displayNameUI;
    

    [Command]
    public void CmdSyncaimDirection(Vector2 cursor){
        aimDirection = cursor;
    } 

    public override void OnStartLocalPlayer()
    {
        CmdChangeName(PlayerPrefs.GetString("Name"));
        FindObjectOfType<CinemachineVirtualCamera>().Follow = transform;
    }

    void Start() {

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
    }

    [Command]
    private void CmdChangeName(string name)
    {
        displayName = name;
    }
}
