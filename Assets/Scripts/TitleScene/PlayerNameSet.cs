using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerNameSet : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private GameObject popup;

    public void SetName()
    {
        string playerName = playerNameInputField.text;
        if(playerName.Length <= 0)
        {
            playerName = "�̿�¯¯��";
        }
        SavePlayerInfo.instance.playerInfo.playerName = playerName;
        popup.SetActive(false);
        Loader.LoadScene("StageSelectScene");
        return;
    }
}
