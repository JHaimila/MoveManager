using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    [Tooltip("This prefab will only be spawned once and persisted between scenes"), SerializeField] GameObject persistenObjectPrefab = null;

    private static bool hasSpawned = false;

    private void Awake() {
        if(hasSpawned) return;

        SpawnPersistenctObjects();
        hasSpawned = true;
    }
    private void SpawnPersistenctObjects()
    {
        GameObject persistentObject = Instantiate(persistenObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
public interface LocomotionContext
{
    public void SetState(LocomotionState state);
}
public interface LocomotionState
{
    public void Jump(LocomotionContext context);
    public void Fall(LocomotionContext context);
    public void Land(LocomotionContext context);
    public void Crouch(LocomotionContext context);
}

public class LocomotionStatePattern : MonoBehaviour, LocomotionContext
{
    LocomotionState currentState = new GroundedState();
    void Jump() => currentState.Jump(this);
    void Fall() => currentState.Fall(this);
    void Land() => currentState.Land(this);
    void Crouch() => currentState.Crouch(this);

    public void SetState(LocomotionState newState)
    {
        currentState = newState;
    }
}

public class GroundedState : LocomotionState
{
    public void Jump(LocomotionContext context)
    {
        context.SetState(new CrouchingState());
    }
    public void Fall(LocomotionContext context)
    {
        context.SetState(new InAirState());
    }
    public void Land(LocomotionContext context)
    {
        context.SetState(new InAirState());
    }
    public void Crouch(LocomotionContext context)
    {
        context.SetState(new CrouchingState());
    }
}
public class InAirState : LocomotionState
{
    public void Jump(LocomotionContext context)
    {

    }
    public void Fall(LocomotionContext context)
    {

    }
    public void Land(LocomotionContext context)
    {
        context.SetState(new GroundedState());
    }
    public void Crouch(LocomotionContext context)
    {
        // context.SetState(new GroundedState());
    }
}
public class CrouchingState : LocomotionState
{
    public void Jump(LocomotionContext context)
    {

    }
    public void Fall(LocomotionContext context)
    {

    }
    public void Land(LocomotionContext context)
    {

    }
    public void Crouch(LocomotionContext context)
    {
        context.SetState(new CrouchingState());
    }
}