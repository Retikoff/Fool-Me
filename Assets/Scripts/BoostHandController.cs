using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class BoostHandController : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameController gameController;
    [SerializeField] private Transform pickedCardPoint;

    private List<GameObject> handCards = new();
    
    public void DrawCard(GameObject card){
        if(handCards.Count >= maxHandSize) return;

        GameObject newCard = Instantiate(card, spawnPoint.position, spawnPoint.rotation);
        newCard.transform.GetComponent<BoostCardController>().HandController = this;

        handCards.Add(newCard);
        UpdateCards();
    }

    private void UpdateCards(){
        UpdateCardPositions();
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
}
