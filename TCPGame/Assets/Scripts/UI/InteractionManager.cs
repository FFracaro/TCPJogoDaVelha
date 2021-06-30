using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    bool CanInteract = false;

    InformationHolder Info;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CanInteract)
        {
            if (Input.GetMouseButtonDown (0))
            {
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection (ray, Mathf.Infinity);

                if (hit.collider != null && hit.collider.tag == "GamePiece")
                {
                    SetupPiece piece = hit.collider.gameObject.GetComponent<SetupPiece>();

                    piece.DisableCollider();

                    if(Info == null)
                        Info = FindObjectOfType<InformationHolder>();

                    piece.SetPieceImage(Info.GetPieceColor());

                    FindObjectOfType<GameBehaviour>().FinishTurn(piece.GetPiecePosition(), Info.GetPieceColor());

                    CanInteract = false;
                }
            }         
        }
    }

    public void SetInteractionActive()
    {
        CanInteract = true;
    }
}
