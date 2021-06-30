using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPiece : MonoBehaviour
{
    [SerializeField]
    GameObject BluePiece;

    [SerializeField]
    GameObject RedPiece;

    [SerializeField]
    bool HasCollider;

    [SerializeField]
    int PiecePosition;

    public void SetPieceImage(int pieceType)
    {
        if (pieceType == 1)
            BluePiece.SetActive(true);
        else
            RedPiece.SetActive(true);
    }

    public void ResetPieceImage()
    {
        BluePiece.SetActive(false);
        RedPiece.SetActive(false);
    }

    public void DisableCollider()
    {
        if(HasCollider)
            GetComponent<BoxCollider2D>().enabled = false;
    }

    public void EnableCollider()
    {
        if(HasCollider)
            GetComponent<BoxCollider2D>().enabled = true;
    }

    public int GetPiecePosition()
    {
        return PiecePosition;
    }
}
