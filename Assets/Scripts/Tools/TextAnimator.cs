using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum TextAnimationType
{
    None, Wave, Shake, Pulse, HeartPulse, Bounce, Rainbow, Skew, Orbit, SharpRotate, WTF
}

public class TextAnimator : MonoBehaviour
{
    public TextAnimationType CurrentTextAnimation = TextAnimationType.Wave;

    public float _waveFrequency = 0.5f;
    public float _waveAmplitude = 0.05f;
    public float _waveSpeed = 1f;

    public float _shakeAmplitude = 0.5f;
    public float _skewAmount = 1f;


    TextMeshProUGUI _currentTextTMP;
    Mesh _currentTextMesh;

    float[] _sharpRotationAngles;
    float _currentTimerSharpRotation = 0f;
    //float _currentTimerSharpRotation;
    float _animationTimer = 1f;

    // À initialiser quand le texte change


    private void Awake() => _currentTextTMP = GetComponent<TextMeshProUGUI>();

    // Update is called once per frame
    void Update()
    {
        if (_animationTimer > 0f)
        {
            if (CurrentTextAnimation == TextAnimationType.SharpRotate)
            {
                _currentTimerSharpRotation += Time.deltaTime;
                if (_currentTimerSharpRotation >= 1f)
                {
                    // Nouveau angle aléatoire pour chaque lettre
                    for (int k = 0; k < _sharpRotationAngles.Length; k++)
                        _sharpRotationAngles[k] = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);

                    _currentTimerSharpRotation = 0f;
                }
            }


            _currentTextTMP.ForceMeshUpdate();
            _currentTextMesh = _currentTextTMP.mesh;

            var nbChar = _currentTextTMP.textInfo.characterCount;


            for (int i = 0; i < nbChar; i++) // Pour chaque caractère du texte
            {
                var charInfo = _currentTextTMP.textInfo.characterInfo[i]; // On récupère les infos du caractère
                if (!charInfo.isVisible) continue;

                LaunchAnimation(CurrentTextAnimation, i);
            }


            _currentTextTMP.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices | TMP_VertexDataUpdateFlags.Colors32); // On applique visuellement les modifications précédentes sur les vertex
        }
    }


    private void InitSharpRotation()
    {
        _sharpRotationAngles = new float[_currentTextTMP.textInfo.characterCount];
    }

    private void LaunchAnimation(TextAnimationType animationType, int i)
    {
        
        switch (animationType)
        {
            case TextAnimationType.Wave:
                DoWave();
                break;
            case TextAnimationType.Shake:
                DoShake();
                break;
            case TextAnimationType.Orbit:
                DoOrbit();
                break;
            case TextAnimationType.Pulse:
                DoPulse(i);
                break;
            case TextAnimationType.HeartPulse:
                DoHeartPulse(i);
                break;
            case TextAnimationType.Bounce:
                DoBounce();
                break;
            case TextAnimationType.Rainbow:
                DoRainbow(i);
                break;
            case TextAnimationType.Skew:
                DoSkew(i);
                break;
            case TextAnimationType.SharpRotate:
                DoSharpRotate(i);
                break;
            case TextAnimationType.WTF:
                DoWTF();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
        }
    }

    private void DoOrbit()
    {
        int nbChar = _currentTextTMP.textInfo.characterCount;
        for (int i = 0; i < nbChar; i++)
        {
            var charInfo = _currentTextTMP.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices;

            float angle = Time.time * _waveSpeed + i * _waveFrequency;
            float x = Mathf.Cos(angle) * _waveAmplitude;
            float y = Mathf.Sin(angle) * _waveAmplitude;
            Vector3 offset = new Vector3(x, y, 0);

            vertices[vertexIndex] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }
    }


    private void DoSharpRotate(int i)
    {
        float xV, yV;

        float cooldown = 1f; // Nombre de secondes entre chaque rotation

        int startIndex = _currentTextTMP.textInfo.characterInfo[i].vertexIndex;
        Vector3 center = (_currentTextTMP.textInfo.meshInfo[0].vertices[startIndex] +
                          _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + 2]) / 2;

        float angle = _currentTimerSharpRotation;
        for (int j = 0; j < 4; j++)
        {
            xV = _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + j].x - center.x;
            yV = _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + j].y - center.y;

            float rotatedX = xV * Mathf.Cos(angle) - yV * Mathf.Sin(angle);
            float rotatedY = xV * Mathf.Sin(angle) + yV * Mathf.Cos(angle);

            _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + j] = new Vector3(center.x + rotatedX, center.y + rotatedY, 0);
        }
        //_currentTimerSharpRotation = 0f;


    }

    private void DoWave()
    {
        int nbChar = _currentTextTMP.textInfo.characterCount;
        for (int i = 0; i < nbChar; i++)
        {
            var charInfo = _currentTextTMP.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices;
            float wave = Mathf.Sin(Time.time * _waveSpeed + i * _waveFrequency) * _waveAmplitude;
            Vector3 offset = new Vector3(0, wave, 0);
            vertices[vertexIndex] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }
    }

    private void DoShake()
    {
        int nbChar = _currentTextTMP.textInfo.characterCount;
        for (int i = 0; i < nbChar; i++)
        {
            var charInfo = _currentTextTMP.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices;
            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-_shakeAmplitude, _shakeAmplitude),
                UnityEngine.Random.Range(-_shakeAmplitude, _shakeAmplitude),
                0);
            vertices[vertexIndex] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }
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
    private void DoHeartPulse(int i)
    {
        int startIndex = _currentTextTMP.textInfo.characterInfo[i].vertexIndex;
        Vector3 center = (_currentTextTMP.textInfo.meshInfo[0].vertices[startIndex] +
                          _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + 2]) / 2; // On calcule le centre du caractère
        float scale = 1f + Mathf.Sin(Time.time * 2f) * 0.2f;

        for (int j = 0; j < 4; j++)
            _currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + j] = center + (_currentTextTMP.textInfo.meshInfo[0].vertices[startIndex + j] - center) * scale;

    }

    private void DoBounce()
    {
        int nbChar = _currentTextTMP.textInfo.characterCount;
        for (int i = 0; i < nbChar; i++)
        {
            var charInfo = _currentTextTMP.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices;

            float t = Time.time * _waveSpeed + i * _waveFrequency;
            float sin = Mathf.Abs(Mathf.Sin(t));

            Vector3 offset = new Vector3(
                0,
                Mathf.Pow(sin, 0.5f) * _waveAmplitude,
                0);

            vertices[vertexIndex] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }
    }


    private void DoRainbow(int i)
    {
        int meshIndex = _currentTextTMP.textInfo.characterInfo[i].materialReferenceIndex; // On regarde son submesh (Sa bibliothèque ou sa police d'écriture)
        int vertexIndex = _currentTextTMP.textInfo.characterInfo[i].vertexIndex; // On récupère l'index de son premier vertex (Un caractère est composé de 4 vertices)

        float hue = (Time.time * 0.5f + i * 0.1f) % 1f; // On calcule la teinte en fonction du temps et de l'index du caractère
        Color color = Color.HSVToRGB(hue, 1f, 1f); // On convertit la teinte en couleur RGB

        for (int j = 0; j < 4; j++)
            _currentTextTMP.textInfo.meshInfo[meshIndex].colors32[vertexIndex + j] = color; // On applique la couleur à chacun des 4 vertices du caractère
    }

    private void DoSkew(int i)
    {
        int meshIndex = _currentTextTMP.textInfo.characterInfo[i].materialReferenceIndex; // On regarde son submesh (Sa bibliothèque ou sa police d'écriture)
        int vertexIndex = _currentTextTMP.textInfo.characterInfo[i].vertexIndex; // On récupère l'index de son premier vertex (Un caractère est composé de 4 vertices)
        Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices; // Liste de tout les vertex de la chaîne de caractères
        float skew = Mathf.Sin(Time.time * 2f + i) * _skewAmount;
        vertices[vertexIndex + 1] += new Vector3(skew, 0, 0);
        vertices[vertexIndex + 2] += new Vector3(skew, 0, 0);
    }

    private void DoWTF()
    {
        int nbChar = _currentTextTMP.textInfo.characterCount;
        for (int i = 0; i < nbChar; i++)
        {
            var charInfo = _currentTextTMP.textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Vector3[] vertices = _currentTextTMP.textInfo.meshInfo[meshIndex].vertices;

            float angle = Time.time * _waveSpeed * _waveFrequency;
            float x = Mathf.Cos(angle) * _waveAmplitude;
            float y = Mathf.Sin(angle) * _waveAmplitude;
            Vector3 offset = new Vector3(x, y, 0);

            int r = UnityEngine.Random.Range(0, 4);

            vertices[vertexIndex + r] += offset;
        }
    }



}
