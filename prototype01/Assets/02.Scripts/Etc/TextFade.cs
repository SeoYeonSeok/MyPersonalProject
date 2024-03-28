using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public float fadeDuration = 0.5f;
    public float alphaValue = 100f;
    private bool isFadingIn = true;



    void Start()
    {
        if (txt == null) txt = transform.GetComponent<TextMeshProUGUI>();

        StartCoroutine(FadingText());
    }

    IEnumerator FadingText()
    {
        while (true)
        {
            // isFadingIn�� true�� ���, �ؽ�Ʈ�� ������ �������ϰ� ����ϴ�.
            if (isFadingIn)
            {
                while (txt.color.a < 1)
                {
                    Color color = txt.color;
                    color.a += Time.deltaTime / fadeDuration;
                    txt.color = color;
                    yield return null;
                }
                isFadingIn = false;
            }
            else // isFadingIn�� false�� ���, �ؽ�Ʈ�� ������ �����ϰ� ����ϴ�.
            {
                while (txt.color.a > 0)
                {
                    Color color = txt.color;
                    color.a -= Time.deltaTime / fadeDuration;
                    txt.color = color;
                    yield return null;
                }
                isFadingIn = true;
            }
        }
    }
}
