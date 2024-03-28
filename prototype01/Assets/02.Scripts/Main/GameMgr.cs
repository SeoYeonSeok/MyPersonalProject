using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameMgr : MonoBehaviour
{
    public bool isGameOver = false;
    public GameObject cam;

    public GameObject canv;
    public GameObject beforeGamePanel;
    public GameObject shopPanel;
    public GameObject rulePanel;


    public GameObject[] tilePrefabs;
    public GameObject tileMgr;

    public GameObject[] itemPrefabs;

    // �ʱ� ��� ���� ���� Ÿ�� ���� ��
    public int remainingSecondTiles = 20;

    // ���������� ������ Ÿ��
    public GameObject lastTile;

    // Ÿ�� ���� ��ġ 1~7
    public int tileNum;


    void Start()
    {
        tileNum = 5;

        for (int i = 0; i < remainingSecondTiles; i++)
        {
            TileMake();
        }
    }

    public void TileMake()
    {
        GameObject tile;

        int rnd = Random.Range(0, 2);

        GameObject tileLine = lastTile.transform.GetChild(5).gameObject;

        if (rnd == 0) // Left Spawn
        {
            tileNum--;

            if (tileNum <= 0) // �������� �Ѿ� ������
            {
                tileNum = 2;
                tile = Instantiate(tilePrefabs[1]); // Front Spawn Change
                tile.transform.parent = tileMgr.transform;
                tile.transform.position = lastTile.transform.GetChild(1).transform.position;
                tile.transform.localRotation = Quaternion.identity;
                tile.transform.Rotate(new Vector3(0, -45, 0));
            }
            else
            {
                tile = Instantiate(tilePrefabs[0]);
                tile.transform.parent = tileMgr.transform;
                tile.transform.position = lastTile.transform.GetChild(0).transform.position;
                tile.transform.localRotation = Quaternion.identity;
                tile.transform.Rotate(new Vector3(0, -45, 0));
            }

            DrawTileLine(tileLine, tile.GetComponent<TileID>().ID);

            lastTile = tile;

            ItemMake();
        }
        else if (rnd == 1) // Front Spawn
        {
            tileNum++;
            
            if (tileNum >= 8)
            {
                tileNum = 6;
                tile = Instantiate(tilePrefabs[0]); // Left Spawn Change
                tile.transform.parent = tileMgr.transform;
                tile.transform.position = lastTile.transform.GetChild(0).transform.position;
                tile.transform.localRotation = Quaternion.identity;
                tile.transform.Rotate(new Vector3(0, -45, 0));
            }
            else
            {                
                tile = Instantiate(tilePrefabs[1]);
                tile.transform.parent = tileMgr.transform;
                tile.transform.position = lastTile.transform.GetChild(1).transform.position;
                tile.transform.localRotation = Quaternion.identity;
                tile.transform.Rotate(new Vector3(0, -45, 0));                
            }

            DrawTileLine(tileLine, tile.GetComponent<TileID>().ID);

            lastTile = tile;

            ItemMake();
        }        
    }

    public void ItemMake()
    {
        // Item Spawn Process
        int rnd2 = Random.Range(0, 10);
        if (rnd2 >= 8)
        {
            GameObject item = null;

            int rnd3 = Random.Range(0, 10);

            if (rnd3 >= 2)
            {
                item = Instantiate(itemPrefabs[0]);
            }
            else
            {
                item = Instantiate(itemPrefabs[1]);
            }
            item.transform.position = lastTile.transform.GetChild(2).transform.position;
            item.transform.localRotation = Quaternion.identity;
        }
    }

    public void DrawTileLine(GameObject tileL, int tileID)
    {        
        if (lastTile.GetComponent<TileID>().ID == 0 && tileID == 0)
        {
            // �������� �����ϴ� ���� ����
            tileL.transform.GetChild(0).gameObject.SetActive(true);
            tileL.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (lastTile.GetComponent<TileID>().ID == 0 && tileID == 1)
        {
            // �������� ���� ���� ����
            tileL.transform.GetChild(0).gameObject.SetActive(false);
            tileL.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (lastTile.GetComponent<TileID>().ID == 1 && tileID == 1)
        {
            // �������� �����ϴ� ���� ����
            tileL.transform.GetChild(0).gameObject.SetActive(true);
            tileL.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (lastTile.GetComponent<TileID>().ID == 1 && tileID == 0)
        {
            // �������� ���� ���� ����
            tileL.transform.GetChild(0).gameObject.SetActive(false);
            tileL.transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    public void SceneReload()
    {
        SceneManager.LoadScene(0);
    }

    public void PressRuleBtn()
    {
        // �������� �� ��ġ
        Vector3 movPos = new Vector3(35f, cam.transform.position.y, cam.transform.position.z);

        StartCoroutine(MoveCam(movPos));

        // �г� UI ó��
        beforeGamePanel.gameObject.SetActive(false);
        rulePanel.gameObject.SetActive(true);
    }

    public void PressShopBtn()
    {
        // �������� �� ��ġ
        Vector3 movPos = new Vector3(-35f, cam.transform.position.y, cam.transform.position.z);

        StartCoroutine(MoveCam(movPos));

        // �г� UI ó��
        beforeGamePanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(true);        
    }

    public void ReturnMainBtn()
    {
        // �������� �� ��ġ
        Vector3 movPos = new Vector3(0f, cam.transform.position.y, cam.transform.position.z);

        StartCoroutine(MoveCam(movPos));

        // ���� ������ �г� �� �����ϰ� SetActive �Ǿ� �ִ� �г� ã�Ƽ� �ݱ�
        GameObject activeChild = null;        

        foreach (Transform child in canv.transform)
        {
            if (child.gameObject.activeSelf)
            {
                activeChild = child.gameObject;
                break;
            }
        }

        activeChild.SetActive(false);
        beforeGamePanel.SetActive(true);
    }

    IEnumerator MoveCam(Vector3 targetPosition)
    {
        float elapsedTime = 0;
        float movementDuration = 0.5f; // ī�޶� �̵� �ӵ� (���� <-> ���� / ? ȭ��)

        Vector3 startingPos = cam.transform.position;            

        while (elapsedTime < movementDuration)
        {
            cam.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / movementDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = targetPosition; // �̵� �Ϸ� �� ����
    }
}
