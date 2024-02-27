using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager : MonoBehaviour
{
    private readonly string urlServer = "https://65d6faa927d9a3bc1d79cece.mockapi.io/v1/buttons";
    [SerializeField] private ScrollViewController _scrollViewController;
    [SerializeField] private PopupController popupController;
    [SerializeField] private FlyText prefabFlyText;
    [SerializeField] private Transform posFlyText;
    public void StartGet()
    {
        StartCoroutine(GetRequest());
    }

    public void StartPost()
    {
        popupController.HidePopup();
        StartCoroutine(PostRequest());
    }

    public void StartDelete()
    {
        StartCoroutine(DeleteRequest());
    }

    public void StartPut()
    {
        int id = _scrollViewController.TryGetInt(popupController.GetTextInputField());
        if (id > 0 && _scrollViewController.Items.ContainsKey(id))
        {
            var da = _scrollViewController.Items[id]?.modelData;
            var data = new
            {
                animationType = da.animationType
            };

            string jsonData = JsonConvert.SerializeObject(data);

            StartCoroutine(PutRequest(jsonData));
        }
        else
        {
            CallFlyText("PutRequest Not found id: " + id);
        }
    }

    IEnumerator GetRequest()
    {
        string id = popupController.GetTextInputField();
        string fixUrl = urlServer;
        if (!string.IsNullOrEmpty(id))
        {
            fixUrl = urlServer + "/" + id;
        }
        UnityWebRequest uwr = UnityWebRequest.Get(fixUrl);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            CallFlyText("ConnectionError " + uwr.error);
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            string result = uwr.downloadHandler.text;
            if (result.Contains("Not found"))
            {
                _scrollViewController.RemoveButtonById(id);
                CallFlyText("GetRequest Not found id: " + id);
                yield break;
            }
            
            string fixJson = "";
            if (!string.IsNullOrEmpty(id))
            {
                fixJson = "[" + result + "]";
                CallFlyText("GetRequest ok id: " + id);
            }
            else
            {
                fixJson = result;
                CallFlyText("GetRequest All ok");
            }

            ListViewModel[] listViewModels = JsonConvert.DeserializeObject<ListViewModel[]>(fixJson);
            if (!string.IsNullOrEmpty(id))
                _scrollViewController.UpdateById(id, listViewModels?[0]);
            else
                _scrollViewController.AddItemWithData(listViewModels);
        }
    }

    IEnumerator PostRequest()
    {
        var uwr = new UnityWebRequest(urlServer, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("");
        uwr.uploadHandler = (UploadHandler) new UploadHandlerRaw(jsonToSend);
        uwr.SetRequestHeader("Content-Type", "application/json");
        
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            CallFlyText("ConnectionError " + uwr.error);
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            CallFlyText("PostRequest ok");
        }
    }

    IEnumerator PutRequest(string json)
    {
        string id = popupController.GetTextInputField();
        string fixUrl = urlServer;
        if (!string.IsNullOrEmpty(id))
        {
            fixUrl = urlServer + "/" + id;
        }

        UnityWebRequest uwr = UnityWebRequest.Put(fixUrl, json);
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            CallFlyText("ConnectionError " + uwr.error);
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            string result = uwr.downloadHandler.text;
            if (result.Contains("Not found"))
            {
                CallFlyText("PutRequest Not found id: " + id);
            }
            else
                CallFlyText("PutRequest ok " + id);
        }
    }

    IEnumerator DeleteRequest()
    {
        string id = popupController.GetTextInputField();
        string fixUrl = urlServer;
        if (!string.IsNullOrEmpty(id))
        {
            fixUrl = urlServer + "/" + id;
        }
        
        UnityWebRequest uwr = UnityWebRequest.Delete(fixUrl);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            CallFlyText("ConnectionError " + uwr.error);
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            CallFlyText("Deleted id: " + id);
            //Debug.Log("Deleted id: " + id);
        }
    }

    public void CallFlyText(string text)
    {
        Instantiate(prefabFlyText, posFlyText.position, Quaternion.identity, posFlyText).Fly(text);
    }
}