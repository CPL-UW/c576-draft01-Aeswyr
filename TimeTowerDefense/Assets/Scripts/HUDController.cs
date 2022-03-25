using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : Singleton<HUDController>
{
    [SerializeField] private TextMeshProUGUI parts;
    [SerializeField] private TextMeshProUGUI ammo;

    [SerializeField] private TextMeshProUGUI clock;
    private long time = 0;
    public void DisplayParts(int count) {
        parts.text = $"x{count}";
    }

    public void DisplayAmmo(int count) {
        ammo.text = $"x{count}";
    }

    private void FixedUpdate() {
        if (GameController.Instance.unpaused)
        {
            if (GameController.Instance.tickDiff > 0)
                time--;
            else
                time++;
        }
        int min = (int)((time / 25) % 12) * 5;
        int hr = (int)((time / 5 / 60) % 12);
        string smin = "";
        if (min < 10)
            smin += "0";
        smin += min.ToString();
        string shr = "";
        if (hr < 10)
            shr += "0";
        shr += hr.ToString();
        clock.text = $"{shr}:{smin}";
    }

    public long ForceTickDiff(long newTick) {
        long dif = time - newTick;
        return dif;
    }

    public void ForceTick(long newTick) {
        time = newTick;
    }

    public long GetTick() {
        return time;
    }
}
