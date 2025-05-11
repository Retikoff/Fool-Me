using UnityEngine;

public class CardFlipper : MonoBehaviour
{
    [SerializeField] public Sprite cardFront;
    [SerializeField] public Sprite cardBack;

    public void Flip(){
        Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;

        if(currentSprite == cardFront){
            GetComponent<SpriteRenderer>().sprite = cardBack;
        }
        else{
            GetComponent<SpriteRenderer>().sprite = cardFront;
        }
    }
}
