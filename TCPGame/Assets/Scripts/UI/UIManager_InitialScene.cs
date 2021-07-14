using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager_InitialScene : MonoBehaviour
{
    [SerializeField]
    TMP_InputField IpAddress;

    [SerializeField]
    TMP_InputField Port;

    [SerializeField]
    GameObject ErrorScreen;

    [SerializeField]
    TMP_Text ErrorText;

    public void LoadGameScene(int SceneNumber)
    {
        var ip = IpAddress.text;
        var port = Port.text;

        Validations Val = GetComponent<Validations>();

        if (Val.ValidateIpAndPort(ip, port))
        {
            SaveServerInfo(ip, port);

            if (SceneNumber == 0)
            {
                Debug.Log("SERVER");
                StartCoroutine(LoadYourAsyncScene("ServerScene"));
            }
            else
            {
                Debug.Log("CLIENT");

                StartCoroutine(LoadYourAsyncScene("ClientScene"));
            }
        }
    }

    private void SaveServerInfo(string ip, string port)
    {
        FindObjectOfType<InformationHolder>().AddIpAddressAndPort(ip, int.Parse(port));
    }

    IEnumerator LoadYourAsyncScene(string SceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void ShowErrorPopUpMessage(string errorMsg)
    {
        ErrorText.text = errorMsg;

        if(!ErrorScreen.activeSelf)
        {
            ErrorScreen.SetActive(true);
        }
    }

    public void CloseErrorWindow()
    {
        if (ErrorScreen.activeSelf)
            ErrorScreen.SetActive(false);
    }

    public void CloseGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }
}
