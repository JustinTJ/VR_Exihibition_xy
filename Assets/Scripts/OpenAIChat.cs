using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json; // 如果使用Newtonsoft.Json库

public class OpenAIChat : MonoBehaviour
{
    private string apiKey = "sk-svcacct-2tVv0DzyCs9DJmJxyuN_FK8Au0FbHZNgzIOLOZm7jtTfJIbmClcFT3BlbkFJ-LdZb62HsPfng06ZdregyNTZcrJ35-dMH2gBIpfuANnt6wICiGQA"; // 替换为你的API密钥
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
        // 启动一个示例对话
        StartCoroutine(SendChatMessage("Hello, how can I help you?"));
    }

    IEnumerator SendChatMessage(string userMessage)
    {
        // 创建对话请求
        List<Message> messages = new List<Message>
        {
            new Message { role = "system", content = "You are a helpful assistant." },
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
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // 解析响应
                string jsonResponse = request.downloadHandler.text;
                ChatCompletionResponse chatResponse = JsonConvert.DeserializeObject<ChatCompletionResponse>(jsonResponse);

                // 输出助手响应内容
                string assistantResponse = chatResponse.choices[0].message.content;
                Debug.Log("Assistant: " + assistantResponse);
            }
        }
    }
}