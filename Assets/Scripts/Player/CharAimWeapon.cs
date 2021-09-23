using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharAimWeapon : NetworkBehaviour
{
    public Transform aimTransform;
    public Transform weapon;
    [SyncVar, HideInInspector] public Vector2 aimDirection;

    private bool aimingRight = true;
    public Canvas UI;
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if(isLocalPlayer){
            HandleAim();
        }
        
    }

    private void HandleAim(){
        if(isLocalPlayer){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimDirection = new Vector2(mousePos.x - transform.position.x,mousePos.y - transform.position.y);

            Vector2 spritePosition = new Vector2(aimTransform.localPosition.x,aimTransform.localPosition.y);

            var dir = aimDirection - spritePosition;       

            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            Quaternion weaponRot = Quaternion.AngleAxis(angle,Vector3.forward);

            aimTransform.eulerAngles = new Vector3(0,0,weaponRot.eulerAngles.z);

            if(aimDirection.x <= 0 && aimingRight){
                Flip();
            }else if(aimDirection.x > 0 && !aimingRight){
                Flip();
            }

        }
    }

    private void Flip(){
        if(isLocalPlayer){
            aimingRight = !aimingRight;
            Vector3 weaponPosition =  weapon.transform.localPosition;
            weaponPosition.x *= -1;
            weapon.transform.localPosition = weaponPosition;

            Vector3 weaponScale =  weapon.transform.localScale;
            weaponScale.y *= -1;
            weapon.transform.localScale = weaponScale;

            Vector3 UITransform =  UI.transform.localScale;
            UITransform.x *= -1;
            UI.transform.localScale = UITransform;

        }
    }
}
