using UnityEngine;

public class PlayerState
{
    public enum ActionState
    {
        Idle,
        forward,
        left,
        right,
        back,
        skill,
        Die,
    }

    public enum FireState
    {
        Normal,
        Burning
    }

    public enum MasterHP
    {
        Basic = 100,
        FireFIghter = 150
    }
}
