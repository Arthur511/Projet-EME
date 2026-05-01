using System;
using TMPro;
using UnityEngine;

public enum TextAnimationType
{
    None, Wave, Shake, Pulse, Rainbow, Skew, Orbit
}

public class TextAnimator : MonoBehaviour
{
    public TextAnimationType CurrentTextAnimation = TextAnimationType.Wave;

    [SerializeField] float _waveFrequency = 1f;
    [SerializeField] float _waveAmplitude = 1f;
    [SerializeField] float _waveSpeed = 1f;

    [SerializeField] float _shakeAmplitude = 1f;
    [SerializeField] float _skewAmount = 1f;


    TextMeshProUGUI _currentTextTMP;
    Mesh _currentTextMesh;

    private void Awake() => _currentTextTMP = GetComponent<TextMeshProUGUI>();

    // Update is called once per frame
    void Update()
    {
        _currentTextTMP.ForceMeshUpdate();
        _currentTextMesh = _currentTextTMP.mesh;

        var nbChar = _currentTextTMP.textInfo.characterCount;


        for (int i = 0; i < nbChar; i++) // Pour chaque caractère du texte
        {
            var charInfo = _currentTextTMP.textInfo.characterInfo[i]; // On récupère les infos du caractère
            if (!charInfo.isVisible) continue;

            if (CurrentTextAnimation == TextAnimationType.Pulse) DoPulse(i);
            else if (CurrentTextAnimation == TextAnimationType.Rainbow) DoRainbow(i);
            else if (CurrentTextAnimation == TextAnimationType.Skew) DoSkew(i);
            else
            {
                int meshIndex = charInfo.materialReferenceIndex; // On regarde son submesh (Sa bibliothèque ou sa police d'écriture)
                int vertexIndex = charInfo.vertexIndex; // On récupère l'index de son premier vertex (Un caractère est composé de 4 vertices)

                Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices; // Liste de tout les vertex de la chaîne de caractères

                // On calcule l'animation au caractère en fonction de son index
                Vector3 offset = GetOffset(i);
                vertices[vertexIndex] += offset;
                vertices[vertexIndex + 1] += offset;
                vertices[vertexIndex + 2] += offset;
                vertices[vertexIndex + 3] += offset;
            }
        }


        _currentTextTMP.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32); // On applique visuellement les modifications précédentes sur les vertex
    }

    private Vector3 GetOffset(int i)
    {
        return CurrentTextAnimation switch
        {
            TextAnimationType.Wave => DoWave(i),
            TextAnimationType.Shake => DoShake(i),
            TextAnimationType.Orbit => DoOrbit(i),
            _ => Vector3.zero
        };
    }

    private Vector3 DoOrbit(int i)
    {
        float angle = Time.time * _waveSpeed + i * _waveFrequency;
        float x = Mathf.Cos(angle) * _waveAmplitude;
        float y = Mathf.Sin(angle) * _waveAmplitude;
        return new Vector3(x, y, 0);
    }

    private Vector3 DoWave(int i)
    {
        float wave = Mathf.Sin(Time.time * _waveSpeed + i * _waveFrequency) * _waveAmplitude;
        return new Vector3(0, wave, 0);
    }

    private Vector3 DoShake(int i)
    {
        return new Vector3(
            UnityEngine.Random.Range(-_shakeAmplitude, _shakeAmplitude),
            UnityEngine.Random.Range(-_shakeAmplitude, _shakeAmplitude),
            0);
    }

    private void DoPulse(int i)
    {
        int startIndex = _currentTextTMP.textInfo.characterInfo[i].vertexIndex;
        Vector3 center = (_currentTextTMP.textInfo.meshInfo[0].vertices[startIndex] +
                          _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + 2]) / 2; // On calcule le centre du caractère
        float scale = 1f + Mathf.Sin(Time.time * 2f + i) * 0.2f;

        for (int j = 0; j < 4; j++)
            _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + j] = center + (_currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + j] - center) * scale;

    }
    private void DoRainbow(int i)
    {
        int meshIndex = _currentTextTMP.textInfo.characterInfo[i].materialReferenceIndex; // On regarde son submesh (Sa bibliothèque ou sa police d'écriture)
        int vertexIndex = _currentTextTMP.textInfo.characterInfo[i].vertexIndex; // On récupère l'index de son premier vertex (Un caractère est composé de 4 vertices)

        float hue = (Time.time * 0.5f + i * 0.1f) % 1f; // On calcule la teinte en fonction du temps et de l'index du caractère
        Color color = Color.HSVToRGB(hue, 1f, hue); // On convertit la teinte en couleur RGB

        for (int j = 0; j < 4; j++)
            _currentTextTMP.textInfo.meshInfo[meshIndex].colors32[vertexIndex + j] = color; // On applique la couleur à chacun des 4 vertices du caractère

    }

    private void DoSkew(int i)
    {
        int meshIndex = _currentTextTMP.textInfo.characterInfo[i].materialReferenceIndex; // On regarde son submesh (Sa bibliothèque ou sa police d'écriture)
        int vertexIndex = _currentTextTMP.textInfo.characterInfo[i].vertexIndex; // On récupère l'index de son premier vertex (Un caractère est composé de 4 vertices)
        Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices; // Liste de tout les vertex de la chaîne de caractères
        float skew = Mathf.Sin(Time.time * 2f + i) * _skewAmount;
        vertices[vertexIndex+1] += new Vector3(skew, 0, 0);
        vertices[vertexIndex + 2] += new Vector3(skew, 0, 0);
    }



}
