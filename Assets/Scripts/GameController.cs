using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform selectedPoint;
    [SerializeField] private NumericHandController numericHandController;
    private GameObject selectedCard = null;
    
    public void TurnCardSelected(GameObject card){
        if(selectedCard == null) {
            selectedCard = card;
        }
        else{
            ScaleGameObject(selectedCard, new Vector3(1,1,1));
            numericHandController.MoveSelectedCardToHand(selectedCard);
            selectedCard = card;
        }
        
        ScaleGameObject(selectedCard, new Vector3(1.5f,1.5f,1.5f));
        selectedCard.transform.DOLocalMove(selectedPoint.position, 0.25f);
    }

    private void ScaleGameObject(GameObject obj, Vector3 scale){
        obj.transform.DOScale(scale, 0.25f);
    }
}
