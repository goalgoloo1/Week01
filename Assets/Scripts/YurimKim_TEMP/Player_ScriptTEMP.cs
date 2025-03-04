using UnityEngine;

public class Player_ScriptTEMP : MonoBehaviour
{
    public ParticleSystem healingParticle;


    public int patientTimer = 50; // ȯ���� Ÿ�̸�
    public int patientTimerMax = 100; // ȯ���� �ִ� Ÿ�̸�
    public int healScore = 100; // ȯ�� ġ��� �ö󰡴� ����
    public int playerKitNum = 0; // �÷��̾ ���� ������ ���޻��� ����
    public int score = 123; // ����(�ӽ�...)


    private void OnTriggerEnter2D(Collider2D _collision)
    {
        // ȯ�ڿ� �浹 �� HealPatient ����
        if (_collision.gameObject.CompareTag("Patient"))
        {
            HealPatient(_collision.gameObject);
        }
    }

    // �÷��̾ ���޻��� ���� ��, (ȯ�ڿ� �浹�ϸ�) ȯ�ڸ� ġ���ϰ�, ��ƼŬ�� ���� ��, ȯ�ڸ� �����ϴ� �Լ�
    public void HealPatient(GameObject patientGameObject)
    {
        if (playerKitNum > 0)
        {
            AddScore(healScore);
            Instantiate(healingParticle, transform.position, transform.rotation);
            Destroy(patientGameObject);
        }
    }

    // ������ �ø��� �Լ�
    public void AddScore(int _score)
    {
        score += _score;
    }


}


