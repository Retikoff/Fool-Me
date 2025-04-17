using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    static public int cardsCount = 16;
    static private string ResourcesPatternNumeric = "Sprites/NumericCards/Card_";
    static private Dictionary<int,Sprite> NumericDictionary = new();
    static public void Init(){
        for(int i = 0; i < cardsCount; i++){
            NumericDictionary[i] = Resources.Load<Sprite>(ResourcesPatternNumeric + i); 
        }  
    }

    static public void ChangeNumericCard(GameObject numericCard, int value){
        if(value > 15){
            value = 15;
        }        
        if(value < 0){
            value = 0;
        }

        GameObject card = numericCard.transform.GetChild(0).gameObject;
        SpriteRenderer cardRenderer = card.GetComponent<SpriteRenderer>();
        cardRenderer.sprite = NumericDictionary[value];

        numericCard.GetComponent<CardController>().CardValue = value;
    }

    static public GameObject InstantiateNumericCard(GameObject numericCard, string name = "Card"){
        GameObject newCard = Instantiate(numericCard);

        newCard.GetComponent<CardController>().CardValue = numericCard.GetComponent<CardController>().CardValue;

        newCard.name = name;

        return newCard;
    }
}
