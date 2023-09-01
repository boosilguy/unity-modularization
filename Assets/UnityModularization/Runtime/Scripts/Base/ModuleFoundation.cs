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

public enum ETransitionCommand
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
        readonly ETransitionCommand CurrentCommand;
        readonly EModuleState CurrentState;
        
        public StateTransition(EModuleState state, ETransitionCommand transition)
        {
            CurrentState = state;
            CurrentCommand = transition;
        }

        public override int GetHashCode() => 17 + 31 * CurrentState.GetHashCode() + 31 * CurrentCommand.GetHashCode();

        public override bool Equals(object obj) => obj is StateTransition other && this.Equals(other);

        public bool Equals(StateTransition other) => CurrentState == other.CurrentState && CurrentCommand == other.CurrentCommand;
    }

    public ModuleFoundation()
    {
        CurrentState = EModuleState.Created;

        transitions = new Dictionary<StateTransition, EModuleState>();
        lifeCycleActions = new Dictionary<EModuleState, ModuleLifeCycle>();
        
        transitions.Add(new StateTransition(EModuleState.Created, ETransitionCommand.Initialize), EModuleState.Inactive);
        transitions.Add(new StateTransition(EModuleState.Inactive, ETransitionCommand.None), EModuleState.Active);
        transitions.Add(new StateTransition(EModuleState.Active, ETransitionCommand.None), EModuleState.Active);
        transitions.Add(new StateTransition(EModuleState.Inactive, ETransitionCommand.Destroy), EModuleState.Destroyed);
        transitions.Add(new StateTransition(EModuleState.Active, ETransitionCommand.Destroy), EModuleState.Destroyed);
        
        lifeCycleActions.Add(EModuleState.Inactive, ModuleInitialize);
        lifeCycleActions.Add(EModuleState.Active, ModuleUpdate);
        lifeCycleActions.Add(EModuleState.Destroyed, ModuleTerminate);
        
        Process(ETransitionCommand.Initialize);
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

    internal void Process(ETransitionCommand transition = ETransitionCommand.None)
    {
        StateTransition stateTransition = new StateTransition(CurrentState, transition);
        EModuleState nextState;

        if (!transitions.TryGetValue(stateTransition, out nextState))
        {
            throw new Exception(
                RichTextUtil.GetColorfulText(
                    new ColorfulText("[Invalid transition] ", Color.red),
                    new ColorfulText("Cannot execute command ", Color.white),
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
