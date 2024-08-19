using UnityEngine;
using UnityEngine.UI;

public class DecorationController : MonoBehaviour
{
    public Button createButton;
    public Slider posXSlider;
    public Slider posYSlider;
    public Slider posZSlider;
    public Slider rotXSlider;
    public Slider rotYSlider;
    public Slider rotZSlider;
    public Slider scaleXSlider;
    public Slider scaleYSlider;
    public Slider scaleZSlider;

    private GameObject sphere;

    void Start()
    {
        createButton.onClick.AddListener(CreateSphere);

        // Initialize sliders with default values
        posXSlider.onValueChanged.AddListener(UpdatePosition);
        posYSlider.onValueChanged.AddListener(UpdatePosition);
        posZSlider.onValueChanged.AddListener(UpdatePosition);

        rotXSlider.onValueChanged.AddListener(UpdateRotation);
        rotYSlider.onValueChanged.AddListener(UpdateRotation);
        rotZSlider.onValueChanged.AddListener(UpdateRotation);

        scaleXSlider.onValueChanged.AddListener(UpdateScale);
        scaleYSlider.onValueChanged.AddListener(UpdateScale);
        scaleZSlider.onValueChanged.AddListener(UpdateScale);
    }

    void CreateSphere()
    {
        if (sphere != null)
        {
            Destroy(sphere);
        }
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Apply the initial values to the sphere
        UpdatePosition(posXSlider.value);
        UpdateRotation(rotXSlider.value);
        UpdateScale(scaleXSlider.value);
    }

    void UpdatePosition(float value)
    {
        if (sphere != null)
        {
            sphere.transform.position = new Vector3(posXSlider.value, posYSlider.value, posZSlider.value);
        }
    }

    void UpdateRotation(float value)
    {
        if (sphere != null)
        {
            sphere.transform.rotation = Quaternion.Euler(rotXSlider.value, rotYSlider.value, rotZSlider.value);
        }
    }

    void UpdateScale(float value)
    {
        if (sphere != null)
        {
            sphere.transform.localScale = new Vector3(scaleXSlider.value, scaleYSlider.value, scaleZSlider.value);
        }
    }
}
