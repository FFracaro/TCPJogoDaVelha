using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorReport : MonoBehaviour
{
    [SerializeField]
    GameObject ErrorUI;

    [SerializeField]
    TMP_Text ErrorText;

    public void ShowSocketError(string msg)
    {
        if(!ErrorUI.activeSelf)
        {
            ErrorText.text = msg;
            ErrorUI.SetActive(true);
        }      
    }

    public void ClientConnectionError(string msg)
    {
        if (!ErrorUI.activeSelf)
        {
            ErrorText.text = msg;
            ErrorUI.SetActive(true);
        }
    }
}
