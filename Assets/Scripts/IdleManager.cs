using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IdleManager : MonoBehaviour {

    [SerializeField] private float timeBetweenSceneCycles = 10.0f;

    [SerializeField] private float fadeTime = 2.0f;

    [SerializeField] private float minutesUntilReset = 5.0f;

    [SerializeField] private float idleRotationAnglePerSecond = 5.0f;

    private GameManager gameManager;

    // Use this for initialization
    void Start () {

        gameManager = GetComponentInParent<GameManager>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public IEnumerator CheckForInput()
    {

        while (true)
        {

            if (GamepadInput.timeSinceLastInput > (minutesUntilReset * 60.0f))
            {

                gameManager.cameraRig.Set3D(false);

                StartCoroutine(CycleScenes());

                GameManager.gameState = GameManager.State.IDLE;

                break;

            }

            yield return null;

        }
    }

    public IEnumerator CycleScenes()
    {

        // Do a fade-out so user doesn't see transitions.
        yield return StartCoroutine(FadePlane(true, fadeTime));

        // Make the platform invisible.
        gameManager.platform.SetPlatformVisible(false);

        // Start the rotation routine that rotates the user and pans around different sites.
        Coroutine rotationRoutine = StartCoroutine(gameManager.platform.RotatePlatformWhileIdle(idleRotationAnglePerSecond));

        // Start the scence cycling routine that cylces between different scenes periodically.
        Coroutine cyclingCoroutine = StartCoroutine(CycleSceneTransition());

        // Wait for user input before proceeding.
        yield return StartCoroutine(WaitForControllerInput());

        /* Everything after this will only be reached when user inputs something */

        // Stop the automatic scene cycling.
        StopCoroutine(cyclingCoroutine);

        // Do the fade-out so user can't see transition.
        yield return StartCoroutine(FadePlane(true, 0.01f));

        // Make the platform visible again.
        gameManager.platform.SetPlatformVisible(true);

        // Stop the automatic platform rotation.
        StopCoroutine(rotationRoutine);

        // NOTE: This line resets the user back to the Earth scene. May want to change later.
        SceneManager.LoadScene(0);

        // Do a fade-in now that everything is ready.
        yield return StartCoroutine(FadePlane(false, fadeTime / 2));

        // Game is active again.
        GameManager.gameState = GameManager.State.ACTIVE;

    }

    public IEnumerator CycleSceneTransition()
    {

        while (true)
        {

            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
            {

                sceneIndex = 1;

            }

            yield return StartCoroutine(FadePlane(true, fadeTime));

            SceneManager.LoadScene(sceneIndex);

            yield return StartCoroutine(FadePlane(false, fadeTime));

            yield return new WaitForSeconds(timeBetweenSceneCycles);

        }
    }

    public IEnumerator WaitForControllerInput()
    {

        while (true)
        {

            // Floats may have rounding issues, so there's a margin.
            if (GamepadInput.timeSinceLastInput <= 0.1f)
            {

                break;

            }

            yield return null;
        }
    }

    public IEnumerator FadePlane(bool fadeIn, float newFadeTime)
    {

        GameObject plane = GameManager.instance.fadePlane;

        MeshRenderer renderer = plane.GetComponent<MeshRenderer>();

        Color startColor = renderer.material.color;
        Color endColor = startColor;


        if (fadeIn)
        {

            endColor.a = 1.0f;

        }
        else
        {

            endColor.a = 0.0f;
        }

        if (startColor.a != endColor.a)
        {

            for (float i = 0; i < fadeTime; i += Time.deltaTime)
            {

                renderer.material.SetColor("_Color", Color.Lerp(startColor, endColor, i / newFadeTime));

                yield return null;

            }

            renderer.material.SetColor("_Color", endColor);

        }
    }
}
