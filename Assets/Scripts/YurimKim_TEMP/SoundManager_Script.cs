using UnityEngine;

public class SoundManager_Script : MonoBehaviour
{
    public static SoundManager_Script Instance;
    private AudioSource audioSource;

    // ����� Audioclip ����Ʈ
    public AudioClip sound1; 

    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �ı����� �ʰ�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // ����Ŵ��� ������ҽ� �ҷ�����
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySFX_PlayerShoot()
    {
        audioSource.PlayOneShot(sound1);
    }
}
