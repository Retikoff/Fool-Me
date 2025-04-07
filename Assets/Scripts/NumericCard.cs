using System;
using UnityEditor.Build;
using UnityEngine;

public class NumericCard : MonoBehaviour
{
    private CardController cardController;
    private bool isPickable = true;
    public bool IsPickable {
        get{
        return isPickable;
        } 
        set{
            isPickable = value;
        }
    }

    private void Start()
    {
        cardController = gameObject.GetComponentInParent<CardController>();
    }

    private void OnMouseDown()
    {
        if(isPickable){
            cardController.TurnPicked();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        cardController.GoDeck();
    }
}
