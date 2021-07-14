using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BoardUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject BoardUI;

    [SerializeField]
    RectTransform Player1;

    [SerializeField]
    RectTransform Player2;

    [SerializeField]
    RectTransform Board;

    [SerializeField]
    GameObject YourTurnObject;

    [SerializeField]
    RectTransform YourTurnMessage;

    [SerializeField]
    GameObject EnemyTurnObject;

    [SerializeField]
    RectTransform EnemyTurnMessage;

    [SerializeField]
    GameObject EnemyTurnPanel;

    [SerializeField]
    float AnimationSpeed = 0f;

    [SerializeField]
    SetupPiece[] BoardPieces;

    [SerializeField]
    GameObject MainMenu;

    private void Start()
    {
        foreach(SetupPiece s in BoardPieces)
            s.EnableCollider();
    }

    public void SetupBoard(int player1Piece, int player2Piece)
    {
        StartCoroutine(SetUpPlayBoard(player1Piece, player2Piece));

    }

    IEnumerator SetUpPlayBoard(int piece1, int piece2)
    {
        Player1.DOAnchorPos(new Vector2(-550, 200), AnimationSpeed);

        Player2.DOAnchorPos(new Vector2(500, 200), AnimationSpeed);

        //Board.DOAnchorPos(new Vector2(0, 0), AnimationSpeed);

        yield return new WaitForSeconds(0.5f);

        Player1.gameObject.GetComponent<SetupPiece>().SetPieceImage(piece1);

        Player2.gameObject.GetComponent<SetupPiece>().SetPieceImage(piece2);

    }

    public void UpdateBoard(int piecePosition, int piece)
    {
        BoardPieces[piecePosition].SetPieceImage(piece);

        BoardPieces[piecePosition].DisableCollider();
    }

    public void UpdateTurnMessage(float delay, bool PlayOrNot)
    {
        if (PlayOrNot)
        {
            if (EnemyTurnPanel.activeSelf)
            {
                EnemyTurnMessage.DOAnchorPos(new Vector2(0, 150), 0f);

                EnemyTurnPanel.SetActive(false);
            }

            YourTurnObject.SetActive(true);

            YourTurnMessage.DOAnchorPos(new Vector2(0, 0), AnimationSpeed * 2f);

        }
        else
        {
            if (YourTurnObject.activeSelf)
            {
                YourTurnMessage.DOAnchorPos(new Vector2(0, 150), 0f);
            }

            EnemyTurnPanel.SetActive(true);

            EnemyTurnMessage.DOAnchorPos(new Vector2(0, 0), AnimationSpeed * 2f);
        }
    }

    IEnumerator ShowTurnMessage(float delay, bool Play)
    {
        yield return new WaitForSeconds(delay);

        if(Play)
        {
            if (EnemyTurnObject.activeSelf)
            {
                EnemyTurnMessage.DOAnchorPos(new Vector2(0, 150), 0f);

                EnemyTurnObject.SetActive(false);
            }

            YourTurnObject.SetActive(true);

            YourTurnMessage.DOAnchorPos(new Vector2(0,0), AnimationSpeed * 2f);
    
        }
        else
        {
            if(YourTurnObject.activeSelf)
            {

                YourTurnMessage.DOAnchorPos(new Vector2(0, 150), 0f);

                YourTurnObject.SetActive(false);
             
            }

            EnemyTurnObject.SetActive(true);

            EnemyTurnMessage.DOAnchorPos(new Vector2(0, 0), AnimationSpeed * 2f);
        }

    }

    public void OpenMenuScreen()
    {
        if (!MainMenu.activeSelf)
            MainMenu.SetActive(true);
    }

    public void CloseMenuScreen()
    {
        if (MainMenu.activeSelf)
            MainMenu.SetActive(false);
    }

}
