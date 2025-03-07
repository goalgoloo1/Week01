using UnityEngine;

public class Soundwave_Script : MonoBehaviour
{
    public float increaseSpeed = 0.5f;
    public float decreaseSpeed = 0.1f;
    public float changeTime = 1f;
    public bool isIncrease = false;
    public bool isActive = false;
    Vector3 soundwaveScale;

    private void Start()
    {
        // �ʱ� scale ����
        soundwaveScale = Vector3.zero;

        // �����ϸ� scale�� 0�̰�, isIncrease�� true�� ���� Ŀ��
        this.transform.localScale = soundwaveScale;
        isIncrease = true;
        isActive = true;

        // changeTime ��� ��, isIncrease�� false�� �ٲپ� �� ���
        Invoke("SoundwaveChange", changeTime);
    }

    private void Update()
    {
        // isIncrease�� ���� �� ũ�� ��ȭ
        if (isIncrease)
        {
            this.transform.localScale = soundwaveScale;
            soundwaveScale = soundwaveScale + new Vector3(increaseSpeed * Time.deltaTime, increaseSpeed * Time.deltaTime, increaseSpeed * Time.deltaTime);
        }
        else
        {
            this.transform.localScale = soundwaveScale;
            soundwaveScale = soundwaveScale + new Vector3(-decreaseSpeed * Time.deltaTime, -decreaseSpeed * Time.deltaTime, -decreaseSpeed * Time.deltaTime);
        }

        // �۾����� �� ũ�Ⱑ ���� ������ ���� ������ �Ǹ�, ����
        if (isActive && soundwaveScale.magnitude < 0.01f)
        {
            DeleteSoundwave();
        }

    }

    public void SoundwaveChange()
    {
        isIncrease = false;
    } 

    public void DeleteSoundwave()
    {
        Destroy(gameObject);
    }
}
