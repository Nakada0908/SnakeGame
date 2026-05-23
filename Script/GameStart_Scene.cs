using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart_Scene : MonoBehaviour
{
    private InputSystem_Actions input;

    void Start()
    {
        input = InputManager.Instance.input;
        InputManager.Instance.SwitchMode(InputMode.Next);
        Application.targetFrameRate = 60;
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Next.Next.WasPressedThisFrame())
        {
            InputManager.Instance.SwitchMode(InputMode.Player);
            SceneManager.LoadScene("Game");
        }
    }
}
