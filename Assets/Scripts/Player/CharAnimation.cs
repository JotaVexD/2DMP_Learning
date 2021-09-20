using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharAnimation : NetworkBehaviour
{
    public void SetMoveValue(Vector2 aimDirection,Vector3 V3,Animator animation){
        animation.SetFloat("MouseX",aimDirection.x);
        animation.SetFloat("MouseY",aimDirection.y);
        animation.SetFloat("Magnitude",V3.magnitude);

    }

//     public IEnumerator SetAttackDirection(){

//     }

    public void Attack(Vector2 aimDir){
        Debug.Log(aimDir);
    }
}
