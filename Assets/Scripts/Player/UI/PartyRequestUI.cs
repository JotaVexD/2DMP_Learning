// Note: this script has to be on an always-active UI parent, so that we can
// always find it from other code. (GameObject.Find doesn't find inactive ones)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class PartyRequestUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text nameText;
    public Button acceptButton;
    public Button declineButton;

    void Update()
    {
        CharController player = CharController.localPlayer;
        // only if there is an invite
        if (player != null && player.partyInviteFrom != "")
        {
            panel.SetActive(true);
            nameText.text = player.partyInviteFrom;
            acceptButton.onClick.AddListener(() => {
                player.CmdPartyInviteAccept();
            });
            declineButton.onClick.AddListener(() => {
                player.CmdPartyInviteDecline();
            });
        }
        else panel.SetActive(false);
    }
}
