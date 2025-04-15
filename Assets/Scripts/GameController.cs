using DG.Tweening;
using NUnit.Framework;
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
        if(GUI.Button(new Rect(260, 10, 40, 40), "0")){

        }
        if(GUI.Button(new Rect(310, 10, 40, 40), "1")){

        }
        if(GUI.Button(new Rect(360, 10, 40, 40), "2")){
            
        }
        if(GUI.Button(new Rect(410, 10, 40, 40), "3")){
   
        }
        if(GUI.Button(new Rect(460, 10, 40, 40), "3")){   

        }
        if(GUI.Button(new Rect(510, 10, 40, 40), "4")){
            
        }
        if(GUI.Button(new Rect(560, 10, 40, 40), "5")){
            
        }
        if(GUI.Button(new Rect(610, 10, 40, 40), "6")){
            
        }
        if(GUI.Button(new Rect(660, 10, 40, 40), "7")){
            
        }
        if(GUI.Button(new Rect(710, 10, 40, 40), "8")){
            
        }
        if(GUI.Button(new Rect(760, 10, 40, 40), "9")){
            
        }
        if(GUI.Button(new Rect(260, 60, 40, 40), "10")){
            
        }
        if(GUI.Button(new Rect(310, 60, 40, 40), "11")){
            
        }
        if(GUI.Button(new Rect(360, 60, 40, 40), "12")){
            
        }
        if(GUI.Button(new Rect(410, 60, 40, 40), "13")){
            
        }
        if(GUI.Button(new Rect(460, 60, 40, 40), "14")){
            
        }
        if(GUI.Button(new Rect(510, 60, 40, 40), "15")){
            
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
