using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class NumericHandController : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    private List<GameObject> handCards = new();
    private CardController[] cardControllers;
    private GameObject pickedCard = null;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            DrawCard(cardPrefab);
        }   
    }

    public void DrawCard(GameObject card){
        if(handCards.Count >= maxHandSize) return;

        GameObject newCard = Instantiate(card, spawnPoint.position, spawnPoint.rotation);
        newCard.transform.GetComponent<CardController>().HandController = this;
        newCard.name = "Card";
        
        handCards.Add(newCard);

        UpdateCards();
    }

    private void UpdateCards(){
        UpdateCardPositions();
        UpdateCardControllers();
        UpdateCardIndexes();
        foreach(var el in cardControllers){
            Debug.Log(el.CardId);
        }
    }

    private void UpdateCardPositions(){
        if(handCards.Count == 0) return;

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
        //might be huge memory leak
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

    private void MoveCardToSelected(int id){

    }

    public void PickCard(int id){
        if(pickedCard != null){
            MovePickedCardToHand();
        }

        pickedCard = Instantiate(handCards[id]);
        pickedCard.name = "Picked Card";

        pickedCard.transform.DOMoveY(pickedCard.transform.position.y + 4f, 0.25f);
        pickedCard.transform.rotation = new Quaternion(0,0,0,0);
        pickedCard.GetComponent<CardController>().SwitchButtons(true);
        pickedCard.GetComponent<CardController>().HandController = this;

        Destroy(handCards[id]);
        handCards.RemoveAt(id);
        UpdateCards();
        Debug.Log("Picked cardId: " + id);
    }

    private void MovePickedCardToHand(){
        if(pickedCard == null)
            return;

        GameObject newCard = Instantiate(pickedCard);
        newCard.name = "Card";
        handCards.Add(newCard);
        newCard.GetComponent<CardController>().HandController = this;

        Destroy(pickedCard);
        UpdateCards();
    }
}
