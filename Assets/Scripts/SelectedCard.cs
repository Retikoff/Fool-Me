using Mirror;
using UnityEngine;

public class SelectedCard : NetworkBehaviour
{
    private GameObject selectedCard = null;

    public void ChangeCard(GameObject card){
        if(selectedCard != null){
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();
            selectedCard.transform.localScale = new Vector3(1f, 1f, 1f);
            playerManager.CmdMoveSelectedCardToHand(selectedCard);
        }
        selectedCard = card;

        selectedCard.transform.SetParent(transform, false);
        selectedCard.transform.localPosition = Vector3.zero;
        selectedCard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void ApplyBoost(GameObject boostCard){

    }
}
