using Mirror;
using UnityEngine;

public class NumericCard : NetworkBehaviour
{
    private CardController cardController;
    public bool IsPicked { get;set; } = false;

    private void Start()
    {
        cardController = gameObject.GetComponentInParent<CardController>();
    }

    private void OnMouseDown()
    {
        if(!IsPicked && isOwned){
            cardController.TurnPicked();
        }
    }
}
