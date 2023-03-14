using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _ui;
    [SerializeField] Sprite[] _coins;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _itemPreb;

    List<Item> lstItems = new List<Item>();
    
    public void OpenOrClose()
    {
        _ui.SetActive(!_ui.activeSelf);
    }

    public void AddItem(Item _item)
    {

        foreach(Sprite sp in _coins)
        {
            if(_item._eType.ToString().Equals(sp.name))
            {
                _item._sprite = sp;
            }
            //foreach (EItemType type in Enum.GetValues(typeof(EItemType)))
            //{
            //    if (type.ToString().Equals(sp.name))
            //    {
            //        _item._sprite = sp;
            //    }

            //}
        }

        lstItems.Add(_item);
        GameObject temp = Instantiate(_itemPreb, _content);
        temp.GetComponent<ItemUI>().Init(_item);
    }
}

public enum EItemType
{
    none,
    Blue,
    Green,
    Gold,
    Brown,
    Purple,
    Max,

}
public class Item 
{
    public Sprite _sprite;
    public EItemType _eType;
    public int _Count;
}
