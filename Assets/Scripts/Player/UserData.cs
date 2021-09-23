using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UserData", menuName = "Slash 2D MMORPG/UserData", order = 0)]
public class UserData : ScriptableObject
{
    public string Username{get;set; }
    public int Id{get;set; }
    public string IPAdrress{get;set; }


}
