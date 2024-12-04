using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image hpBar;

    public void UpdateHp(int hp, int maxHp)
    {
        if (maxHp == 0) return;

        float retion = (float)hp / (float)maxHp;
        hpBar.fillAmount = retion;
    }
}
