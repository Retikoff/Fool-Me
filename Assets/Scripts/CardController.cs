using System.Threading;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CardController : MonoBehaviour
{
    private GameObject applyButton;
    private GameObject cancelButton;
    private GameObject numericCard;
    private Transform SelectedCardPosition; // should be initialized by GameController
    private Vector3 defaultPosition; // will be replaced with grid implementation
    private bool isPicked = false;

    private void Awake()
    {
        applyButton = gameObject.GetComponentInChildren<ApplyButton>().transform.gameObject;
        applyButton.SetActive(false);
        cancelButton = gameObject.GetComponentInChildren<CancelButton>().transform.gameObject;
        cancelButton.SetActive(false);
        numericCard = gameObject.GetComponentInChildren<NumericCard>().transform.gameObject;
        defaultPosition = transform.position;
    }

    public void TurnPicked(){
        if(isPicked){
            isPicked = false;
            numericCard.GetComponent<NumericCard>().IsPickable = true;

            transform.DOMoveY(transform.position.y - 1.5f, 0.4f);
            //move down
        }
        else{
            isPicked = true;
            numericCard.GetComponent<NumericCard>().IsPickable = false;

            transform.DOMoveY(transform.position.y + 1.5f, 0.4f);
            //move up
        }

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
