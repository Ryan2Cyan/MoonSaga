using UnityEngine;

public class Idle : IPlayerState
{
    public void OnStart(PlayerController player)
    {
        player.PlayerDataScript._rigidbody2D.velocity = Vector2.zero;
    }

    public IPlayerState HandleInput(PlayerController player)
    {
        if (player.InputHandler.Movement.x != 0.0f && player.GroundCheckScript._collided)
            return new Walking();
        return null;
    }

    public void OnUpdate(PlayerController player)
    {
#if DEBUG
        if(player.Debug) Debug.Log("Player State: <b>Idle</b>");
#endif
    }
}
