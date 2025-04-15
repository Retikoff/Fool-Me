using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class CardController : MonoBehaviour
{
    private int cardId = 0;
    public int CardId {
        get
        {
            return cardId;
        } 
        set{
            cardId = value;
            gameObject.name = "Card " + cardId;
        }
    }
    private GameObject applyButton;
    private GameObject cancelButton;
    private GameObject numericCard;
    public NumericHandController HandController {get;set;}
    private bool isPicked = false;
    public bool IsPicked {
        get{
            return isPicked;
        } 
        set{
            isPicked = value;
            numericCard.GetComponent<NumericCard>().IsPicked = value;
        }
    }

    private void Awake()
    {
        //hard code
        applyButton = transform.GetChild(1).gameObject;
        applyButton.SetActive(false);
        cancelButton = transform.GetChild(2).gameObject;
        cancelButton.SetActive(false);
        numericCard = transform.GetChild(0).gameObject;
    }   

    public void TurnPicked(){
        HandController.GetComponent<NumericHandController>().PickCard(CardId);
    } 

    public void SwitchButtons(bool value){
        applyButton.SetActive(value);
        cancelButton.SetActive(value);
    }

    public void TurnSelected(){
        HandController.GetComponent<NumericHandController>().MoveCardToSelected();
    }

    public void MovePickedCardToHand(){
        HandController.GetComponent<NumericHandController>().MovePickedCardToHand();
    }
}
