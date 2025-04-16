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
       GameObject card = numericCard.transform.GetChild(0).gameObject;
       SpriteRenderer cardRenderer = card.GetComponent<SpriteRenderer>();
       cardRenderer.sprite = NumericDictionary[value];
    }
}
