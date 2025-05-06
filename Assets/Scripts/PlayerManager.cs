using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private GameObject numericCardFab;
    [SerializeField] private GameObject boostCardFab;
    //[SerializeField] private GameObject PlayerArea;
    //[SerializeField] private GameObject EnemyArea;
    [SerializeField] private GameObject PlayerNumericHand;
    [SerializeField] private GameObject PlayerBoostHand;
    [SerializeField] private GameObject EnemyNumericHand;
    [SerializeField] private GameObject EnemyBoostHand;

    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerNumericHand = GameObject.Find("PlayerNumericHand");
        PlayerBoostHand = GameObject.Find("PlayerBoostHand");
        EnemyNumericHand = GameObject.Find("EnemyNumericHand");
        EnemyBoostHand = GameObject.Find("EnemyBoostHand");
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Command]
    public void CmdDealNumericCards()
    {
        for(int i = 0; i < 4; i++){
            GameObject card = CardFactory.GetRandomNumericCard(numericCardFab);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowNumericCard(card, "Dealt");
        }
    }

    [Command]
    public void CmdDealBoostCards(){
        for(int i = 0; i < 8; i++){
            GameObject card = CardFactory.GetRandomBoostCard(boostCardFab);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowBoostCard(card, "Dealt");
        }
    }

    [ClientRpc]
    private void RpcShowNumericCard(GameObject card ,string type){
        if(type == "Dealt"){
            if(isOwned){
                PlayerNumericHand.GetComponent<NumericHandController>().DrawCard(card);
            }
            else{
                EnemyNumericHand.GetComponent<NumericHandController>().DrawCard(card);
            }
        }
        else if(type == "Selected"){

        }
    }

    [ClientRpc]
    private void RpcShowBoostCard(GameObject card, string type){
        if(type == "Dealt"){
            if(isOwned){
                PlayerBoostHand.GetComponent<BoostHandController>().DrawCard(card);
            }
            else{
                EnemyBoostHand.GetComponent<BoostHandController>().DrawCard(card);
            }
        }
        else if(type == "Selected"){

        }
    }
}
