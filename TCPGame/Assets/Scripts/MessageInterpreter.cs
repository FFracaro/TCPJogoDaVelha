using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageInterpreter : MonoBehaviour
{
    // Message format:
    // CODE:<OPTIONAL>:<OPTIONAL>
    // 1000:15:7
    public void ParseMessageReceived(string message)
    {
        string[] Messages = message.Split(':');

        ExecuteCode(VerifyCode(Messages[0]), Messages);
    }

    private void ExecuteCode(int code, string[] messages)
    {
        Debug.Log("Código recebido:" + code);

        switch(code)
        {
            case 100:
                ExecuteStartGame(messages);
                break;

            case 101:
                ExecuteNewTurn(messages);
                break;

            case 111:
                break;
        }
    }

    private void ExecuteStartGame(string[] messages) 
    {
        int pieceColor = int.Parse(messages[1]);
        int whoGoesFirst = int.Parse(messages[2]);

        Debug.Log("Who goes first: " + whoGoesFirst);

        FindObjectOfType<UIManager>().CloseLoadingClientPanel();

        GetComponent<GameBehaviour>().StartGameClient(pieceColor, whoGoesFirst);
    }

    private void ExecuteUpdateBoard(int position, int color)
    {
        GetComponent<GameBehaviour>().UpdateBoardScore(position, color);

        FindObjectOfType<BoardUIManager>().UpdateBoard(position, color);
    }

    private void ExecuteNewTurn(string[] messages)
    {
        int position = int.Parse(messages[1]);
        int color = int.Parse(messages[2]);
        int win = int.Parse(messages[3]);

        UIManager MainUI = FindObjectOfType<UIManager>();

        ExecuteUpdateBoard(position, color);

        if (win == 2)
        {
            MainUI.ShowResults("Empate.");
        }
        else if(win == 1)
        {
            MainUI.ShowResults("Derrota.");
        }
        else
        {
            GetComponent<GameBehaviour>().StartTurn();

        }
    }

    private int VerifyCode(string code)
    {
        return int.Parse(code);
    }

    public void ComposeMessageStartGame(int pieceColor, int whoGoesFirst)
    {
        string message = "100:" + pieceColor + ":" + whoGoesFirst;

        FindObjectOfType<TCPServer>().MessageToSend(message);
    }

    public void ComposeMessageUpdateBoardClient(int position, int pieceColor, int win)
    {
        string message = "101:" + position + ":" + pieceColor + ":" + win;

        FindObjectOfType<TCPClient>().MessageToSend(message);
    }

    public void ComposeMessageUpdateBoard(int position, int pieceColor, int win)
    {
        string message = "101:" + position + ":" + pieceColor + ":" + win;

        FindObjectOfType<TCPServer>().MessageToSend(message);
    }

    /* 
     * Codes sent only by the server:
     * 
     * 100:0/1:0/1 (start game:blue or red pieces:first or last turn)
     * 
     * Codes sent only by the client:
     * 
     * 
     * 
     * Server/Client codes:
     * 
     * 101:0-8:0/1:0-2 (update board:position to update:blue or red pieces:win condition - 0 nobody won yet, 1 somebody won, 2 draw)
     * 
     * 111,
     * 
    */
}
