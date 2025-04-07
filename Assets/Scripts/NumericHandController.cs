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
    private List<CardController> cardControllers = new();
    private GameObject pickedCard;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            DrawCard();
        }   
    }

    private void DrawCard(){
        if(handCards.Count >= maxHandSize) return;

        GameObject newCard = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);
        newCard.transform.GetComponent<CardController>().HandController = this;

        handCards.Add(newCard);

        UpdateCards();
    }

    private void UpdateCards(){
        UpdateCardPositions();
        UpdateCardControllers();
        UpdateCardIndexes();
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
        for(int i = 0;i<cardControllers.Count; i++){
            cardControllers[i] = handCards[i].GetComponent<CardController>();
        }
    }

    private void UpdateCardIndexes(){
        for(int i = 0;i<cardControllers.Count;i++){
            cardControllers[i].CardId = i; 
        }
    }

    private void MoveCardToSelected(int id){

    }

    public void PickCard(int id){
        if(pickedCard != null){
            handCards.Add(pickedCard);
            UpdateCards();
        }

        pickedCard = Instantiate(handCards[id]);
        pickedCard.transform.DOMoveY(pickedCard.transform.position.y + 1f, 0.25f);

        handCards.RemoveAt(id);
        UpdateCards();
    }


}
