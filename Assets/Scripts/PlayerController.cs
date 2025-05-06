using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform selectedPoint;
    [SerializeField] private NumericHandController numericHandController;
    [SerializeField] private BoostHandController boostHandController;
    [SerializeField] private GameObject numericCardFab;
    [SerializeField] private GameObject boostCardFab;
    [SerializeField] private TextMeshProUGUI textObject;
    private GameObject selectedCard = null; 
    private int selectedCardValue = 0;

    void Awake()
    {
        MessageController.textContainer = textObject;
    }

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

    public bool ApplyBoost(GameObject obj){
        if(selectedCard == null){
            MessageController.ShowMessage("Select card to apply boost");
            return false;
        }

        if(selectedCard.GetComponent<CardController>().CurrentMarkIndex == 4){
            MessageController.ShowMessage("Selected card already has 4 boosts applied");
            return false;
        }

        string action = obj.GetComponent<BoostCardController>().Action;        
        char actionOperation = action[0];
        char actionValue = action[1];

        obj.GetComponent<BoostCardController>().SwitchButtons(false);
        StartCoroutine(ChangeCardWithAnimation(obj, actionOperation, actionValue));

        selectedCard.GetComponent<CardController>().AddMark(obj.GetComponent<BoostCardController>().boostMark);

        MessageController.ResetMessage();

        return true;
    }

    private IEnumerator ChangeCardWithAnimation(GameObject obj, char actionOperation, char actionValue){
        selectedCard.GetComponent<CardController>().SwitchMarks(false);
        
        ScaleGameObject(obj, new Vector3(1.5f, 1.5f, 1.5f), 0.05f);

        obj.transform.DOLocalMove(selectedPoint.position, 0.25f);

        yield return new WaitForSecondsRealtime(1f);
        
        obj.transform.DORotate(new Vector3(0, 90, 0), 0.25f);
        
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

        yield return new WaitForSecondsRealtime(0.25f);

        selectedCard.GetComponent<CardController>().SwitchMarks(true);
        DOTween.Kill(obj.transform);
        Destroy(obj);
    }

    private void ScaleGameObject(GameObject obj, Vector3 scale, float duration){
        obj.transform.DOScale(scale, duration);
    }
}
