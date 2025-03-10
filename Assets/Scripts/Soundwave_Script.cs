using UnityEngine;

public class Soundwave_Script : MonoBehaviour
{
    [Header("Soundwave")]
    public float increaseSpeed = 0.5f;
    public float decreaseSpeed = 0.1f;
    public float changeTime = 1f;
    public bool isIncreasing = true; // ���¸� ��Ȯ�ϰ� ����
    private Vector3 soundwaveScale;

    [Header("Player Transform")]
    public Transform playerTransform;

    private void Start()
    {
        // �ʱ� scale ����
        soundwaveScale = Vector3.zero;
        this.transform.localScale = soundwaveScale;
        Invoke("StartDecrease", changeTime); // ���� �ð� �� �پ��� ����
    }

    private void Update()
    {
        // ���� ũ�� ��ȭ
        if (isIncreasing)
        {
            soundwaveScale += new Vector3(increaseSpeed * Time.deltaTime, increaseSpeed * Time.deltaTime, increaseSpeed * Time.deltaTime);
        }
        else
        {
            soundwaveScale -= new Vector3(decreaseSpeed * Time.deltaTime, decreaseSpeed * Time.deltaTime, decreaseSpeed * Time.deltaTime);
        }

        // �������� �ʹ� �۾����� �ʵ��� ����
        if (soundwaveScale.x < 0.1f && !isIncreasing)
        {
            soundwaveScale = Vector3.zero; // 0���� �ʱ�ȭ
            DeleteSoundwave();
        }

        // ������ ����
        this.transform.localScale = soundwaveScale;
    }

    void StartDecrease()
    {
        isIncreasing = false; // �پ��� ���·� ��ȯ
    }

    public void DeleteSoundwave()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� ���Ŀ� ����� ��
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("�鸰�ٵ鸰��~!");

            // �� ���� ������ ��, �ش� ���� Checking ���·� ����
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.currentState = Enemy.EnemyState.Checking;
                enemy.MoveToPlayerPosition();
            }
        }
    }

}
