using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform selectedPoint;
    private GameObject selectedCard = null;
    
    public void TurnCardSelected(GameObject card){
        if(selectedCard == null) {
            selectedCard = card;
        }
        else{
            //move current selected to hand

        }

        card.transform.DOLocalMove(selectedPoint.position, 0.25f);
    }
}
