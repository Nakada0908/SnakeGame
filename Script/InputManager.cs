using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    Player,
    Next
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputSystem_Actions input { get; private set; }
    //現在アクティブなアクションマップを保持する変数
    private InputActionMap nowinput;

    //シーン読み込み前に自動で1回だけ実行される
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        //プログラム側でInputManagerという名前の空のGameObjectを新規作成する
        GameObject managerObj = new GameObject("InputManager");
        //作成した空のオブジェクトに、このInputManagerスクリプトをアタッチする
        managerObj.AddComponent<InputManager>();
    }

    private void Awake()
    {
        //DontDestroyOnLoadを使ってるから重複しないようにする
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //インスタンスを保持
        Instance = this;
        //シーン遷移で破棄されないように設定し、使い回す
        DontDestroyOnLoad(gameObject);

        //入力処理の初期化
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        SwitchMode(InputMode.Player);
    }

    private void OnDisable()
    {
        input.Disable();
    }
    private void OnDestroy()
    {
        input.Dispose();
    }

    //InputManager.Instance.SwitchMode(InputMode.UI);のように呼び出す
    public void SwitchMode(InputMode mode)
    {
        //渡されたEnumをもとに、対応するアクションマップを切り替える
        switch (mode)
        {
            case InputMode.Player:
                SwitchActionMap(input.Player);
                break;
            case InputMode.Next:
                SwitchActionMap(input.Next);
                break;
        }
    }

    //どのアクションマップでも汎用的に切り替えられるメソッド
    public void SwitchActionMap(InputActionMap newMap)
    {
        //現在アクティブなマップが同じなら何もしない
        if (nowinput == newMap){ return; }

        //現在アクティブなマップがあれば無効化する
        if (nowinput != null)
        {
            nowinput.Disable();
        }

        //新しいマップを現在のマップとして上書き
        nowinput = newMap;
        //新しいマップを有効化
        nowinput.Enable();
    }
}