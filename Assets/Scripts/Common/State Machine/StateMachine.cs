using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected State currentState;
    protected bool inTransition;

    public virtual State CurrentState
    {
        get { return this.currentState; }
        set { Transition(value); }
    }

    public virtual T GetState<T>() where T : State
    {
        T target = GetComponent<T>();

        if (target == null)
        {
            target = this.gameObject.AddComponent<T>();
        }

        return target;
    }

    /// <summary>
    /// Change the current state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }

    /// <summary>
    /// Transition from one state to the ohter
    /// </summary>
    /// <param name="value"></param>
    protected virtual void Transition(State value)
    {
        if (this.currentState != value && !this.inTransition)
        {
            this.inTransition = true;

            if (this.currentState != null)
            {
                this.currentState.Exit();
            }

            this.currentState = value;

            if (this.currentState != null)
            {
                currentState.Enter();
            }

            this.inTransition = false;
        }
    }
}
