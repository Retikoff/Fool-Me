using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    static public TextMeshProUGUI textContainer;

    void Start()
    {
        textContainer.text = "";
    }

    static public void ShowMessage(string message){
        textContainer.text = message;
    }

    static public void ResetMessage(){
        textContainer.text = "";
    }
}
