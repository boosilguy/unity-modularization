using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public enum EModuleState
{
    Created,
    Inactive,
    Active,
    Destroyed
}

public enum ETransition
{
    Initialize,
    None,
    Destroy
}

public class ModuleFoundation : ScriptableObjectFoundation
{

    public EModuleState CurrentState { get; private set; }

    private Dictionary<StateTransition, EModuleState> transitions;
    private Dictionary<EModuleState, ModuleLifeCycle> lifeCycleActions;
    private ConcurrentQueue<Action> mainThreadWorks = new ConcurrentQueue<Action>();
    private delegate void ModuleLifeCycle();

    struct StateTransition
    {
        readonly ETransition CurrentTransition;
        readonly EModuleState CurrentState;
        
        public StateTransition(EModuleState state, ETransition transition)
        {
            CurrentState = state;
            CurrentTransition = transition;
        }

        public override int GetHashCode() => 17 + 31 * CurrentState.GetHashCode() + 31 * CurrentTransition.GetHashCode();

        public override bool Equals(object obj) => obj is StateTransition other && this.Equals(other);

        public bool Equals(StateTransition other) => CurrentState == other.CurrentState && CurrentTransition == other.CurrentTransition;
    }

    public ModuleFoundation()
    {
        CurrentState = EModuleState.Created;

        transitions = new Dictionary<StateTransition, EModuleState>();
        lifeCycleActions = new Dictionary<EModuleState, ModuleLifeCycle>();
        
        transitions.Add(new StateTransition(EModuleState.Created, ETransition.Initialize), EModuleState.Inactive);
        transitions.Add(new StateTransition(EModuleState.Inactive, ETransition.None), EModuleState.Active);
        transitions.Add(new StateTransition(EModuleState.Active, ETransition.None), EModuleState.Active);
        transitions.Add(new StateTransition(EModuleState.Inactive, ETransition.Destroy), EModuleState.Destroyed);
        transitions.Add(new StateTransition(EModuleState.Active, ETransition.Destroy), EModuleState.Destroyed);
        
        lifeCycleActions.Add(EModuleState.Inactive, ModuleInitialize);
        lifeCycleActions.Add(EModuleState.Active, ModuleUpdate);
        lifeCycleActions.Add(EModuleState.Destroyed, ModuleTerminate);
        
        Process(ETransition.Initialize);
    }

    public void RunOnMainThread(params Action[] action)
    {
        foreach (var item in action)
        {
            mainThreadWorks.Enqueue(item);
        }
    }

    public virtual void ModuleInitialize()
    {

    }

    public virtual void ModuleUpdate()
    {
        while (mainThreadWorks.TryDequeue(out Action action))
            action?.Invoke();
    }

    public virtual void ModuleTerminate()
    {

    }

    internal void Process(ETransition transition = ETransition.None)
    {
        StateTransition stateTransition = new StateTransition(CurrentState, transition);
        EModuleState nextState;

        if (!transitions.TryGetValue(stateTransition, out nextState))
        {
            throw new Exception(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("Invalid transition", Color.red),
                    new ColorfulText(": Cannot execute command ", Color.white),
                    new ColorfulText(transition.ToString(), Color.yellow),
                    new ColorfulText(" from state ", Color.white),
                    new ColorfulText(CurrentState.ToString(), Color.yellow))
            );
        }

        if (lifeCycleActions.ContainsKey(nextState))
        {
            lifeCycleActions[nextState].Invoke();
        }

        CurrentState = nextState;
    }
}
