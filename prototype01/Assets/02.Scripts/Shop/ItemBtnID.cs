using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBtnID : MonoBehaviour
{
    public CharaChangeInShop chara;
    public int ID;
    public int price;

    // ��ư���� Ŭ�� �̺�Ʈ
    public void LoadItem()
    {
        chara.ChngChara(ID, price);
    }
}
