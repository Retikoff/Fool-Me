using UnityEngine;

public class NumericCard : MonoBehaviour
{
    private CardController cardController;
    public bool IsPicked { get;set; } = false;

    private void Start()
    {
        cardController = gameObject.GetComponentInParent<CardController>();
    }

    private void OnMouseDown()
    {
        if(!IsPicked){
            cardController.TurnPicked();
        }
    }
}
