using UnityEngine;

public class EmergencyKit_Script : MonoBehaviour
{
    public Player_ScriptTEMP Player_ScriptTEMP; // �÷��̾� ��������

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        

        if (_collision.gameObject.CompareTag("Player"))
        {
            AddEmergencyKitNum();
            Destroy(gameObject);
        }
    }

    

    // �÷��̾ ���޻��ڿ� �浹��, �÷��̾��� ���� ���޻��� ������ ������Ű�� �Լ�
    public void AddEmergencyKitNum()
    {
        Player_ScriptTEMP.playerKitNum += 1;
    }


    
}
