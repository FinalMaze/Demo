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
    public FSMManager(int count)
    {
        allState = new FsmBase[count];
    }

    public void AddState(FsmBase tmpFSM)
    {
        if (stateCount > allState.Length)
        {
            return;
        }
        stateCount++;
        try
        {
            allState[stateCount] = tmpFSM;
        }
        catch (System.Exception)
        {
            Debug.Log(tmpFSM);
        }
    }
    public void ChangeState(sbyte animationrCount)
    {
        if (state == animationrCount)
        {
            return;
        }
        if (state != -1)
        {
            allState[state].OnExit();
        }
        state = animationrCount;
        try
        {
            allState[state].OnEnter();
        }
        catch (System.Exception)
        {
            Debug.Log(state);
        }

    }
    public void OnStay()
    {
        if (state != -1)
        {
            allState[state].OnStay();
        }
    }
}
