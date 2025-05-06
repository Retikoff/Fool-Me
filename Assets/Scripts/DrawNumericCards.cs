using Mirror;
using UnityEngine;

public class DrawNumericCards : NetworkBehaviour
{
    public PlayerManager playerManager;

    public void OnClick(){
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();
        playerManager.CmdDealNumericCards(); 
    }
}
