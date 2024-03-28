using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObjectMake : MonoBehaviour
{
    // Env ������Ʈ Prefab
    public GameObject[] envObjPrefab;
    public GameObject[] envObj;

    void Start()
    {
        envObj = new GameObject[20];

        float posY = 1.25f;
        float posZ = -22f;

        for (int i = 0; i < envObj.Length; i++) 
        {
            // 1��° ������Ʈ
            int rnd1 = Random.Range(0, envObjPrefab.Length); // ������ �� 1�� �����ϰ� �ޱ�

            envObj[i] = Instantiate(envObjPrefab[rnd1]);
            envObj[i].transform.SetParent(transform); // ���� �� Env N�� �ڽ� ��ü�� �α�

            float posX1 = Random.Range(-3.2f, 3.2f); // X�� ���� ���� ����
            
            envObj[i].transform.localPosition = new Vector3(posX1, posY, posZ); // ���� ����
            envObj[i].transform.gameObject.SetActive(true);

            // 2��° ������Ʈ
            i++;

            rnd1 = Random.Range(0, envObjPrefab.Length); // ������ �� 1�� �����ϰ� �ޱ�

            envObj[i] = Instantiate(envObjPrefab[rnd1]);
            envObj[i].transform.SetParent(transform); // ���� �� Env N�� �ڽ� ��ü�� �α�

            float posX2 = 0;
            float posX2Padding = Random.Range(2.5f, 5f);

            if (posX1 >= 0)
            {
                posX2 = posX1 - posX2Padding;
            }
            else
            {
                posX2 = posX1 + posX2Padding;
            }            

            envObj[i].transform.localPosition = new Vector3(posX2, posY, posZ); // ���� ����
            envObj[i].transform.gameObject.SetActive(true);            

            posZ += 5f; // 10 x 2���� ����� Z�� ���� �ø���
        }
    }
}
