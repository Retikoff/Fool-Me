using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;

public class BoostHandController : NetworkBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private Transform pickedCardPoint;

    private GameObject pickedCard = null;
    private List<GameObject> handCards = new();
    private BoostCardController[] cardControllers;

    public void DrawCard(GameObject card){
        card.transform.SetParent(gameObject.transform, false);
        card.GetComponent<BoostCardController>().HandController = this;
        handCards.Add(card);
        UpdateCards();
    }

    #region CardUpdates
    private void UpdateCards(){
        UpdateCardControllers();
        UpdateCardIndexes();
        UpdateCardLayerOrders();
    }

    private void UpdateCardControllers(){
        cardControllers = null;
        cardControllers = new BoostCardController[handCards.Count];

        for(int i = 0; i < handCards.Count; i++){
            cardControllers[i] = handCards[i].GetComponent<BoostCardController>();
        }
    }

    private void UpdateCardIndexes(){
        for(int i = 0;i < handCards.Count; i++){
            cardControllers[i].CardId = i;
        }
    }

    private void UpdateCardLayerOrders(){
        for(int i = 0; i < handCards.Count; i++){
            handCards[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
        }
    }
    #endregion
    public void PickCard(int id){
        MovePickedCardToHand();

        pickedCard = handCards[id];
        var pickedCardController = pickedCard.GetComponent<BoostCardController>();

        pickedCard.transform.SetParent(pickedCardPoint, false);
        pickedCard.transform.localPosition = Vector3.zero;

        pickedCardController.IsPicked = true;
        pickedCard.name = "Picked Boost card";

        pickedCard.transform.rotation = new Quaternion(0, 0, 0, 0);
        pickedCardController.SwitchButtons(true);
    }

    public void MovePickedCardToHand(){
        if(pickedCard == null){
            return;
        }

        pickedCard.GetComponent<BoostCardController>().SwitchButtons(false);
        pickedCard.GetComponent<BoostCardController>().IsPicked = false;

        pickedCard.transform.SetParent(transform, false);
        pickedCard = null;
    }

    public void ApplyBoost(){
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();
        playerManager.CmdApplyBoost(pickedCard);
        handCards.Remove(pickedCard);
        UpdateCards();
    }
}
