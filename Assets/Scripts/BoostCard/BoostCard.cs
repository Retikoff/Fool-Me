using UnityEngine;

public class BoostCard : MonoBehaviour
{
    private BoostCardController cardController;
    public bool IsPicked {get; set;} = false;

    void Start()
    {
        cardController = gameObject.GetComponentInParent<BoostCardController>();
    }

    private void OnMouseDown()
    {
        if(!IsPicked){
            cardController.TurnPicked();
        }
    }

    private void OnMouseOver()
    {
        cardController.PrintOveredCard();
    }
}
