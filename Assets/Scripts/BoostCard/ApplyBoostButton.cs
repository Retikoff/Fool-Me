using UnityEngine;

public class ApplyBoostButton : MonoBehaviour
{
    private BoostCardController cardController;

    void Start()
    {
        cardController = GetComponentInParent<BoostCardController>();
    }

    void OnMouseDown()
    {
        cardController.ApplyBoost();
    }
}
