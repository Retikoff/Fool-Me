using System;
using UnityEditor.Build;
using UnityEngine;

public class NumericCard : MonoBehaviour
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
