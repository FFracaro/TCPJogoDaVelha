using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    private InformationHolder Info;

    [SerializeField]
    private MessageInterpreter Interpreter;

    [SerializeField]
    BoardUIManager BoardUIController;

    private int[] BoardPieces = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public void GetInformationHolder()
    {
        Info = FindObjectOfType<InformationHolder>();
    }

    public void PickRandomPiece()
    {
        int ServerColorChoice = Random.Range(0, 1);

        if (ServerColorChoice == 0)
            Info.SetPieceColor(-1);
        else
            Info.SetPieceColor(1);

    }

    public int WhoGoesFirst()
    {
        int ServerChoice = Random.Range(0, 1);

        Info.SetPlayFirst(ServerChoice);

        return ServerChoice;
    }

    public void StartGame()
    {
        GetInformationHolder();

        PickRandomPiece();

        Interpreter.ComposeMessageStartGame(Info.GetPieceColor(), WhoGoesFirst());

        ShowBoard();

    }

    public void StartGameClient(int pieceColor, int WhoGoesFirst)
    {
        GetInformationHolder();

        Info.SetPieceColor(pieceColor);

        Info.SetPlayFirst(WhoGoesFirst);

        ShowBoard();
    }

    public void ShowBoard()
    {
        StartCoroutine(BoardSetUp());
    }

    IEnumerator BoardSetUp()
    {
        BoardUIController.SetupBoard(Info.GetPieceColor(), (-1*Info.GetPieceColor()));

        if (Info.GetPlayFirst() == 0)
        {
            StartTurn();
        }
        else
        {
            FindObjectOfType<BoardUIManager>().UpdateTurnMessage(1f, false);
        }

        yield return null;
    }

    public void StartTurn()
    {
        FindObjectOfType<BoardUIManager>().UpdateTurnMessage(1f, true);

        FindObjectOfType<InteractionManager>().SetInteractionActive();

        // If adversary turn panel is active, disable it / enable Your turn message
        // Allow to click board if it's this player turn (interaction class set interaction to true)
    }

    public void FinishTurn(int positionClicked, int colorPiece)
    {

        FindObjectOfType<BoardUIManager>().UpdateTurnMessage(0f, false);

        UpdateBoardScore(positionClicked, colorPiece);

        int WinCheckerResult = VerifyWinningCondition();

        switch(WinCheckerResult)
        {
            case 1:
                if(Info.GetPieceColor() == 1)
                {
                    Interpreter.ComposeMessageUpdateBoard(positionClicked, 1, 1);
                    EndGame(1);
                }
                break;
            case 2:
                if(Info.GetPieceColor() == -1)
                {
                    Interpreter.ComposeMessageUpdateBoard(positionClicked, -1, 1);
                    EndGame(-1);
                }
                break;
            case 3:
                Interpreter.ComposeMessageUpdateBoard(positionClicked, Info.GetPieceColor(), 0);
                break;
            case -1:
                Interpreter.ComposeMessageUpdateBoard(positionClicked, Info.GetPieceColor(), 2);
                EndGame(0);
                break;
        }
    }

    // -1 == red color
    // 1 == blue color
    public void UpdateBoardScore(int position, int color)
    {
        if(position >= 0 && position < 9)
        {
            BoardPieces[position] = color;
        }

        // Se position == -1, turn was skipped and there's nothing to update

    }

    public void EndGame(int whoWon)
    {
        // Show winning interface
    }

    // 1 = blue victory
    // 2 = red victory
    // 3 = game still going
    // -1 = draw
    public int VerifyWinningCondition()
    {
        int[] Sum = { 0, 0, 0, 0, 0, 0, 0, 0 };

        Sum[0] = BoardPieces[0] + BoardPieces[1] + BoardPieces[2];
        Sum[1] = BoardPieces[3] + BoardPieces[4] + BoardPieces[5];
        Sum[2] = BoardPieces[6] + BoardPieces[7] + BoardPieces[8];

        Sum[3] = BoardPieces[0] + BoardPieces[3] + BoardPieces[6];
        Sum[4] = BoardPieces[1] + BoardPieces[4] + BoardPieces[7];
        Sum[5] = BoardPieces[2] + BoardPieces[5] + BoardPieces[8];

        Sum[6] = BoardPieces[0] + BoardPieces[4] + BoardPieces[8];
        Sum[7] = BoardPieces[2] + BoardPieces[4] + BoardPieces[6];

        int BoardIncomplete = 0;

        foreach(int k in Sum)
        {
            if (k == 3)
                return 1;
            if (k == -3)
                return 2;
            if (k == 0)
                BoardIncomplete++;
        }

        if (BoardIncomplete > 0)
            return 3;

        return -1;
    }
}
