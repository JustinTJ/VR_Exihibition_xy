using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json; // 如果使用 Newtonsoft.Json 库

public class OpenAIChatCE : MonoBehaviour
{
    public InputField userInputField; // 输入框的引用
    public Button submitButton;       // 提交按钮的引用
    public Text responseText;         // 显示响应的文本组件引用

    public DocumentReader01 documentReader; // DocumentReader 脚本的引用

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
    }

    void OnSubmit()
    {
        string userMessage = userInputField.text;
        if (!string.IsNullOrEmpty(userMessage))
        {
            StartCoroutine(SendChatMessage(userMessage));
        }
    }

    IEnumerator SendChatMessage(string userMessage)
    {
        // 获取文档内容
        string documentContent = documentReader.GetDocumentContent();

        // 创建聊天请求
        List<Message> messages = new List<Message>
        {
            new Message { role = "system", content = "你是一个有帮助的助手。以下是一些专业知识背景：" + documentContent },
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
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
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
