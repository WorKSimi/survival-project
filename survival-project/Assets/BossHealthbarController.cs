using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthbarController : MonoBehaviour
{
    [SerializeField] private GameObject bossHealthBarUIObject;
    [SerializeField] private HealthBar bossHealthBar;
    public void EnableBossHealthbar()
    {
        bossHealthBarUIObject.SetActive(true);
    }

    public void DisableBossHealthbar()
    {
        bossHealthBarUIObject.SetActive(false);
    }

    public void SetBossHealthbar(float health)
    {
        bossHealthBar.SetHealth(health);
    }
}
