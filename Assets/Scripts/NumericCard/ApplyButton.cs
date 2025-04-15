using UnityEngine;

public class ApplyButton : MonoBehaviour
{
    private CardController cardController;

    private void Start()
    {
        cardController = gameObject.GetComponentInParent<CardController>();
    }

    private void OnMouseDown()
    {
        cardController.TurnSelected();
    }
}
