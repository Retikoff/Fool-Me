using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;


public class NumericHandController : NetworkBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform pickedCardPoint;

    private List<GameObject> handCards = new();
    private CardController[] cardControllers;
    private GameObject pickedCard = null;
    private SplineContainer splineContainer;

    private void Start()
    {
        splineContainer = GetComponent<SplineContainer>();
    }

    public void DrawCard(GameObject card){
        card.transform.SetParent(gameObject.transform, false);
    }

    // public void DrawCard(GameObject card){
    //     if(handCards.Count >= maxHandSize) return;

    //     card.transform.position = spawnPoint.position;
    //     card.transform.rotation = spawnPoint.rotation;
    //     card.transform.GetComponent<CardController>().HandController = this;
        
    //     handCards.Add(card);
    //     UpdateCards();
    // }

    private void UpdateCards(){
        UpdateCardPositions();
        UpdateCardControllers();
        UpdateCardIndexes();
        UpdateCardLayerOrders();
    }

    private void UpdateCardPositions(){
        if(handCards.Count == 0) return;
        
        DOTween.KillAll();

        float cardSpacing = 1f / maxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;

        for(int i = 0; i < handCards.Count; i++){
            float pos = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(pos);
            Vector3 forward = spline.EvaluateTangent(pos);
            Vector3 up = spline.EvaluateUpVector(pos);
            Quaternion rotation = Quaternion.LookRotation(up, Vector3.Cross(up, forward).normalized);
            handCards[i].transform.DOMove(splinePosition, 0.25f);
            handCards[i].transform.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }

    private void UpdateCardControllers(){
        cardControllers = null;
        cardControllers = new CardController[handCards.Count];

        for(int i = 0;i < handCards.Count; i++){
            cardControllers[i] = handCards[i].GetComponent<CardController>();
        }
    }

    private void UpdateCardIndexes(){
        for(int i = 0;i < handCards.Count;i++){
            cardControllers[i].CardId = i; 
        }
    }

    private void UpdateCardLayerOrders(){
        for(int i = 0; i < handCards.Count; i++){
            handCards[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = i;
        }
    }

    public void MoveCardToSelected(){
        pickedCard.name = "Selected card";
        pickedCard.GetComponent<CardController>().SwitchButtons(false);

        playerController.TurnCardSelected(pickedCard);

        MessageController.ResetMessage();

        pickedCard = null;
    }

    public void PickCard(int id){
        MovePickedCardToHand();

        pickedCard = handCards[id];
        var pickedCardController = pickedCard.GetComponent<CardController>();

        handCards.RemoveAt(id);
        UpdateCards();

        pickedCardController.IsPicked = true;
        pickedCardController.SwitchMarks(true);

        pickedCard.transform.DOMove(pickedCardPoint.position , 0.15f);
        pickedCard.transform.rotation = new Quaternion(0,0,0,0);
        pickedCardController.SwitchButtons(true);
    }

    public void MovePickedCardToHand(){
        if(pickedCard == null)
            return;

        pickedCard.GetComponent<CardController>().SwitchButtons(false); 
        pickedCard.GetComponent<CardController>().IsPicked = false;
        pickedCard.GetComponent<CardController>().SwitchMarks(false);

        handCards.Add(pickedCard);
        pickedCard = null;  
        UpdateCards();
   }

    public void MoveSelectedCardToHand(GameObject selectedCard){
        selectedCard.GetComponent<CardController>().IsPicked = false;
        selectedCard.GetComponent<CardController>().SwitchMarks(false);
        selectedCard.GetComponent<CardController>().SetName();
        handCards.Add(selectedCard);
        UpdateCards();
    }
}
