using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    bool CanInteract = false;

    InformationHolder Info = null;

    /*
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown (0) && CanInteract)
        {
            //Debug.Log("Clicked: x: " + Input.mousePosition.x + " y: " + Input.mousePosition.y);
            Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)), Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && hit.collider.gameObject.tag == "piece")
            {
                SetupPiece piece = hit.collider.gameObject.GetComponent<SetupPiece>();
                Debug.Log("Piece clicked: " + piece.GetPiecePosition());
                Debug.Log("Piece is clickable: " + piece.IsClickable());

                if(piece.IsClickable())
                {
                    CanInteract = false;

                    piece.DisableCollider();

                    if(Info == null)
                        Info = FindObjectOfType<InformationHolder>();

                    piece.SetPieceImage(Info.GetPieceColor());

                    GetComponent<GameBehaviour>().FinishTurn(piece.GetPiecePosition(), Info.GetPieceColor());  
                }
            }
        }         
    }*/

    public void WasClicked(GameObject pieceClicked)
    {
        if(CanInteract)
        {
            SetupPiece piece = pieceClicked.GetComponent<SetupPiece>();
            Debug.Log("Piece clicked: " + piece.GetPiecePosition());
            Debug.Log("Piece is clickable: " + piece.IsClickable());

            if (piece.IsClickable())
            {
                CanInteract = false;

                piece.DisableCollider();

                if (Info == null)
                    Info = FindObjectOfType<InformationHolder>();

                piece.SetPieceImage(Info.GetPieceColor());

                GetComponent<GameBehaviour>().FinishTurn(piece.GetPiecePosition(), Info.GetPieceColor());
            }
        }
    }

    public void SetInteractionActive()
    {
        CanInteract = true;

        Debug.Log("Can Interact.");
    }
}
