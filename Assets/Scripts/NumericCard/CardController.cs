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
            //gameObject.name = "Card " + cardId;
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
    private int cardValue = 0;
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
        //hard code
        applyButton = transform.GetChild(1).gameObject;
        applyButton.SetActive(false);
        cancelButton = transform.GetChild(2).gameObject;
        cancelButton.SetActive(false);
        numericCard = transform.GetChild(0).gameObject;
        var tmp = transform.GetChild(3).gameObject;
        for(int i = 0; i < 4; i++){
            markSlots[i] = tmp.transform.GetChild(i).gameObject;
        }
        SwitchMarks(false);
    }   

    public void TurnPicked(){
        HandController.GetComponent<NumericHandController>().PickCard(CardId);
    } 

    public void SwitchButtons(bool value){
        applyButton.SetActive(value);
        cancelButton.SetActive(value);
    }

    public void SwitchMarks(bool value){
        for(int i = 0;i < 4; i++){
            markSlots[i].SetActive(value);
        }
    }

    public void TurnSelected(){
        HandController.GetComponent<NumericHandController>().MoveCardToSelected();
    }

    public void MovePickedCardToHand(){
        HandController.GetComponent<NumericHandController>().MovePickedCardToHand();
    }

    public void SetName(){
        gameObject.name = "Card " + CardValue;
    }

    public void AddMark(Sprite mark){
        if(CurrentMarkIndex > 3) return;
        Debug.Log(mark); 
        markSlots[CurrentMarkIndex].GetComponent<SpriteRenderer>().sprite = mark;
        CurrentMarkIndex++;
    }
}
