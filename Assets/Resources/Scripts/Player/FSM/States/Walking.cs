using UnityEngine;

public class Walking : IPlayerState
{
    public IPlayerState HandleInput(PlayerController player)
    {
        if (player.InputHandler.Movement.x == 0.0f && player.GroundCheckScript._collided)
            return new Idle();
        return null;
    }

    public void OnUpdate(PlayerController player)
    {
#if DEBUG
        if(player.Debug) Debug.Log("Player State: <b>Walking</b>");
#endif
    }

    public void OnFixedUpdate(PlayerController player)
    {
        player.ApplyXMovement();
    }
}
