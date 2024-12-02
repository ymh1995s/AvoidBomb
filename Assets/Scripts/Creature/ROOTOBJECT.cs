using UnityEngine;

public abstract class ROOTOBJECT : MonoBehaviour
{
    // 멀티게임 구상용
    protected string CLASS; // 어떤 클래스의 오브젝트인가?
    protected int ID; // 몇 번째 아이디인가?, 혹은 위 CLASS 삭제 후 ID로만 관리할 것인가?
}
