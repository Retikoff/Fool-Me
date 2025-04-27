using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;


public class NumericHandController : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameController gameController;
    [SerializeField] private Transform pickedCardPoint;

    private List<GameObject> handCards = new();
    private CardController[] cardControllers;
    private GameObject pickedCard = null;

    public void DrawCard(GameObject card){
        if(handCards.Count >= maxHandSize) return;

        //GameObject newCard = CardFactory.InstantiateNumericCard(card);
        GameObject newCard = CardFactory.GetRandomNumericCard(card);
        newCard.transform.position = spawnPoint.position;
        newCard.transform.rotation = spawnPoint.rotation;
        newCard.transform.GetComponent<CardController>().HandController = this;
        
        handCards.Add(newCard);
        UpdateCards();
    }

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

        gameController.TurnCardSelected(pickedCard);

        pickedCard = null;
    }

    public void PickCard(int id){
        MovePickedCardToHand();

        pickedCard = handCards[id];
        var pickedCardController = pickedCard.GetComponent<CardController>();

        handCards.RemoveAt(id);
        UpdateCards();

        pickedCardController.IsPicked = true;

        pickedCard.transform.DOMove(pickedCardPoint.position , 0.15f);
        pickedCard.transform.rotation = new Quaternion(0,0,0,0);
        pickedCardController.SwitchButtons(true);
    }

    public void MovePickedCardToHand(){
        if(pickedCard == null)
            return;

        pickedCard.GetComponent<CardController>().SwitchButtons(false); 
        pickedCard.GetComponent<CardController>().IsPicked = false;

        handCards.Add(pickedCard);
        pickedCard = null;  
        UpdateCards();
   }

    public void MoveSelectedCardToHand(GameObject selectedCard){
        selectedCard.GetComponent<CardController>().IsPicked = false;
        selectedCard.GetComponent<CardController>().SetName();
        handCards.Add(selectedCard);
        UpdateCards();
    }

    private void DEBUGPrintHandCardsList(){
        Debug.Log("List size: " + handCards.Count);
        Debug.Log("All elements and their i");
        for(int i = 0; i < handCards.Count; i++){
            Debug.Log("Index " + i + " element: " + handCards[i]);
        }
    }
}
