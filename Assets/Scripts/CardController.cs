using System.Threading;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CardController : MonoBehaviour
{
    private GameObject ApplyButton;
    private GameObject CancelButton;
    private GameObject NumericCard;
    [SerializeField] Transform SelectedCardPosition; // should be initialized by GameController
    private Vector3 defaultPosition; // will be replaced with grid implementation
    private bool isPicked = false;

    private void Awake()
    {
        ApplyButton = gameObject.GetComponentInChildren<ApplyButton>().transform.gameObject;
        ApplyButton.SetActive(false);
        CancelButton = gameObject.GetComponentInChildren<CancelButton>().transform.gameObject;
        CancelButton.SetActive(false);
        NumericCard = gameObject.GetComponentInChildren<NumericCard>().transform.gameObject;
        defaultPosition = transform.position;
    }

    public void TurnPicked(){
        if(isPicked){
            isPicked = false;

            //move down
        }
        else{
            isPicked = true;

            //move up
        }

        ApplyButton.SetActive(isPicked);
        CancelButton.SetActive(isPicked);

    } 

    public void TurnSelected(){
        TurnPicked();
        NumericCard.GetComponent<NumericCard>().IsPickable = false;

        transform.position = SelectedCardPosition.position;
    }

    public void GoDeck(){

    }
}
