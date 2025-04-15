using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform selectedPoint;
    [SerializeField] private NumericHandController numericHandController;
    [SerializeField] private BoostHandController boostHandController;
    [SerializeField] private GameObject numericCardFab;
    [SerializeField] private GameObject boostCardFab;
    private GameObject selectedCard = null;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            numericHandController.DrawCard(numericCardFab);
        }

        if(Input.GetKeyDown(KeyCode.W)){
            boostHandController.DrawCard(boostCardFab);
        }
    }

    //debug for init implementation
    void OnGUI()
    {
        if(GUI.Button(new Rect(760,100,100,30), "+1")){

        }

        if(GUI.Button(new Rect(760, 140, 100, 30), "*2")){

        }

        if(GUI.Button(new Rect(760, 180, 100, 30), "-1")){

        }
    }

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
