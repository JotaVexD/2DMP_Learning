using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyInviteUI : MonoBehaviour
{
    public Button inviteParty;
    public Button sendInvite;
    public TMP_InputField username;

    void Update(){
        CharController player = CharController.localPlayer;
        if (player != null)
        {
            if(username.text != ""){
                sendInvite.onClick.AddListener(delegate {player.CmdPartyInvite(username.text); });
            }
        }
        
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
