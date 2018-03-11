using UnityEngine;

public abstract class State : MonoBehaviour
{
    /// <summary>
    /// Enter the state (add all the listeners
    /// </summary>
    public virtual void Enter()
    {
        AddListeners();
    }

    /// <summary>
    /// Exit the state (remove all listeners)
    /// </summary>
    public virtual void Exit()
    {
        RemoveListeners();
    }

    /// <summary>
    /// On destroy, we remove the listeners (if we didn't exit properly)
    /// </summary>
    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    /// <summary>
    /// Add all the listeners
    /// </summary>
    protected virtual void AddListeners()
    {
    }

    /// <summary>
    /// Remove all the listeners
    /// </summary>
    protected virtual void RemoveListeners()
    {
    }
}
