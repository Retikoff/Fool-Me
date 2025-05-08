using Mirror;
using UnityEngine;

public class BoostCard : NetworkBehaviour
{
    private BoostCardController cardController;
    public bool IsPicked {get; set;} = false;

    void Start()
    {
        cardController = gameObject.GetComponentInParent<BoostCardController>();
    }

    private void OnMouseDown()
    {
        if(!IsPicked && isOwned){
            cardController.TurnPicked();
        }
    }
}
