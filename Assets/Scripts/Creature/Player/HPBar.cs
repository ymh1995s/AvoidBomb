using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image hpBar;

    public void UpdateHp(float retion)
    {
        hpBar.fillAmount = retion;
    }
}
