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

    // 초기 경로 구축 목적 타일 생성 수
    public int remainingSecondTiles = 20;

    // 마지막으로 생성된 타일
    public GameObject lastTile;

    // 타일 생성 위치 1~7
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

            if (tileNum <= 0) // 좌측으로 넘어 버리면
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
            // 좌측으로 직진하는 라인 생성
            tileL.transform.GetChild(0).gameObject.SetActive(true);
            tileL.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (lastTile.GetComponent<TileID>().ID == 0 && tileID == 1)
        {
            // 우측으로 꺾는 라인 생성
            tileL.transform.GetChild(0).gameObject.SetActive(false);
            tileL.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (lastTile.GetComponent<TileID>().ID == 1 && tileID == 1)
        {
            // 우측으로 직진하는 라인 생성
            tileL.transform.GetChild(0).gameObject.SetActive(true);
            tileL.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (lastTile.GetComponent<TileID>().ID == 1 && tileID == 0)
        {
            // 좌측으로 꺾는 라인 생성
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
        // 움직여야 할 위치
        Vector3 movPos = new Vector3(35f, cam.transform.position.y, cam.transform.position.z);

        StartCoroutine(MoveCam(movPos));

        // 패널 UI 처리
        beforeGamePanel.gameObject.SetActive(false);
        rulePanel.gameObject.SetActive(true);
    }

    public void PressShopBtn()
    {
        // 움직여야 할 위치
        Vector3 movPos = new Vector3(-35f, cam.transform.position.y, cam.transform.position.z);

        StartCoroutine(MoveCam(movPos));

        // 패널 UI 처리
        beforeGamePanel.gameObject.SetActive(false);
        shopPanel.gameObject.SetActive(true);        
    }

    public void ReturnMainBtn()
    {
        // 움직여야 할 위치
        Vector3 movPos = new Vector3(0f, cam.transform.position.y, cam.transform.position.z);

        StartCoroutine(MoveCam(movPos));

        // 현재 생성된 패널 중 유일하게 SetActive 되어 있는 패널 찾아서 닫기
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
        float movementDuration = 0.5f; // 카메라 이동 속도 (메인 <-> 상점 / ? 화면)

        Vector3 startingPos = cam.transform.position;            

        while (elapsedTime < movementDuration)
        {
            cam.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / movementDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = targetPosition; // 이동 완료 후 보정
    }
}
