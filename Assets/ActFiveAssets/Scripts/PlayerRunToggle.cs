using AC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunToggle : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("PlayerRunToggle: No Player component found.");
        }
    }

    void Update()
    {
        if (player == null || KickStarter.stateHandler.gameState != GameState.Normal)
            return;

        // Use input from Adventure Creator's InputGetButton instead
        if (KickStarter.playerInput.InputGetButton("Run"))
        {
            player.isRunning = true;
        }
        else
        {
            player.isRunning = false;
        }
    }
}