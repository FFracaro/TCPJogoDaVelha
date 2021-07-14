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
        HasCollider = false;
    }

    public void EnableCollider()
    {
        HasCollider = true;
    }

    public bool IsClickable()
    {
        return HasCollider;
    }

    public int GetPiecePosition()
    {
        return PiecePosition;
    }
}
