using Mirror;
using UnityEngine;

public class DrawBoostCards : NetworkBehaviour
{
    public PlayerManager playerManager;

    public void OnClick(){
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();
        playerManager.CmdDealBoostCards();
    }
}
