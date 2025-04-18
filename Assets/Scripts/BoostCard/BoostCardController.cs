using UnityEngine;

public class BoostCardController : MonoBehaviour
{
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
    private GameObject applyButton;
    private GameObject cancelButton;
    private GameObject boostCard;
    public BoostHandController HandController {get;set;}
    public string Action {get; set;}
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
    private void Awake()
    {
        boostCard = transform.GetChild(0).gameObject;
        applyButton = transform.GetChild(1).gameObject;
        applyButton.SetActive(false);
        cancelButton = transform.GetChild(2).gameObject;
        cancelButton.SetActive(false);
    }
    
    
}
