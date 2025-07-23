using UnityEngine;

public class CollectibleObjectAnimation : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.2f;  // Yükseklik
    [SerializeField] private float frequency = .5f;    // Hız (düşükse daha yavaş döner)
    [SerializeField] private float scaleFactor = 1.2f; // Maksimum büyüklük oranı

    private Vector3 startPosition;
    public Vector3 originalScale;


    private void OnEnable()
    {
        startPosition = transform.position;
        transform.localScale = originalScale; // Başlangıç ölçeğini ayarla

    }

    private void Update()
    {
        float sinValue = Mathf.Sin(Time.time * frequency * Mathf.PI * 2); // -1 to 1
        float height = sinValue * amplitude;

        // Yumuşak pozisyon değişimi
        transform.position = startPosition + new Vector3(0, height, 0);

        // Yumuşak büyüme: sinValue 0 → 1 olduğunda scale büyür, 0 → -1'de küçülür
        float scaleMultiplier = Mathf.Lerp(1f, scaleFactor, (sinValue + 1f) / 2f); // normalize to 0-1
        transform.localScale = originalScale * scaleMultiplier;
    }
}
