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
    private bool isPicked = false;

    private void Awake()
    {
        applyButton = gameObject.GetComponentInChildren<ApplyButton>().transform.gameObject;
        applyButton.SetActive(false);
        cancelButton = gameObject.GetComponentInChildren<CancelButton>().transform.gameObject;
        cancelButton.SetActive(false);
        numericCard = gameObject.GetComponentInChildren<NumericCard>().transform.gameObject;
    }

    public void TurnPicked(){
        isPicked = true;
        HandController.GetComponent<NumericHandController>().PickCard(CardId);

        applyButton.SetActive(isPicked);
        cancelButton.SetActive(isPicked);
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
