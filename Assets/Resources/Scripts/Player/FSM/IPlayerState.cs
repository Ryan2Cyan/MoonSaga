public interface IPlayerState
{
    public virtual void OnStart(PlayerController player){}
    public virtual IPlayerState HandleInput(PlayerController player) { return null; }
    public virtual void OnUpdate(PlayerController player) {}
    public virtual void OnFixedUpdate(PlayerController player) {}
    public virtual void OnExit(PlayerController player) {}

    
}
