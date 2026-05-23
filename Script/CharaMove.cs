using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

enum NowHoukou
{
    Up,
    Down,
    Left,
    Right
}

public class CharaMove : MonoBehaviour
{
    private InputSystem_Actions input;
    public static CharaMove Instance;
    private Animator animator;

    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private TextMeshProUGUI Ositene;
    [SerializeField] private GameObject tennmetu;

    public int speed;
    private float posX, posY;
    private NowHoukou nowHoukou;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else        
        {
            Destroy(this.gameObject); 
        }
    }

    void Start()
    {
        input = InputManager.Instance.input;
        animator = GetComponentInChildren<Animator>();

        GameOverText.enabled = false;
        Ositene.enabled = false;
        tennmetu.SetActive(false);

        //初期は下に動くようにする
        posX = 0;
        posY = -1;
        speed = 1;
        nowHoukou = NowHoukou.Down;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Player.MoveUp.WasPressedThisFrame())
        {
            posX = 0f; posY = 1f;
            animator.SetTrigger("Move_U");
            nowHoukou = NowHoukou.Up;
        }
        if (input.Player.MoveDown.WasPressedThisFrame())
        {
            posX = 0f; posY = -1f;
            animator.SetTrigger("Move_D");
            nowHoukou = NowHoukou.Down;
        }
        if (input.Player.MoveRight.WasPressedThisFrame())
        {
            posX = 1f; posY = 0f;
            animator.SetTrigger("Move_R");
            nowHoukou = NowHoukou.Right;
        }
        if (input.Player.MoveLeft.WasPressedThisFrame())
        {
            posX = -1f; posY = 0f;
            animator.SetTrigger("Move_L");
            nowHoukou = NowHoukou.Left;
        }

        //常に現在の方向に動かし続ける
        transform.Translate(posX * speed * Time.deltaTime, posY * speed * Time.deltaTime, 0);
    }

    //プレイヤー死亡時の処理
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.CompareTag("DeadArea")) { return; }

        //プレイヤーに対しての処理
        speed = 0;
        animator.enabled = false;

        //壁からプレイヤーを離す
        float modosi = 1f;
        Vector3 wallPos=col.gameObject.transform.position;
        Vector3 playerPos=transform.position;

        switch(nowHoukou)
        {
            case NowHoukou.Up:
                playerPos.y = wallPos.y - modosi;
                break;
            case NowHoukou.Down:
                playerPos.y = wallPos.y + modosi;
                break;
            case NowHoukou.Left:
                playerPos.x = wallPos.x + modosi;
                break;
            case NowHoukou.Right:
                playerPos.x = wallPos.x - modosi;
                break;
        }
        // 修正した座標を適用
        transform.position = playerPos;

        //文字表示
        tennmetu.SetActive(true);
        GameOverText.enabled = true;
        Ositene.enabled = true;

        //何か押したらエンドに行けるようにする
        InputManager.Instance.SwitchMode(InputMode.Next);
        StartCoroutine(NextScene());
    }

    private IEnumerator NextScene()
    {
        while (true)
        {
            if (input.Next.Next.WasPressedThisFrame())
            {
                SceneManager.LoadScene("End");
            }
            yield return null;
        }
    }
}
