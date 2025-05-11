using DG.Tweening;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR;

public class CardController : NetworkBehaviour
{
    // change to private after debug
    public int cardId = 0;
    public int CardId {
        get
        {
            return cardId;
        } 
        set{
            cardId = value;
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
    public int cardValue = 0;
    public int CardValue {
        get{
            return cardValue; 
        }
        set{
            cardValue = value;
            gameObject.name = "Card " + value.ToString();
        }
    }  
    private GameObject[] markSlots = new GameObject[4];
    public int CurrentMarkIndex = 0;

    private void Awake()
    {
        applyButton = transform.GetChild(1).gameObject;
        applyButton.SetActive(false);
        cancelButton = transform.GetChild(2).gameObject;
        cancelButton.SetActive(false);
        numericCard = transform.GetChild(0).gameObject;
        var tmp = transform.GetChild(3).gameObject;
        for(int i = 0; i < 4; i++){
            markSlots[i] = tmp.transform.GetChild(i).gameObject;
        }
    }

    public void TurnPicked(){
        HandController.GetComponent<NumericHandController>().PickCard(CardId);
    } 

    public void SwitchButtons(bool value){
        applyButton.SetActive(value);
        cancelButton.SetActive(value);
    }


    public void TurnSelected(){
        HandController.GetComponent<NumericHandController>().SelectPickedCard();
    }

    public void MovePickedCardToHand(){
        HandController.GetComponent<NumericHandController>().MovePickedCardToHand();
    }

    public void SetName(){
        gameObject.name = "Card " + CardValue;
    }

    public void AddMark(GameObject mark){
        if(CurrentMarkIndex > 3) return;
        mark.transform.SetParent(markSlots[CurrentMarkIndex].transform, false);
        CurrentMarkIndex++;
    }
}
