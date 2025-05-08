using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;

public class NumericHandController : NetworkBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform pickedCardPoint;

    private List<GameObject> handCards = new();
    private CardController[] cardControllers;
    private GameObject pickedCard = null;


    public void DrawCard(GameObject card){
        card.transform.SetParent(gameObject.transform, false);
        card.GetComponent<CardController>().HandController = this;
        handCards.Add(card);
        UpdateCards();
    }

    #region CardUpdate
    private void UpdateCards(){
        UpdateCardControllers();
        UpdateCardIndexes();
        UpdateCardLayerOrders();
    }

    private void UpdateCardControllers(){
        cardControllers = null;
        cardControllers = new CardController[handCards.Count];

        for(int i = 0;i < handCards.Count; i++){
            cardControllers[i] = handCards[i].GetComponent<CardController>();
        }
    }

    private void UpdateCardIndexes(){
        for(int i = 0; i < handCards.Count;i++){
            cardControllers[i].CardId = i; 
        }
    }

    private void UpdateCardLayerOrders(){
        for(int i = 0; i < handCards.Count; i++){
            handCards[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
        }
    }
    #endregion
    public void MoveCardToSelected(){
        pickedCard.name = "Selected card";
        pickedCard.GetComponent<CardController>().SwitchButtons(false);

        //playerController.TurnCardSelected(pickedCard);

        MessageController.ResetMessage();

        pickedCard = null;
    }

    public void PickCard(int id){
        MovePickedCardToHand();

        pickedCard = handCards[id];
        var pickedCardController = pickedCard.GetComponent<CardController>();

        pickedCard.transform.SetParent(null);
        handCards.RemoveAt(id);
        UpdateCards();

        pickedCardController.IsPicked = true;

        pickedCard.transform.DOMove(pickedCardPoint.position, 0.25f);
        pickedCard.transform.rotation = new Quaternion(0,0,0,0);
        pickedCardController.SwitchButtons(true);
    }

    public void MovePickedCardToHand(){
        if(pickedCard == null)
            return;

        pickedCard.GetComponent<CardController>().SwitchButtons(false); 
        pickedCard.GetComponent<CardController>().IsPicked = false;

        pickedCard.transform.SetParent(gameObject.transform, false);
        handCards.Add(pickedCard);
        pickedCard = null;  
        UpdateCards();
   }

    public void MoveSelectedCardToHand(GameObject selectedCard){
        selectedCard.GetComponent<CardController>().IsPicked = false;
        selectedCard.GetComponent<CardController>().SetName();
        selectedCard.transform.SetParent(gameObject.transform, false);
        handCards.Add(selectedCard);
        UpdateCards();
    }
}
