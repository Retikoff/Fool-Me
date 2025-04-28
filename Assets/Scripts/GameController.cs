using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform selectedPoint;
    [SerializeField] private NumericHandController numericHandController;
    [SerializeField] private BoostHandController boostHandController;
    [SerializeField] private GameObject numericCardFab;
    [SerializeField] private GameObject boostCardFab;
    private GameObject selectedCard = null; 
    private int selectedCardValue = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            numericHandController.DrawCard(numericCardFab);
        }

        if(Input.GetKeyDown(KeyCode.W)){
            boostHandController.DrawCard(boostCardFab);
        }

        if( selectedCard != null && (selectedCardValue != selectedCard.GetComponent<CardController>().CardValue)){
            selectedCardValue = selectedCard.GetComponent<CardController>().CardValue; 
        }
    }

    //debug for init implementation
    void OnGUI()
    {
        #region DEBUG_SETTERS
        if(GUI.Button(new Rect(760,100,100,30), "+1")){
            CardFactory.ChangeNumericCard(selectedCard, selectedCard.GetComponent<CardController>().CardValue + 1);
        }
        if(GUI.Button(new Rect(760, 140, 100, 30), "*2")){
            CardFactory.ChangeNumericCard(selectedCard, selectedCard.GetComponent<CardController>().CardValue * 2);
        }
        if(GUI.Button(new Rect(760, 180, 100, 30), "-1")){
            CardFactory.ChangeNumericCard(selectedCard, selectedCard.GetComponent<CardController>().CardValue - 1);
        }
        if(GUI.Button(new Rect(260, 10, 40, 40), "0")){
            CardFactory.ChangeNumericCard(selectedCard, 0);
        }
        if(GUI.Button(new Rect(310, 10, 40, 40), "1")){
            CardFactory.ChangeNumericCard(selectedCard, 1);
        }
        if(GUI.Button(new Rect(360, 10, 40, 40), "2")){
            CardFactory.ChangeNumericCard(selectedCard, 2);
        }
        if(GUI.Button(new Rect(410, 10, 40, 40), "3")){
            CardFactory.ChangeNumericCard(selectedCard, 3);
        }
        if(GUI.Button(new Rect(460, 10, 40, 40), "4")){
            CardFactory.ChangeNumericCard(selectedCard, 4);
        }
        if(GUI.Button(new Rect(510, 10, 40, 40), "5")){
            CardFactory.ChangeNumericCard(selectedCard, 5);
        }
        if(GUI.Button(new Rect(560, 10, 40, 40), "6")){
            CardFactory.ChangeNumericCard(selectedCard, 6);
        }
        if(GUI.Button(new Rect(610, 10, 40, 40), "7")){
            CardFactory.ChangeNumericCard(selectedCard, 7);
        }
        if(GUI.Button(new Rect(660, 10, 40, 40), "8")){
            CardFactory.ChangeNumericCard(selectedCard, 8);
        }
        if(GUI.Button(new Rect(710, 10, 40, 40), "9")){
            CardFactory.ChangeNumericCard(selectedCard, 9);
        }
        if(GUI.Button(new Rect(260, 60, 40, 40), "10")){
            CardFactory.ChangeNumericCard(selectedCard, 10);
        }
        if(GUI.Button(new Rect(310, 60, 40, 40), "11")){
            CardFactory.ChangeNumericCard(selectedCard, 11);
        }
        if(GUI.Button(new Rect(360, 60, 40, 40), "12")){
            CardFactory.ChangeNumericCard(selectedCard, 12);
        }
        if(GUI.Button(new Rect(410, 60, 40, 40), "13")){
            CardFactory.ChangeNumericCard(selectedCard, 13);
        }
        if(GUI.Button(new Rect(460, 60, 40, 40), "14")){
            CardFactory.ChangeNumericCard(selectedCard, 14);
        }
        if(GUI.Button(new Rect(510, 60, 40, 40), "15")){
            CardFactory.ChangeNumericCard(selectedCard, 15);
        }
        #endregion 
    }

    public void TurnCardSelected(GameObject card){
        if(selectedCard == null) {
            selectedCard = card;
        }
        else{
            selectedCard.transform.localScale = new Vector3(1,1,1);            
            numericHandController.MoveSelectedCardToHand(selectedCard);
            selectedCard = card;
        }
        
        ScaleGameObject(selectedCard, new Vector3(1.5f,1.5f,1.5f), 0.25f);
        selectedCard.transform.DOLocalMove(selectedPoint.position, 0.25f);
        selectedCardValue = selectedCard.GetComponent<CardController>().CardValue;
    }

    public void ApplyBoost(GameObject obj){
        if(selectedCard == null){
            //show message that numericCard is not selected
            return;
        }

        if(selectedCard.GetComponent<CardController>().CurrentMarkIndex == 4){
            //show message that selected card already have 4 marks
            return;
        }

        string action = obj.GetComponent<BoostCardController>().Action;        
        char actionOperation = action[0];
        char actionValue = action[1];

        obj.transform.DOLocalMove(selectedPoint.position, 0.25f);
        DOTween.Kill(obj.transform);
        Destroy(obj);

        switch(actionOperation){
            case '+':
                CardFactory.ChangeNumericCard(selectedCard, selectedCardValue + int.Parse(actionValue.ToString()));
                break;
            case '-':
                CardFactory.ChangeNumericCard(selectedCard, selectedCardValue - int.Parse(actionValue.ToString()));
                break;
            case '*':
                CardFactory.ChangeNumericCard(selectedCard, selectedCardValue * int.Parse(actionValue.ToString()));
                break;
            default:
                Debug.Log("Error in applyBoost switch (unhandled operation)");
                break;
        }
        selectedCard.GetComponent<CardController>().AddMark(obj.GetComponent<BoostCardController>().boostMark);
    }

    private void ScaleGameObject(GameObject obj, Vector3 scale, float duration){
        obj.transform.DOScale(scale, duration);
    }
}
