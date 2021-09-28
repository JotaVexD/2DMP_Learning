using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyInviteUI : MonoBehaviour
{
    public Button inviteParty;
    public Button sendInvite;
    public TMP_InputField username;
    private bool invSend = false;

    void Update(){
        CharController player = CharController.localPlayer;
            
        if (player != null && !invSend)
        {
            if(username.text != ""){
                OnInviteSend(player);
            }
        }
        
    }

    void OnInviteSend(CharController player){
        sendInvite.onClick.AddListener(delegate {player.CmdPartyInvite(username.text); });
        return;
    }

    public void OpenPanel(){
        // foreach (var res in CharController.onlinePlayers)
        // {
        //     Debug.Log(res.Key);
        // }
        if(!gameObject.activeSelf){
            gameObject.SetActive(true);
        }else if(gameObject.activeSelf){
            gameObject.SetActive(false);
        }
    }

}
