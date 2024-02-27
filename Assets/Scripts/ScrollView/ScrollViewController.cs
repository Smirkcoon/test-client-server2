using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ScrollViewController : MonoBehaviour
{
    public Dictionary<int,ListViewItem> Items = new Dictionary<int, ListViewItem>();

    [SerializeField] private Transform content;
    [SerializeField] private ListViewItem prefab;

    public ListViewItem[] GetArray()
    {
        return Items.Values.ToArray();
    }

    public void AddItemWithData(ListViewModel[] itemsModels)
    {
        for (int i = 0; i < itemsModels.Length; i++)
        {
            int x = i;
            if (Items.Keys.Contains(itemsModels[i].id))
                continue;

            Sequence s = DOTween.Sequence();
            s.AppendInterval(i* 1.0f/10);
            s.OnComplete(() =>
            {
                if (Items.Keys.Contains(itemsModels[x].id))
                    return;
                var newItem = Instantiate(prefab, content);
                Items.Add(itemsModels[x].id, newItem);
                newItem.Setup(itemsModels[x]);
                newItem.transform.DOScale(1, 0.2f);
            });
        }

        CheckForRemove(itemsModels);
    }
    public void UpdateById(string id, ListViewModel itemsModel)
    {
        Items[TryGetInt(id)].Setup(itemsModel);
    }
    private void CheckForRemove(ListViewModel[] itemsModels)
    {
        HashSet<int> itemsModelIds = new HashSet<int>(itemsModels.Select(item => item.id));
        
        foreach (var itemModel in itemsModels)
        {
            if (!Items.ContainsKey(itemModel.id))
            {
                var newItem = Instantiate(prefab, content);
                Items.Add(itemModel.id, newItem);
                newItem.Setup(itemModel);
                newItem.transform.DOScale(1, 0.2f);
            }
        }

        List<int> keysToRemove = new List<int>();
        foreach (var key in Items.Keys)
        {
            if (!itemsModelIds.Contains(key))
                keysToRemove.Add(key);
        }
        
        foreach (var key in keysToRemove)
        {
            RemoveButtonById(key);
        }
        
    }
    public void RemoveButtonById(int id)
    {
        if (id >= 0 && Items.Keys.Contains(id))
        {
            Destroy(Items[id].gameObject);
            Items.Remove(id);
        }
        else
            Debug.LogError("Remove Button Not Correct id");
    }
    public void RemoveButtonById(string id)
    {
        RemoveButtonById(TryGetInt(id));
    }
    public int TryGetInt(string id)
    {
        int i = -1;
        int.TryParse(id, out i);
        return i;
    }
}
