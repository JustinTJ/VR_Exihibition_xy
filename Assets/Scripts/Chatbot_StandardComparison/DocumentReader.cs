using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DocumentReader02 : MonoBehaviour
{
    public string filePath = "E:/A_Unity works/VR_exihibition_xy/Assets/Document/DesignCode.txt"; // 使用相对路径

    private string documentContent;

    void Start()
    {
        ReadDocument();
    }

    void ReadDocument()
    {
        Debug.Log("Checking file at: " + filePath);

        if (File.Exists(filePath))
        {
            documentContent = File.ReadAllText(filePath);
            Debug.Log("Document Content Loaded.");
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    public string GetDocumentContent()
    {
        return documentContent;
    }
}
