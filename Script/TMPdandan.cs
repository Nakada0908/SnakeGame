using UnityEngine;
using TMPro;
using System.Collections;

public class TMPdandan : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] text;
    [SerializeField] private GameObject tennmetu;
    private bool isShowing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is creBated
    void Start()
    {
        //すべてのテキストを非表示にしておく
        for (int i = 0; i < text.Length; i++)
        {
            text[i].enabled = false;
        }
        tennmetu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //コルーチンで１つずつ表示する
        if(isShowing) { return; }

        isShowing = true;
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        //１にスコアを表示する
        text[1].text = AppleManager.Instance.nowScore + "個";

        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SoundManager.Instance.PlayKekkaAudio();
            text[i].enabled = true;
        }
        tennmetu.SetActive(true);
    }
}
