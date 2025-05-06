using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class BoostHandController : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform pickedCardPoint;

    private GameObject pickedCard = null;
    private List<GameObject> handCards = new();
    private BoostCardController[] cardControllers;
    private SplineContainer splineContainer;

    private void Start()
    {
        splineContainer = GetComponent<SplineContainer>();   
    }

    public void DrawCard(GameObject card){
        if(handCards.Count >= maxHandSize) return;

        GameObject newCard = CardFactory.GetRandomBoostCard(card);
        newCard.transform.position = spawnPoint.position;
        newCard.transform.rotation = spawnPoint.rotation;
        newCard.transform.GetComponent<BoostCardController>().HandController = this;

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
            handCards[i].GetComponentInChildren<Collider2D>().layerOverridePriority = i;
        }
    }

    public void PickCard(int id){
        MovePickedCardToHand();

        pickedCard = handCards[id];
        var pickedCardController = pickedCard.GetComponent<BoostCardController>();

        handCards.RemoveAt(id);
        UpdateCards();

        pickedCardController.IsPicked = true;
        pickedCard.name = "Picked Boost card";
        pickedCard.transform.DOMove(pickedCardPoint.position, 0.25f);
        pickedCard.transform.rotation = new Quaternion(0, 0, 0, 0);
        pickedCardController.SwitchButtons(true);
    }

    public void MovePickedCardToHand(){
        if(pickedCard == null){
            return;
        }

        pickedCard.GetComponent<BoostCardController>().SwitchButtons(false);
        pickedCard.GetComponent<BoostCardController>().IsPicked = false;

        handCards.Add(pickedCard);
        pickedCard = null;

        UpdateCards();
    }

    public void ApplyBoost(){
        bool isCompleted = playerController.ApplyBoost(pickedCard);

        if(isCompleted)
            pickedCard = null;
    }
}
