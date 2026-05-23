using TMPro;
using UnityEngine;

public class Tennmetu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private float count = 0;
    private float maxCount = 0.5f;

    private void OnEnable()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (count >= maxCount)
        {
            count = 0;
            text.enabled = !text.enabled;
        }
    }
}
