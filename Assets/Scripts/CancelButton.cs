using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CancelButton : MonoBehaviour
{
    private CardController cardController;

    private void Start()
    {
        cardController = gameObject.GetComponentInParent<CardController>();
    }

    private void OnMouseDown()
    {
        cardController.TurnPicked();
    }
}
