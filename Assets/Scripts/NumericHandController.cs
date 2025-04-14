using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;


//Bug: dotween try to move objects that already destroyed so warning appear in console
public class NumericHandController : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameController gameController;
    [SerializeField] private Transform pickedCardPoint;

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
            DOTween.logBehaviour = LogBehaviour.Verbose;
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

    public void MoveCardToSelected(){
        pickedCard.name = "Selected card";
        pickedCard.GetComponent<CardController>().SwitchButtons(false);

        gameController.TurnCardSelected(pickedCard);

        pickedCard = null;
    }

    public void PickCard(int id){
        MovePickedCardToHand();

        pickedCard = Instantiate(handCards[id]);
        pickedCard.name = "Picked Card";
        var pickedCardController = pickedCard.GetComponent<CardController>();

        pickedCard.transform.DOMove(pickedCardPoint.position , 0.25f);
        pickedCard.transform.rotation = new Quaternion(0,0,0,0);

        pickedCardController.SwitchButtons(true);
        pickedCardController.HandController = this;
        pickedCardController.IsPicked = true;

        Destroy(handCards[id]);
        handCards.RemoveAt(id);
        UpdateCards();
    }

    public void MovePickedCardToHand(){
        if(pickedCard == null)
            return;

        GameObject newCard = Instantiate(pickedCard);
        handCards.Add(newCard);
        newCard.GetComponent<CardController>().SwitchButtons(false); 
        newCard.GetComponent<CardController>().HandController = this;
        newCard.GetComponent<CardController>().IsPicked = false;
        
        UpdateCards();
        Destroy(pickedCard);
        pickedCard = null;
    }

    public void MoveSelectedCardToHand(GameObject selectedCard){
        selectedCard.GetComponent<CardController>().IsPicked = false;
        handCards.Add(selectedCard);
        UpdateCards();
    }
}
