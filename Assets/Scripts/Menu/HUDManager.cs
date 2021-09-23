using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_Text errorText;
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }
        //Will be called from Login Button
    public void LoginButton()
    {
        
        NetworkManager.singleton.StartClient();
        PlayerPrefs.SetString("Name", username.text);
        gameObject.SetActive(false);
    }

    public void DEBUG_CLIENTANDSERVER()
    {
        NetworkManager.singleton.StartHost();
        PlayerPrefs.SetString("Name", username.text);
        gameObject.SetActive(false);
    }
}
