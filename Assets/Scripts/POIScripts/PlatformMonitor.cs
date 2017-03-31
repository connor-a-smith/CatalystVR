using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformMonitor : MonoBehaviour {

    [SerializeField] private static string startText = "Welcome to the CAVEkiosk!\n\nUse the Xbox controller to interact with this exhibit, and press the Right Trigger for a list of detailed controls.";

    private static MonitorButtonScript[] monitorButtons;
    private static int selectedButtonIndex = 0;
    public static bool monitorButtonsActive = false;
    private static PlatformInfoText monitorText;

	// Use this for initialization
	void Start () {

        monitorButtons = GetComponentsInChildren<MonitorButtonScript>(true);

        monitorText = GetComponentInChildren<PlatformInfoText>();

        SetMonitorText(startText);

        DeactivateMonitorButtons();

	}

    public void Update()
    {

        if (monitorButtonsActive)
        {

            if (GamepadInput.GetDown(GamepadInput.InputOption.A_BUTTON))
            {

             //  monitorButtons[selectedButtonIndex].ToggleButton();

            }

            if (GamepadInput.GetDown(GamepadInput.InputOption.LEFT_STICK_HORIZONTAL))
            {

                float value = GamepadInput.GetInputValue(GamepadInput.InputOption.LEFT_STICK_HORIZONTAL);

                if (value < 0)
                {
                    SelectNextButton(-1);
                }
                else if (value > 0)
                {
                    SelectNextButton(1);
                }

            }

            else if (GamepadInput.GetDown(GamepadInput.InputOption.RIGHT_STICK_HORIZONTAL))
            {
                float value = GamepadInput.GetInputValue(GamepadInput.InputOption.RIGHT_STICK_HORIZONTAL);

                if (value < 0)
                {
                    SelectNextButton(-1);
                }
                else if (value > 0)
                {
                    SelectNextButton(1);
                }
            }
        }
    }

    public static void SetMonitorText(string text)
    {
        if (monitorText != null)
        {
            monitorText.SetText(text);
        }
    }

    public static void ResetMonitorText()
    {
        monitorText.SetText(startText);
    }


    public static void SelectNextButton(int direction)
    {

        int curIndex = selectedButtonIndex;

        do
        {

            curIndex += direction;

            if (curIndex < 0)
            {
                curIndex = monitorButtons.Length - 1;
            }
            else if (curIndex > monitorButtons.Length - 1)
            {
                curIndex = 0;
            }

            if (monitorButtons[curIndex].activatable)
            {

                monitorButtons[curIndex].select();
                monitorButtons[selectedButtonIndex].deselect();
                selectedButtonIndex = curIndex;
                return;

            }


        } while (curIndex != selectedButtonIndex);
    }

    public static void ActivateMonitorButtons()
    {

        foreach (MonitorButtonScript monitorButton in monitorButtons)
        {

            monitorButton.gameObject.SetActive(true);
            monitorButton.UpdateButtonState();

        }

        monitorButtonsActive = true;
        selectedButtonIndex = monitorButtons.Length - 1;
        SelectNextButton(1);
    }

    public static void DeactivateMonitorButtons()
    {

        foreach (MonitorButtonScript monitorButton in monitorButtons)
        {
            if (monitorButton != null)
            {
                monitorButton.activatable = false;
                monitorButton.gameObject.SetActive(false);
            }


        }

        selectedButtonIndex = 0;
        monitorButtonsActive = false;

    }
}
