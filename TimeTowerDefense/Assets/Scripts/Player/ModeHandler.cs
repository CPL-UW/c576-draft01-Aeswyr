using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeHandler : MonoBehaviour
{
    [SerializeField] private GameObject selector;
    int mode = 1;
    public void Increment(int dir) {
        mode += dir;
        if (mode > 2)
            mode = 0;
        if (mode < 0)
            mode = 2;

        selector.transform.localPosition = new Vector3(3.125f * (mode - 1), 0, 0);
    }

    public Mode GetMode() {
        return (Mode)(mode + 1);
    }

    public void ForceMode(Mode newMode) {
        mode = (int)newMode - 1;
        GameController.Instance.Gamemode = newMode;
        selector.transform.localPosition = new Vector3(3.125f * (mode - 1), 0, 0);
    }

    public void SyncMode() {
        mode = (int)GameController.Instance.Gamemode - 1;
        selector.transform.localPosition = new Vector3(3.125f * (mode - 1), 0, 0);
    }

}
