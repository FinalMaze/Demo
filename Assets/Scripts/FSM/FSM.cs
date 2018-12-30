using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FsmBase
{
    public virtual void OnEnter() { }
    public virtual void OnStay() { }
    public virtual void OnExit() { }
}


public class FSMManager
{
    public FsmBase[] allState;
    sbyte stateCount = -1;
    sbyte state = -1;
    public FSMManager()
    {
        allState = new FsmBase[(int)Data.AnimationCount.Max];
    }

    public void AddState(FsmBase tmpFSM)
    {
        if (stateCount>allState.Length)
        {
            return;
        }
        stateCount++;
        allState[stateCount]=tmpFSM;
    }
    public void ChangeState(sbyte animationrCount)
    {
        if (state==animationrCount)
        {
            return;
        }
        if (state!=-1)
        {
            allState[state].OnExit();
        }
        state = animationrCount;
        allState[state].OnEnter();
    }
    public void OnStay()
    {
        if (state!=-1)
        {
            allState[state].OnStay();
        }
    }
}
