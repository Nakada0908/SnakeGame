using UnityEngine;
using UnityEngine.SceneManagement;

public class EndToTitleScene : MonoBehaviour
{
    private InputSystem_Actions input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = InputManager.Instance.input;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Next.Next.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Title");
        }
    }
}
