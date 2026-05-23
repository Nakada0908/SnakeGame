using UnityEngine;
using UnityEngine.InputSystem;

public class ESCGame : MonoBehaviour
{
    private static ESCGame Instance;

    private void Start()
    {
        //DontDestroyOnLoadを使ってるから重複しないようにする
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //シーン遷移で破棄されないように設定し、使い回す
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //キーボードが存在し、かつESCキーが押されたか確認
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
