using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text LoadingServerMessage;

    [SerializeField]
    RectTransform LoadingServerPanel;

    [SerializeField]
    GameObject IconsLoadingAnimation;

    [SerializeField]
    GameObject IconsLoadingAnimationBackground;

    [SerializeField]
    GameObject StartGameButton;

    [SerializeField]
    float LoadingServerTweenSpeed = 0f;

    [SerializeField]
    GameObject PlayBoardUI;

    [SerializeField]
    GameObject ResultUI;

    [SerializeField]
    RectTransform ResultMessagePanel;

    [SerializeField]
    RectTransform ResultButtons;

    [SerializeField]
    TMP_Text ResultMessage;

    [SerializeField]
    bool IsServer = true;

    private bool ServerOn = false;
    private bool ClientOn = false;

    private void Start()
    {
        if (IsServer)
            StartCoroutine(LoadingServerUI());
        else
            StartCoroutine(LoadingClientUI());
    }

    public void ServerUICommunication(ServerMessageType msgType, string msg)
    {
        if (msgType == ServerMessageType.ERROR)
            ShowErrorMessage(msg);

        if (msgType == ServerMessageType.SERVERON)
        {
            UpdateLoadingServerUI(msg);
            ServerOn = true;
        }
    }

    public void ClientUICommunication(ServerMessageType msgType, string msg)
    {
        if (msgType == ServerMessageType.ERROR)
            ShowErrorMessage(msg);

        if (msgType == ServerMessageType.CLIENTON)
        {
            UpdateLoadingServerUI(msg);
            ClientOn = true;
        }
    }

    public void UpdateLoadingServerUI(string message)
    {
        LoadingServerMessage.text = message;
    }

    public void ShowErrorMessage(string msg)
    {
        ServerOn = false;
        ClientOn = false;
        // Update error ui and show it
    }

    public void ClientConnectedUpdateLoadingServerUI()
    {

        IconsLoadingAnimation.SetActive(false);
        IconsLoadingAnimationBackground.SetActive(false);
        StartGameButton.SetActive(true);

        UpdateLoadingServerUI("Adversário conectado.");
    }

    IEnumerator LoadingServerUI()
    {
        yield return new WaitForSeconds(0.3f);

        LoadingServerPanel.DOAnchorPos(Vector2.zero, LoadingServerTweenSpeed);

        yield return new WaitForSeconds(1f);

        IconsLoadingAnimation.SetActive(true);

        yield return new WaitForSeconds(2f);

        FindObjectOfType<TCPServer>().InitiateThreadServer();

        while(!ServerOn)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        UpdateLoadingServerUI("Esperando adversário.");
    }

    IEnumerator LoadingClientUI()
    {
        yield return new WaitForSeconds(0.3f);

        LoadingServerPanel.DOAnchorPos(Vector2.zero, LoadingServerTweenSpeed);

        yield return new WaitForSeconds(1f);

        IconsLoadingAnimation.SetActive(true);

        yield return new WaitForSeconds(2f);

        FindObjectOfType<TCPClient>().ConnectToTcpServer();

        while (!ClientOn)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        UpdateLoadingServerUI("Esperando início da partida.");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }

    public void StartGame()
    {
        StartCoroutine(CloseLoadingServerPanel());

        GameBehaviour Behaviour = FindObjectOfType<GameBehaviour>();

        Behaviour.StartGame();
    }

    public void CloseLoadingClientPanel()
    {
        StartCoroutine(CloseLoadingServerPanel());
    }

    IEnumerator CloseLoadingServerPanel()
    {
        LoadingServerPanel.DOAnchorPos(new Vector2(0, 750), LoadingServerTweenSpeed);

        yield return new WaitForSeconds(0.5f);

        LoadingServerPanel.gameObject.SetActive(false);

        PlayBoardUI.SetActive(true);

    }

    public void ShowResults(string message)
    {
        ResultMessage.text = message;

        StartCoroutine(ShowResultPanel());
    }

    IEnumerator ShowResultPanel()
    {
        ResultUI.SetActive(true);

        ResultMessagePanel.DOAnchorPos(new Vector2(0, 0), LoadingServerTweenSpeed);

        ResultButtons.DOAnchorPos(new Vector2(0, 50), LoadingServerTweenSpeed);

        yield return null;
    }

    public void ReturnToMainScreen()
    {
        if (IsServer)
            FindObjectOfType<TCPServer>().CloseServer();
        else
            FindObjectOfType<TCPClient>().CloseClient();

        StartCoroutine(LoadYourAsyncScene("TelaInicial"));
    }

    IEnumerator LoadYourAsyncScene(string SceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

public enum ServerMessageType
{
    ERROR,
    SERVERON,
    NEWCLIENT,
    CLIENTON
}
