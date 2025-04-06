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
    private List<GameObject> handCards = new(4);

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            DrawCard();
        }   
    }

    private void DrawCard(){
        if(handCards.Count >= maxHandSize) return;

        GameObject newCard = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);
        handCards.Add(newCard);
        UpdateCardPositions();
    }

    public void RemoveCard(int index, ref GameObject removedCard){
        removedCard = Instantiate(handCards[index]);
        handCards.RemoveAt(index);
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
