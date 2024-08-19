using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

public class OpenAIChatAEPicture : MonoBehaviour
{
    public InputField userInputField; // 输入框的引用
    public Button submitButton;       // 提交按钮的引用
    public Button screenshotButton;   // 截图按钮的引用
    public Text responseText;         // 显示响应的文本组件引用

    private string apiKey = "sk-svcacct-2tVv0DzyCs9DJmJxyuN_FK8Au0FbHZNgzIOLOZm7jtTfJIbmClcFT3BlbkFJ-LdZb62HsPfng06ZdregyNTZcrJ35-dMH2gBIpfuANnt6wICiGQA"; // 替换为你的 API 密钥
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    public class ChatCompletionRequest
    {
        public string model;
        public List<Message> messages;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }

    [System.Serializable]
    public class ChatCompletionResponse
    {
        public List<Choice> choices;
    }

    void Start()
    {
        // 添加按钮点击事件监听器
        submitButton.onClick.AddListener(OnSubmit);
        screenshotButton.onClick.AddListener(OnScreenshot); // 添加截图按钮的点击事件监听器
    }

    void OnSubmit()
    {
        string userMessage = userInputField.text;
        if (!string.IsNullOrEmpty(userMessage))
        {
            StartCoroutine(SendChatMessage(userMessage));
        }
    }

    public void OnScreenshot()
    {
        StartCoroutine(CaptureAndUploadScreenshot());
    }

    IEnumerator SendChatMessage(string userMessage)
    {
        // 默认提示词
        string defaultPrompt = "How do you find the aesthetic expression of this building?";

        // 将默认提示词加到用户输入之前
        string fullMessage = defaultPrompt + " " + userMessage;

        // 创建聊天请求
        List<Message> messages = new List<Message>
        {
            new Message { role = "system", content = "You are a helpful architectural aesthetics critic." },
            new Message { role = "user", content = fullMessage }
        };

        ChatCompletionRequest chatRequest = new ChatCompletionRequest
        {
            model = "gpt-4",
            messages = messages
        };

        string jsonBody = JsonConvert.SerializeObject(chatRequest);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);
                
            // 设置超时时间为 60 秒
            request.timeout = 60;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("错误: " + request.error);
            }
            else
            {
                // 解析响应
                string jsonResponse = request.downloadHandler.text;
                ChatCompletionResponse chatResponse = JsonConvert.DeserializeObject<ChatCompletionResponse>(jsonResponse);

                // 显示助手响应
                string assistantResponse = chatResponse.choices[0].message.content;
                responseText.text = "助手: " + assistantResponse;
            }
        }
    }

    IEnumerator CaptureAndUploadScreenshot()
    {
        // 等待截图完成
        yield return new WaitForEndOfFrame();

        // 截取屏幕并保存为临时文件
        string screenshotPath = Application.persistentDataPath + "/screenshot.png";
        ScreenCapture.CaptureScreenshot(screenshotPath);

        // 等待截图文件写入
        yield return new WaitUntil(() => File.Exists(screenshotPath));

        // 读取截图文件并转换为字节数组
        byte[] imageData = File.ReadAllBytes(screenshotPath);

        Debug.Log("截图读取成功");

        // 上传截图并显示在对话框中
        StartCoroutine(UploadScreenshot(imageData));
    }

    IEnumerator UploadScreenshot(byte[] imageData)
    {
        string userMessage = "上传了一个截图。";

        // 创建聊天请求
        List<Message> messages = new List<Message>
        {
            new Message { role = "system", content = "You are a helpful architectural aesthetics critic." },
            new Message { role = "user", content = userMessage }
        };

        ChatCompletionRequest chatRequest = new ChatCompletionRequest
        {
            model = "gpt-4",
            messages = messages
        };

        string jsonBody = JsonConvert.SerializeObject(chatRequest);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            // 处理上传数据
            request.uploadHandler = new UploadHandlerRaw(imageData);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("错误: " + request.error);
            }
            else
            {
                // 解析响应
                string jsonResponse = request.downloadHandler.text;
                ChatCompletionResponse chatResponse = JsonConvert.DeserializeObject<ChatCompletionResponse>(jsonResponse);

                // 显示助手响应
                string assistantResponse = chatResponse.choices[0].message.content;
                responseText.text = "助手: " + assistantResponse;
            }
        }
    }
}
