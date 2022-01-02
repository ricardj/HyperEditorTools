using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProFader : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public float waitTime = 0.01f;
    public int characterSkip = 10;
    TMP_TextInfo textInfo;

    void Awake()
    {
        _text = gameObject.GetComponent<TextMeshProUGUI>();
        Color originalColor = _text.color;
        originalColor.a = 0;

        _text.color = originalColor;
        textInfo = _text.textInfo;
    }

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        _text.ForceMeshUpdate();
        TMP_CharacterInfo info;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (textInfo.characterInfo[i].character == ' ')
                continue;
            info = textInfo.characterInfo[i];
            StartCoroutine(FadeInLetters(info));

            yield return new WaitForSeconds(waitTime);
        }

    }

    private IEnumerator FadeInLetters(TMP_CharacterInfo info)
    {
        for (int i = 0; i <= 255; i += characterSkip)
        {
            int meshIndex = textInfo.characterInfo[info.index].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[info.index].vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0].a = (byte)i;
            vertexColors[vertexIndex + 1].a = (byte)i;
            vertexColors[vertexIndex + 2].a = (byte)i;
            vertexColors[vertexIndex + 3].a = (byte)i;
            _text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
