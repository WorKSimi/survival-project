using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HostClientManager : MonoBehaviour
{
    public bool IsHost;
    public string clientJoinCode;
    public TMP_InputField joinCodeInputField;

    public void Start()
    {
        DontDestroyOnLoad(this); //Dont Destroy this on load
        IsHost = false;
    }

    public void PlayerClickedHost()
    {
        IsHost = true;
    }

    public void PlayerClickedClient()
    {
        IsHost = false;
    }

    public void JoinGameButton()
    {
        clientJoinCode = joinCodeInputField.text;
        SceneManager.LoadScene(1);
    }
}
