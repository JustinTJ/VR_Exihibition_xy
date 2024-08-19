using UnityEngine;
using UnityEngine.UI;

public class SpotlightController : MonoBehaviour
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
    public Slider emissionSlider; // Slider to control emission intensity

    private GameObject sphere;
    private Material sphereMaterial;

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

        emissionSlider.onValueChanged.AddListener(UpdateEmission);
    }

    void CreateSphere()
    {
        if (sphere != null)
        {
            Destroy(sphere);
        }
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Create a new material with emission
        sphereMaterial = new Material(Shader.Find("Standard"));
        sphereMaterial.SetColor("_Color", Random.ColorHSV()); // Set random color
        sphereMaterial.EnableKeyword("_EMISSION");
        sphereMaterial.SetColor("_EmissionColor", sphereMaterial.GetColor("_Color") * emissionSlider.value); // Set emission color and intensity
        sphere.GetComponent<Renderer>().material = sphereMaterial;

        // Apply the initial values to the sphere
        UpdatePosition(posXSlider.value);
        UpdateRotation(rotXSlider.value);
        UpdateScale(scaleXSlider.value);
        UpdateEmission(emissionSlider.value);
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

    void UpdateEmission(float value)
    {
        if (sphere != null && sphereMaterial != null)
        {
            Color color = sphereMaterial.GetColor("_Color");
            sphereMaterial.SetColor("_EmissionColor", color * value);
            DynamicGI.SetEmissive(sphere.GetComponent<Renderer>(), color * value);
        }
    }
}
