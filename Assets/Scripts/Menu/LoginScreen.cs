using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class LoginScreen : MonoBehaviour
{
    public CharacterData clientData;
    [SerializeField] private NetworkManager2DMP networkManager;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private Canvas HUD;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        InputFieldFunction(username.text);
    }

    public void InputFieldFunction(string input)
    {
        if(!loginButton.gameObject.activeSelf) loginButton.gameObject.SetActive(true);

        if (string.IsNullOrWhiteSpace(input))
        {
            errorText.text = "Name cannot be empty or contains only space!";
            loginButton.interactable = false;
        } 
        else if(input.Length > 12)
        {
            errorText.text = "Name cannot be longer than 16 characters!";
            loginButton.interactable = false;
        }
        else
        {
            errorText.text = "";
            loginButton.interactable = true;
            clientData.username = input;
        }
    }
        //Will be called from Login Button
    public void LoginButton()
    {
        UserConfig.Instance.charData.username = clientData.username;
        networkManager.StartClient();
        // NetworkManager.singleton.StartClient();
        // PlayerPrefs.SetString("Name", username.text);
        HUD.gameObject.SetActive(false);
    }

    public void DEBUG_CLIENTANDSERVER()
    {
        UserConfig.Instance.charData.username = clientData.username;
        networkManager.StartHost();
        // PlayerPrefs.SetString("Name", username.text);
        HUD.gameObject.SetActive(false);
    }
}


[Serializable]
public class CharacterData
{
    public string username;
}
