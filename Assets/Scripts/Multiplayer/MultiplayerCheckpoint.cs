using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCheckpoint : Photon.PunBehaviour {

    public GameObject playerHost;
    public GameObject playerOther;
    public Vector3 playerPositionHost;
    public Vector3 playerPositionOther;

    private bool checkpointReached = true;

    [PunRPC]
    private void RespawnPosition()
    {
        if (checkpointReached)
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (playerHost != null)
                {
                    playerHost.transform.position = playerPositionHost;
                }
            }

            else
            {
                if (playerPositionOther != null)
                {
                    playerOther.transform.position = playerPositionOther;
                }
            }
        }
    }

    public void Respawn()
    {
        photonView.RPC("RespawnPosition", PhotonTargets.All);
    }
}
