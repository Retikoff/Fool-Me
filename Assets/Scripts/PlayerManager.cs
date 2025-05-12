using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Video;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private GameObject numericCardFab;
    [SerializeField]private GameObject boostCardFab;
    [SerializeField] private GameObject PlayerNumericHand;
    [SerializeField] private GameObject PlayerBoostHand;
    [SerializeField] private GameObject EnemyNumericHand;
    [SerializeField] private GameObject EnemyBoostHand;
    [SerializeField] private GameObject PlayerSelectedCard;
    [SerializeField] private GameObject EnemySelectedCard;

    public int numericCardsCount = 16;
    private Dictionary<int, GameObject> NumericDictionary = new();
    private Dictionary<string, GameObject> BoostDictionary = new();
    private Dictionary<string, GameObject> BoostMarkDictionary = new();
    private readonly List<Sprite> NumericSpriteList = new();
    private string[] operations = {"+","*","-"};

    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerNumericHand = GameObject.Find("PlayerNumericHand");
        PlayerBoostHand = GameObject.Find("PlayerBoostHand");
        EnemyNumericHand = GameObject.Find("EnemyNumericHand");
        EnemyBoostHand = GameObject.Find("EnemyBoostHand");
        PlayerSelectedCard = GameObject.Find("SelectedPlayerCard");
        EnemySelectedCard = GameObject.Find("SelectedEnemyCard");
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

        for(int i = 0; i < numericCardsCount; i++){
            NumericDictionary[i] = Resources.Load<GameObject>("NumericCards/Num_Card_" + i); 
            NumericSpriteList.Add(Resources.Load<Sprite>("Sprites/NumericCards/Card_" + i));
        } 

        Debug.Log(NumericSpriteList[12]);

        for(int i = 1; i <= 4;i++){
            BoostDictionary["-" + i] = Resources.Load<GameObject>("BoostCards/Boost_Card_min_" + i);
        } 
        
        for(int i = 1; i <= 4; i++){
            BoostDictionary["+" + i] = Resources.Load<GameObject>("BoostCards/Boost_Card_p_" + i);
        }

        for(int i = 0; i <= 4; i++){
            if(i == 1) continue;

            BoostDictionary["*" + i] = Resources.Load<GameObject>("BoostCards/Boost_Card_mul_" + i);
        }
    }

    [Command]
    public void CmdDealNumericCards()
    {
        for(int i = 0; i < 4; i++){
            GameObject ranCard = Instantiate(NumericDictionary[UnityEngine.Random.Range(0,16)]);
            NetworkServer.Spawn(ranCard, connectionToClient);
            RpcShowNumericCard(ranCard, "Dealt");
        }
    }

    [Command]
    public void CmdDealBoostCards(){
        for(int i = 0; i < 6; i++){
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
            GameObject ranCard = Instantiate(BoostDictionary[ranOperation + ranValue]);
            NetworkServer.Spawn(ranCard, connectionToClient);
            RpcShowBoostCard(ranCard, "Dealt");
        }
    }

    [Command]
    public void CmdSelectCard(GameObject card){
        RpcShowNumericCard(card, "Selected");
    }

    [Command]
    public void CmdMoveSelectedCardToHand(GameObject card){
        RpcMoveSelectedCardToHand(card);
    }

    [Command]
    public void CmdApplyBoost(GameObject boostCard){
        RpcShowBoostCard(boostCard, "Selected");
    }

    [Command]
    public void CmdChangeCard(GameObject card, int newValue){
        RpcChangeCard(card, newValue);
    }

    [ClientRpc]
    private void RpcShowNumericCard(GameObject card ,string type){
        if(type == "Dealt"){
            if(isOwned){
                PlayerNumericHand.GetComponent<NumericHandController>().DrawCard(card);
            }
            else{
                EnemyNumericHand.GetComponent<NumericHandController>().DrawCard(card);
                //uncomment after boostApply is completed
                //card.GetComponentInChildren<CardFlipper>().Flip();
            }
        }
        else if(type == "Selected"){
            if(isOwned){
                PlayerSelectedCard.GetComponent<SelectedCard>().ChangeCard(card);
            }
            else{
                EnemySelectedCard.GetComponent<SelectedCard>().ChangeCard(card);
            }
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
            if(isOwned){
                PlayerSelectedCard.GetComponent<SelectedCard>().ApplyBoost(card);
            }
            else{
                EnemySelectedCard.GetComponent<SelectedCard>().ApplyBoost(card);
            }
        }
    }

    [ClientRpc]
    private void RpcMoveSelectedCardToHand(GameObject card){
        if(isOwned){
            PlayerNumericHand.GetComponent<NumericHandController>().MoveSelectedCardToHand(card);
        }
        else{
            EnemyNumericHand.GetComponent<NumericHandController>().MoveSelectedCardToHand(card);
        }
    }

    [ClientRpc]
    private void RpcChangeCard(GameObject card, int newValue){
        card.GetComponentInChildren<SpriteRenderer>().sprite = NumericSpriteList[newValue];
        card.GetComponent<CardController>().CardValue = newValue;
    }
}
