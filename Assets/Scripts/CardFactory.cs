using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    static public int numericCardsCount = 16;
    static private string ResourcesPatternNumeric = "Sprites/NumericCards/Card_";
    static private Dictionary<int,Sprite> NumericDictionary = new();
    static private Dictionary<string, Sprite> BoostDictionary = new();
    static private string[] operations = {"+","*","-"};

    void Awake(){
        for(int i = 0; i < numericCardsCount; i++){
            NumericDictionary[i] = Resources.Load<Sprite>(ResourcesPatternNumeric + i); 
        } 

        for(int i = 1; i <= 4;i++){
            BoostDictionary["-" + i] = Resources.Load<Sprite>("Sprites/BoostCards/Boost_min_" + i);
        } 
        
        for(int i = 1; i <= 4; i++){
            BoostDictionary["+" + i] = Resources.Load<Sprite>("Sprites/BoostCards/Boost_p_" + i);
        }

        for(int i = 0; i <= 4; i++){
            if(i == 1) continue;

            BoostDictionary["*" + i] = Resources.Load<Sprite>("Sprites/BoostCards/Boost_mul_" + i);
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
        // conflict with naming in CardValue
        newCard.name = name;
    
        return newCard;
    }

    //add some argument to keep cards unique
    static public GameObject GetRandomNumericCard(GameObject cardFab){
        GameObject ranCard = InstantiateNumericCard(cardFab);
        var ranValue = UnityEngine.Random.Range(0,16);
        ranCard.GetComponentInChildren<SpriteRenderer>().sprite = NumericDictionary[ranValue];

        ranCard.GetComponent<CardController>().CardValue = ranValue;
        return ranCard;
    }

    static public GameObject GetRandomBoostCard(GameObject cardFab){
        GameObject ranCard = Instantiate(cardFab);

        string ranOperation = operations[Random.Range(0,3)];

        string ranValue;
        if(ranOperation == "+" || ranOperation == "-"){
            ranValue = Random.Range(1, 5).ToString();
        } 
        else{
            do{
                ranValue = Random.Range(0,5).ToString();
            } while(ranValue == "1");
        }

        ranCard.GetComponent<BoostCardController>().Action = ranOperation + ranValue;
        ranCard.GetComponentInChildren<SpriteRenderer>().sprite = BoostDictionary[ranOperation + ranValue];

        return ranCard;
    }
}
