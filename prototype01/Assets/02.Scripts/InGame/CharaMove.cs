using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class CharaMove : MonoBehaviour
{
    public GameMgr GM;

    public bool isMove = false;
    public bool leftIsTrue_rightIsFalse = false;
    public float moveSpeed = 5f; // �̵� �ӵ�

    public int score;
    public int highScore;
    public int coin;
    public int retry;
    public bool wing = false;
    public GameObject wingObj;

    public GameObject beforeGamePanel;
    public GameObject onGamePanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scroeTxt;
    public TextMeshProUGUI coinTxt;

    public TextMeshProUGUI scoreTextOver;
    public TextMeshProUGUI highScoreTextOver;
    
    public TextMeshProUGUI scoreTextPrefab;
    public Transform canvasTransform;
    public float speed = 1.0f;
    public float duration = 1.0f;

    public GameObject[] env;
    public int envNum = 0;

    public AudioClip[] clips; // 0 �̵�, 1 ����, 2 �߶�

    public GameObject charaModel;
    

    private void Start()
    {
        coin = PlayerPrefs.GetInt("Coin", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        retry = PlayerPrefs.GetInt("Retry", 0);

        beforeGamePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Retry : " + retry.ToString();
        beforeGamePanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "High Score : " + highScore.ToString();

        score = 0;
        scroeTxt.text = score.ToString();

        coinTxt.text = PlayerPrefs.GetInt("Coin").ToString();

        transform.GetComponent<CharaChangeInShop>().InitAll(coin);
    }

    private void OnDisable()
    {
        /*
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("Retry", retry);
        */
    }

    void Update()
    {
        if (isMove == false && GM.isGameOver == false) // ���� �����ϱ� ����
        {
            if (Input.GetKeyDown(KeyCode.Q)) // ���콺 ��ư Ŭ���� ������ ���� ����
            {
                CharaMoveStart();
            }
        }

        if (isMove == true) // �����̴� ���ȿ�
        {
            // ���� ���� üũ
            if (wing == false) // Wing�� ���� ���� ���¶��
            {
                CheckGameOver(); // ���� ����
            }            

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Q)) // ���콺 ��ư Ŭ���� ������ �̵� ���� ��ȯ
            {
                ChangingDirection();
            }

            KeepMoving();
        }

    }

    void CheckGameOver()
    {
        // ĳ���Ͱ� ���ٴڿ� �پ��ִ��� ����
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity) && hit.collider.CompareTag("ENV"))
        {
            GameOver();
        }
    }

    void ChangingDirection()
    {        
        // ���� ��ȯ        
        if (leftIsTrue_rightIsFalse)
        {
            Quaternion targetRot = Quaternion.Euler(0, 45, 0);
            charaModel.transform.rotation = targetRot;
        }
        else if (!leftIsTrue_rightIsFalse)
        {
            Quaternion targetRot = Quaternion.Euler(0, -45, 0);
            charaModel.transform.rotation = targetRot;
        }
        
        AddScore(1);

        leftIsTrue_rightIsFalse = !leftIsTrue_rightIsFalse;
        PlayingClip(clips[0]);
    }

    void KeepMoving()
    {
        if (leftIsTrue_rightIsFalse == true) // �»�� �밢�� �̵�
        {
            Vector3 mov = new Vector3(-1, 0, 1);
            transform.Translate(mov * moveSpeed * Time.deltaTime);
        }
        else if (leftIsTrue_rightIsFalse == false) // ���� �밢�� �̵�
        {
            Vector3 mov = new Vector3(1, 0, 1);
            transform.Translate(mov * moveSpeed * Time.deltaTime);
        }
    }


    void PlayingClip(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    void GameOver()
    {
        PlayingClip(clips[2]);

        // ���� ���� ó��
        GetComponent<Rigidbody>().useGravity = true;
        // Ÿ�� �ٱ����� ƨ�� ������ ����
        //GetComponent<Rigidbody>().AddForce(transform.forward * 2.5f, ForceMode.Impulse);


        isMove = false;
        GM.isGameOver = true;

        scroeTxt.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);

        scoreTextOver.text = score.ToString();
        CompareHighScore();
    }

    public void CharaMoveStart() // ���� ����
    {
        retry++;
        PlayerPrefs.SetInt("Retry", retry);

        beforeGamePanel.SetActive(false);
        onGamePanel.SetActive(true);

        AddScore(1);
        
        charaModel = transform.GetChild(0).gameObject;

        Quaternion targetRot = Quaternion.Euler(0, 45, 0);
        charaModel.transform.rotation = targetRot;        

        isMove = true; // �����̱�
    }

    void AddScore(int plus)
    {
        score += plus;
        scroeTxt.text = score.ToString();

        if ((score % 50) == 0 && score != 0) // score�� 50�� ����� ������ ������
        {            
            ChangeEnv();
        }
    }

    void AddCoin(int plus)
    {
        coin += plus;
        PlayerPrefs.SetInt("Coin", coin); // PlayerPrefs�� ����

        coinTxt.text = PlayerPrefs.GetInt("Coin").ToString();
    }

    void CompareHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }

        highScoreTextOver.text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    void ChangeEnv()
    {
        for (int i = 0; i < env.Length; i++)
        {
            env[i].transform.GetChild(envNum).gameObject.SetActive(false);
        }

        envNum++;
        if (env.Length <= envNum)
        {
            envNum = 0;
        }


        for (int i = 0; i < env.Length; i++)
        {
            env[i].transform.GetChild(envNum).gameObject.SetActive(true);            
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item_Coin"))
        {
            Destroy(other.gameObject);

            // ������ ȹ�� ó��
            AddCoin(1);
            AddScore(1);
            AddScore(1);
            StartCoroutine(ShowScoreText("+2", other.transform.position));
            PlayingClip(clips[1]);
        }
        else if (other.gameObject.CompareTag("Item_Wing"))
        {
            Destroy(other.gameObject);

            // ������ ȹ�� ó��
            wing = true;
            wingObj.SetActive(true);

            StartCoroutine(ShowScoreText("Wing!", other.transform.position));            
            StartCoroutine(WingTimeOut());
            PlayingClip(clips[1]);            
        }

        // ȭ�� �ٱ��� Boundary�� ������
        if (other.gameObject.CompareTag("Boundary"))
        {
            GameOver(); // ���� ����
        }        
    }

    IEnumerator WingTimeOut()
    {
        yield return new WaitForSeconds(5f);
        wing = false;
        wingObj.SetActive(false);
    }

    IEnumerator ShowScoreText(string text, Vector3 pos)
    {
        TextMeshProUGUI scoreText = Instantiate(scoreTextPrefab, canvasTransform);
        scoreText.text = text;
        scoreText.transform.position = Camera.main.WorldToScreenPoint(pos);

        float elapsedTime = 0f;
        Vector3 startPos = scoreText.transform.position;

        while (elapsedTime < Time.deltaTime) 
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(startPos.y, startPos.y + 50f, elapsedTime / duration);
            scoreText.transform.position = new Vector3(startPos.x, newY, startPos.z);
            yield return null;
        }

        Color startColor = scoreText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        elapsedTime = 0f;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime / duration;
            scoreText.color = Color.Lerp(startColor, endColor, elapsedTime);
            yield return null;
        }

        Destroy(scoreText.gameObject);
    }
}
