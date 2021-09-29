using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public partial class HUDPartyUI : MonoBehaviour
{
    public GameObject panel;
    public PartyMemberSlotUI slotPrefab;
    public Transform memberContent;
    //[Range(0,1)] public float visiblityAlphaRange = 0.5f;

    void Update()
    {
        CharController player = CharController.localPlayer;

        // only show and update while there are party members
        if (player != null && player.InParty())
        {
            panel.SetActive(true);
            Party party = player.party;

            // get party members without self. no need to show self in HUD too.
            List<string> members = player.InParty() ? party.members.Where(m => m != player.name).ToList() : new List<string>();

            // instantiate/destroy enough slots
            UIUtils.BalancePrefabs(slotPrefab.gameObject, members.Count, memberContent);

            // refresh all members
            for (int i = 0; i < members.Count; ++i)
            {
                PartyMemberSlotUI slot = memberContent.GetChild(i).GetComponent<PartyMemberSlotUI>();
                string memberName = members[i];

                slot.username.text = memberName;


                // pull health, mana, etc. from observers so that party struct
                // doesn't have to send all that data around. people will only
                // see health of party members that are near them, which is the
                // only time that it's important anyway.
                foreach (var item in NetworkServer.spawned)
                {
                    if(item.Value.tag == "Player"){
                        if(memberName == item.Value.gameObject.GetComponent<CharController>().displayName){
                            CharController member = item.Value.gameObject.GetComponent<CharController>();
                            slot.healthSlider.value = member.c_health;
                            slot.healthStatus.text = member.c_health + " / " + member.healthMax;
                        }
                    }
                }
            }
        }
        else panel.SetActive(false);
    }
}
