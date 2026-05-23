using TMPro;
using UnityEngine;

enum Apple
{
    badApple,godApple,apple
}

public class AppleManager : MonoBehaviour
{
    public static AppleManager Instance;

    [SerializeField] private GameObject[] apples;
    public TextMeshProUGUI scoreTMP;
    public int nowScore = 0;

    private int baseSpeed = 1;
    private int addSpeed = 2;
    private int speedUpDankai = 7;
    private int maxSpeed;

    private bool isCreate = false;
    private int unLockAppleNum= 10;
    private const int godAppleSpownNum = 5;
    private bool[] god = new bool[godAppleSpownNum];
    private int godSpownCount = 0;


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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxSpeed = baseSpeed + addSpeed * speedUpDankai;
        scoreTMP.text = nowScore + "個";
        CreateApple(apples[(int)Apple.apple]);
        //一定確率で生成するようにする
        RandomGodApple();
    }

    private void Update()
    {
        //10個ゲットしたら解禁される
        if (nowScore >= unLockAppleNum && !isCreate)
        {
            CreateApple(apples[(int)Apple.badApple]);
            isCreate = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //appleは常に生成し続ける
        if(col.gameObject.CompareTag("Apple"))
        {
            SoundManager.Instance.PlayGetAudio();
            //スコアを加算する
            nowScore++;
            //テキストの更新
            scoreTMP.text = nowScore + "個";
            //次のリンゴを作る
            CreateApple(col.gameObject);
            //プレイヤーの速度を上げる
            CharaAddSpeed();
            Destroy(col.gameObject);

            if (isCreate)
            {
                //一定確率でゴッドアップルを出現させる
                if (god[godSpownCount])
                {
                    CreateApple(apples[(int)Apple.godApple]);
                }

                godSpownCount++;

                //ゴッドアップルを出現させる確率をリセットする
                if (godSpownCount >= god.Length)
                {
                    godSpownCount = 0;
                    RandomGodApple();
                }
            }
        }

        if (col.gameObject.CompareTag("BadApple"))
        {
            SoundManager.Instance.PlayGetAudio();
            nowScore--;
            scoreTMP.text = nowScore + "個";
            CreateApple(col.gameObject);
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("GodApple"))
        {
            SoundManager.Instance.PlayGetAudio();
            nowScore += 5;
            scoreTMP.text = nowScore + "個";
            Destroy(col.gameObject);
        }
    }

    private void CreateApple(GameObject _apple)
    {
        //カメラの左下と右上のワールド座標を取得して、画面内に出現させる
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //プレイヤーと重ならないようにするために、プレイヤーの位置から一定距離以上離れた場所に出現させる
        //リンゴの当たり判定の半径
        float checkRadius = 0.5f;
        //重なりを避けたいレイヤーを名前で取得
        int layerMask = LayerMask.GetMask("Apple");
        //指定した円の中に「Player」レイヤーのコライダーがある間は再計算し続ける
        float randomX, randomY;
        Vector3 spawnPos;
        //リンゴが見切れないようにするための画面端からの余白
        float spawnTyousei = 0.5f;

        //一旦やってみる
        do
        {
            //取得した画面の端から端までの間でランダムなXとYの数値を決める
            randomX = Random.Range(min.x + spawnTyousei, max.x - spawnTyousei);
            randomY = Random.Range(min.y + spawnTyousei, max.y - spawnTyousei);
            //画面内のランダムな座標を指定して出現させる
            spawnPos = new Vector3(randomX, randomY, 0f);
        }
        //指定した円の中に「Player」レイヤーのコライダーがある間は再計算し続ける
        while (Physics2D.OverlapCircle(spawnPos, checkRadius, layerMask) != null);

        GameObject newApple = Instantiate(_apple, spawnPos, Quaternion.identity);
        //リンゴの名前から「(Clone)」を消す
        newApple.name= newApple.name.Replace("(Clone)", "");
    }

    private void RandomGodApple()
    {
        //5個中のどこかでゴッドアップルを出現させる
        for (int i = 0; i < god.Length; i++)
        {
            god[i] = false;
        }
        int godNum = Random.Range(0, god.Length);
        god[godNum] = true;
    }

    private void CharaAddSpeed()
    {
        if (CharaMove.Instance.speed < maxSpeed)
        {
            CharaMove.Instance.speed += addSpeed;
        }
    }
}
