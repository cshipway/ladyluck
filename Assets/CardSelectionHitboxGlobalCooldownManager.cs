using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionHitboxGlobalCooldownManager : MonoBehaviour
{
    private static float cooldown;
    public static bool CoolingDown = cooldown > 0;

    [SerializeField] private float localCooldown;

    // Update is called once per frame
    void Update()
    {
        if(cooldown > 0)
        {
            cooldown = Mathf.Clamp(cooldown - Time.deltaTime, 0, cooldown);
        }

        localCooldown = cooldown;
    }

    public static void Cooldown()
    {
        cooldown = 0.5f;
    }
}
