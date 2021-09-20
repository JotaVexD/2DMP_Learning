using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharMovement : NetworkBehaviour
{  
    public Rigidbody2D rb;
    public void HandleMovement(Vector2 aimDirection,Animator animation){
        if(isLocalPlayer){
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 moveDelta = new Vector3(x,y,0).normalized;

            if(aimDirection.x > 0){
                transform.localScale = Vector3.one;
            }else if(aimDirection.x < 0){
                transform.localScale = new Vector3(-1,1,0);
            }

            gameObject.GetComponent<CharAnimation>().SetMoveValue(aimDirection,moveDelta,animation);
            // transform.Translate(moveDelta * Time.deltaTime);
            rb.velocity = new Vector2(moveDelta.x,moveDelta.y);
 
        }
    }
}
