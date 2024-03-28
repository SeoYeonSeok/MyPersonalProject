using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaChangeInShop : MonoBehaviour
{
    public int itemID;

    // ĳ���� �� (ĳ������ �ڽ�)
    public GameObject chara;

    // ĳ���� ���� �����յ�
    public GameObject[] charaPrefabs;

    // ĳ���� ���� ������ ���ѿ� ���� Key ����
    public int sample0Key;
    public int sample1Key;
    public int sample2Key;
    public int sample3Key;
    public int sample4Key;
    public int sample5Key;
    public int sample6Key;
    public int sample7Key;
    public int sample8Key;
    public int sample9Key;

    public ShopButtonColor shopBtns;
    public ShopMgr shopMgr;


    public void InitAll(int currentCoin)
    {
        chara = transform.GetChild(0).gameObject; // �ڽ� ��ü �ʱ�ȭ

        shopMgr.ChngShopCoinText(currentCoin); // ���� ȭ���� ���� �ؽ�Ʈ �ʱ�ȭ

        InitKey(); // Ű �ʱ�ȭ
        itemID = PlayerPrefs.GetInt("ItemID", 0); // ���� ItemID�� Ű �� �޾ƿ���
        ChngCharaStart(itemID); // ���۰� ���ÿ� ItemID�� Ű ���� �ش��ϴ� ĳ���� ������ ���� �Լ� ����
    }

    private void InitKey()
    {
        sample0Key = PlayerPrefs.GetInt("S0Key", 1); // �⺻ �������� Key�� 1�� ������ �ʱ�ȭ
        sample1Key = PlayerPrefs.GetInt("S1Key", 0);
        sample2Key = PlayerPrefs.GetInt("S2Key", 0);
        sample3Key = PlayerPrefs.GetInt("S3Key", 0);
        sample4Key = PlayerPrefs.GetInt("S4Key", 0);
        sample5Key = PlayerPrefs.GetInt("S5Key", 0);
        sample6Key = PlayerPrefs.GetInt("S6Key", 0);
        sample7Key = PlayerPrefs.GetInt("S7Key", 0);
        sample8Key = PlayerPrefs.GetInt("S8Key", 0);
        sample9Key = PlayerPrefs.GetInt("S9Key", 0);
    }

    
    public void ChngCharaStart(int itemID)
    {
        if (ReturnKey(itemID) == true) // Ű�� �ִ��� ������ �˻�, ������ (������ ���¸�) �Ʒ� �ڵ� ����
        {
            ChngPrefabs(charaPrefabs[itemID]);
            PlayerPrefs.SetInt("ItemID", itemID);
            shopBtns.AllBtnColorCng();
            shopBtns.SelectBtnColorCng(PlayerPrefs.GetInt("ItemID"));
        }
    }

    public void ChngChara(int itemID, int price)
    {
        if (ReturnKey(itemID) == true) // Ű�� �ִ��� ������ �˻�, ������ (������ ���¸�) �Ʒ� �ڵ� ����
        {
            ChngPrefabs(charaPrefabs[itemID]);
            PlayerPrefs.SetInt("ItemID", itemID);
            shopBtns.AllBtnColorCng();
            shopBtns.SelectBtnColorCng(PlayerPrefs.GetInt("ItemID"));
        }
        else // Key�� ������ ���� ���¶�� ���ΰ� ���� �˻� �Լ� ����
        {
            UnlockKey(price, itemID);
        }
    }

    public void ChngPrefabs(GameObject prefab)
    {
        Destroy(chara.transform.GetChild(0).gameObject); // �ڽ� ��ü �ı��ϰ�

        GameObject newChild = Instantiate(prefab, transform);
        newChild.transform.SetParent(chara.transform);
    }
    
    
    private bool ReturnKey(int id)
    {
        if (id == 0)
        {            
            if (PlayerPrefs.GetInt("S0Key") == 1) return true;
            else return false;
        }
        else if (id == 1)
        {
            if (PlayerPrefs.GetInt("S1Key") == 1) return true;
            else return false;
        }
        else if (id == 2)
        {
            if (PlayerPrefs.GetInt("S2Key") == 1) return true;
            else return false;
        }
        else if (id == 3) 
        {
            if (PlayerPrefs.GetInt("S3Key") == 1) return true;
            else return false;
        }
        else if (id == 4)
        {
            if (PlayerPrefs.GetInt("S4Key") == 1) return true;
            else return false;
        }
        else if (id == 5)
        {
            if (PlayerPrefs.GetInt("S5Key") == 1) return true;
            else return false;
        }
        else if (id == 6)
        {
            if (PlayerPrefs.GetInt("S6Key") == 1) return true;
            else return false;
        }
        else if (id == 7)
        {
            if (PlayerPrefs.GetInt("S7Key") == 1) return true;
            else return false;
        }
        else if (id == 8)
        {
            if (PlayerPrefs.GetInt("S8Key") == 1) return true;
            else return false;
        }
        else if (id == 9)
        {
            if (PlayerPrefs.GetInt("S9Key") == 1) return true;
            else return false;
        }

        return false;
    }

    private void UnlockKey(int price, int id)
    {
        CharaMove chara = transform.GetComponent<CharaMove>();
        int curCoin = PlayerPrefs.GetInt("Coin");

        if (curCoin >= price) // ���� CharaMove ������Ʈ ���� coin�� ����(100) ��
        {
            curCoin -= price;
            
            shopMgr.ChngShopCoinText(curCoin);
            chara.coinTxt.text = curCoin.ToString();

            PlayerPrefs.SetInt("Coin", curCoin);

            if (id == 1) PlayerPrefs.SetInt("S1Key", 1);
            else if (id == 2) PlayerPrefs.SetInt("S2Key", 1);
            else if (id == 3) PlayerPrefs.SetInt("S3Key", 1);
            else if (id == 4) PlayerPrefs.SetInt("S4Key", 1);
            else if (id == 5) PlayerPrefs.SetInt("S5Key", 1);
            else if (id == 6) PlayerPrefs.SetInt("S6Key", 1);
            else if (id == 7) PlayerPrefs.SetInt("S7Key", 1);
            else if (id == 8) PlayerPrefs.SetInt("S8Key", 1);
            else if (id == 9) PlayerPrefs.SetInt("S9Key", 1);
        }
    }

    public void DebugKey()
    {
        PlayerPrefs.SetInt("S1Key", 0);
        PlayerPrefs.SetInt("S2Key", 0);
        PlayerPrefs.SetInt("S3Key", 0);
        PlayerPrefs.SetInt("S4Key", 0);
        PlayerPrefs.SetInt("S5Key", 0);
        PlayerPrefs.SetInt("S6Key", 0);
        PlayerPrefs.SetInt("S7Key", 0);
        PlayerPrefs.SetInt("S8Key", 0);
        PlayerPrefs.SetInt("S9Key", 0);
    }

    public void DebugKey2()
    {
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 100);

        CharaMove charaM = transform.GetComponent<CharaMove>();
        charaM.coinTxt.text = PlayerPrefs.GetInt("Coin").ToString();

        shopMgr.ChngShopCoinText(PlayerPrefs.GetInt("Coin"));
    }
}
