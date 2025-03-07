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
        // 초기 scale 설정
        soundwaveScale = Vector3.zero;

        // 시작하면 scale이 0이고, isIncrease가 true일 동안 커짐
        this.transform.localScale = soundwaveScale;
        isIncrease = true;
        isActive = true;

        // changeTime 경과 후, isIncrease를 false로 바꾸어 원 축소
        Invoke("SoundwaveChange", changeTime);
    }

    private void Update()
    {
        // isIncrease에 따른 원 크기 변화
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

        // 작아지며 그 크기가 눈에 보이지 않을 정도가 되면, 삭제
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
