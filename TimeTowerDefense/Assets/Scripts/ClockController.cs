using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{

    [SerializeField] private GameObject minHand;
    [SerializeField] private GameObject hrHand;
    [SerializeField] private GameObject minButtons;
    [SerializeField] private GameObject minKey;
    [SerializeField] private GameObject hrButtons;
    [SerializeField] private GameObject hrKey;
    private long currtime;
    private long targettime;
    public void Apply() {
        Analytics.LogEvent("CLOCKCHANGE", $"{currtime-targettime}");
        GameController.Instance.StartRollback(targettime);
        GameController.Instance.Gamemode = Mode.MOVE;
        GameController.Instance.SyncPlayerMode();
    }

    public void TuneMinutesActive() {
        minButtons.SetActive(true);
        hrButtons.SetActive(false);
    }

    public void TuneHoursActive() {
        minButtons.SetActive(false);
        hrButtons.SetActive(true);
    }
    public void MinUp() {
        if (targettime + 25 > currtime)
            return;
        targettime += 25;
        minKey.transform.Rotate(Quaternion.Euler(0, 0, 30).eulerAngles);
        UpdateClockFace();
    }
    public void MinDown() {
        if (targettime - 25 < 0)
            return;
        targettime -= 25;
        minKey.transform.Rotate(Quaternion.Euler(0, 0, -30).eulerAngles);
        UpdateClockFace();
    }
    public void HrUp() {
        if (targettime + 300 > currtime)
            return;
        targettime += 300;
        hrKey.transform.Rotate(Quaternion.Euler(0, 0, 30).eulerAngles);
        UpdateClockFace();
    }

    public void HrDown() {
        if (targettime - 300 < 0)
            return;
        targettime -= 300;
        hrKey.transform.Rotate(Quaternion.Euler(0, 0, -30).eulerAngles);
        UpdateClockFace();
    }

    public void UpdateClockFace() {
        int min = (int)((targettime / 25) % 12) * 5;
        int hr = (int)((targettime / 5 / 60) % 12);

        minHand.transform.rotation = Quaternion.Euler(0, 0, -360 * min / 60);
        hrHand.transform.rotation = Quaternion.Euler(0, 0, -360 * hr / 12);
    }

    public void OpenClock() {
        currtime = HUDController.Instance.GetTick();
        targettime = currtime;
        UpdateClockFace();
    }
}
