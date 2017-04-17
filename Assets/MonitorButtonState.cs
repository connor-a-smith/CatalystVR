using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorButtonState : MonoBehaviour {

    public enum ButtonState {

        ACTIVE,
        PRESSED,
        SELECTED,
        DISABLED

    }

    public ButtonState state;
}
