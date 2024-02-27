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
    [SerializeField] private PopupController popupController;
    private void Start()
    {
        prefab.transform.DOScale(0, 0);
    }

    public ListViewItem[] GetArray()
    {
        return Items.Values.ToArray();
    }

    public void AddItemWithData(ListViewModel[] itemsModels)
    {
        StartCoroutine(CreateItemsWithDelay(itemsModels));
    }

    IEnumerator CreateItemsWithDelay(ListViewModel[] itemsModels)
    {
        foreach (ListViewModel itemModel in itemsModels)
        {
            if (Items.ContainsKey(itemModel.id))
                continue;

            var newItem = Instantiate(prefab, content);
            Items.Add(itemModel.id, newItem);
            newItem.Setup(itemModel);
            newItem.transform.DOScale(1, 0.2f);
            newItem.btn.onClick.AddListener(() =>
            {
                if (popupController.Opened)
                    popupController.idInputField.text = newItem.modelData.id.ToString();
            });
            yield return new WaitForSeconds(0.1f); // Задержка между созданием элементов
        }

        // Проверяем на удаление объектов
        if (itemsModels.Length > 1)
            CheckForRemove(itemsModels);
    }
    public void UpdateById(string id, ListViewModel itemModel)
    {
        if(Items.ContainsKey(TryGetInt(id)))
            Items[TryGetInt(id)].Setup(itemModel);
        else
        {
            AddItemWithData(new ListViewModel[] { itemModel });
        }
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
            Items[id].transform.DOScale(0, 0.2f)
                .OnComplete(() =>
                {
                    Items[id].btn.onClick.RemoveAllListeners();
                    Destroy(Items[id].gameObject);
                    Items.Remove(id);
                });
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
