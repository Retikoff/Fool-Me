using DG.Tweening;
using UnityEngine;
using UnityEngine.XR;

public class CardController : MonoBehaviour
{
    public int CardId {get; set;}
    private GameObject applyButton;
    private GameObject cancelButton;
    private GameObject numericCard;
    public NumericHandController HandController {get;set;}
    //private bool isPicked = false;

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
        TurnPicked();
        numericCard.GetComponent<NumericCard>().IsPickable = false;
        //delete from numericHand List and assign to selectedCard position
        transform.position = new Vector3(0,-0.35f,0);
    }

    public void GoDeck(){

    }
}
