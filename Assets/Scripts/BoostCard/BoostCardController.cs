using UnityEngine;
using UnityEngine.XR;

public class BoostCardController : MonoBehaviour
{
    private GameObject applyButton;
    private GameObject cancelButton;
    private GameObject boostCard;
    public BoostHandController HandController {get;set;}
    public string action;
    public string Action {
        get{return action;}
        set{action = value;}
    }
    private bool isPicked = false;
    public bool IsPicked{
        get{
            return isPicked;
        }
        set{
            isPicked = value;
            boostCard.GetComponent<BoostCard>().IsPicked = value;
        }
    }
    public int cardId = 0;
    public int CardId{
        get{
            return cardId;
        }
        set{
            cardId = value;
            gameObject.name = "Boost card " + value;
        }
    }
    public GameObject boostMark = null;

    private void Awake()
    {
        boostCard = transform.GetChild(0).gameObject;
        applyButton = transform.GetChild(1).gameObject;
        applyButton.SetActive(false);
        cancelButton = transform.GetChild(2).gameObject;
        cancelButton.SetActive(false);
    }
    
    public void SwitchButtons(bool value){
        applyButton.SetActive(value);
        cancelButton.SetActive(value);
    }

    public void TurnPicked(){
        HandController.GetComponent<BoostHandController>().PickCard(CardId);
    }

    public void MovePickedCardToHand(){
        HandController.GetComponent<BoostHandController>().MovePickedCardToHand();
    }

    public void ApplyBoost(){
        HandController.ApplyBoost();
    }
}
