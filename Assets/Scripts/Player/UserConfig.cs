using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserConfig : MonoBehaviour
{
    private static UserConfig _instance;
    public static UserConfig Instance
    {
        get
        {
            if(_instance == null)
            {
                throw new System.Exception("No UserConfig File");
            }
            return _instance;
        }
    }
    public CharData charData;

    void Awake() {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
