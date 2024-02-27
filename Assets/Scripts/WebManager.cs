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

    public void StartGet()
    {
        popupController.HidePopup();
        StartCoroutine(GetRequest());
    }

    public void StartPost()
    {
        popupController.HidePopup();
        StartCoroutine(PostRequest());
    }

    public void StartDelete()
    {
        popupController.HidePopup();
        StartCoroutine(DeleteRequest());
    }

    public void StartPut()
    {
        popupController.HidePopup();
        int id = _scrollViewController.TryGetInt(popupController.GetTextInputField());
        if (id > 0)
        {
            var da = _scrollViewController.Items[id].modelData;
            var data = new
            {
                animationType = da.animationType
            };

            string jsonData = JsonConvert.SerializeObject(data);

            StartCoroutine(PutRequest(jsonData));
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
        Debug.Log("GetRequest fixUrl " + fixUrl);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            string result = uwr.downloadHandler.text;
            if (result.Contains("Not found"))
            {
                _scrollViewController.RemoveButtonById(id);
                Debug.Log("GetRequest Not found id: " + id);
                yield break;
            }

            string fixJson = "";
            if (!string.IsNullOrEmpty(id))
                fixJson = "[" + result + "]";
            else
                fixJson = result;
            
            Debug.Log("GetRequest ok ");
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

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error While Sending: " + uwr.error);
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
        Debug.Log("PutRequest: " + fixUrl + " " + json);
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
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
            Debug.LogError("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Deleted id: " + id);
        }
    }
}