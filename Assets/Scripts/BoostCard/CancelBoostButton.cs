using UnityEngine;

public class CancelBoostButton : MonoBehaviour
{
    private BoostCardController cardController;

    void Start()
    {
        cardController = gameObject.GetComponentInParent<BoostCardController>();
    }

    private void OnMouseDown()
    {
        cardController.MovePickedCardToHand();
    } 
}
