using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;

public class NumericHandController : NetworkBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private Transform pickedCardPoint;

    //change to private after debug
    public List<GameObject> handCards = new();
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
    #endregion
    public void SelectPickedCard(){
        pickedCard.name = "Selected card";
        pickedCard.GetComponent<CardController>().SwitchButtons(false);

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();
        playerManager.CmdSelectCard(pickedCard);

        //MessageController.ResetMessage();

        pickedCard = null;
    }

    public void PickCard(int id){
        MovePickedCardToHand();

        pickedCard = handCards[id];
        var pickedCardController = pickedCard.GetComponent<CardController>();

        pickedCard.transform.SetParent(pickedCardPoint, false);
        pickedCard.transform.position = pickedCardPoint.position;
        handCards.RemoveAt(id);
        UpdateCards();

        pickedCardController.IsPicked = true;

        pickedCard.transform.rotation = new Quaternion(0,0,0,0);
        pickedCardController.SwitchButtons(true);
    }

    public void MovePickedCardToHand(){
        if(pickedCard == null)
            return;

        pickedCard.GetComponent<CardController>().SwitchButtons(false); 
        pickedCard.GetComponent<CardController>().IsPicked = false;

        pickedCard.transform.SetParent(transform, false);
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
