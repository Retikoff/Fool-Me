using System;
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
        if(selectedCard == null) return ;
        string action = boostCard.GetComponent<BoostCardController>().Action;
        char actionOperation = action[0];
        char actionValue = action[1];

        int newValue = 0;

        switch(actionOperation){
            case '+':
                newValue = selectedCard.GetComponent<CardController>().CardValue + int.Parse(actionValue.ToString());
                break;
            case '-':
                newValue = selectedCard.GetComponent<CardController>().CardValue - int.Parse(actionValue.ToString());
                break;
            case '*':
                newValue = selectedCard.GetComponent<CardController>().CardValue * int.Parse(actionValue.ToString());
                break;
        }

        if(newValue < 0) newValue = 0;
        if(newValue > 15) newValue = 15;

        selectedCard.GetComponent<CardController>().AddMark(boostCard.GetComponent<BoostCardController>().boostMark);

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();
        playerManager.CmdChangeCard(selectedCard, newValue);

        NetworkServer.Destroy(boostCard);
    }
}
