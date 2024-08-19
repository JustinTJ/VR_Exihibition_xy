using UnityEngine;
using UnityEngine.UI;

public class CorridorController : MonoBehaviour
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

    private GameObject cube;

    void Start()
    {
        createButton.onClick.AddListener(CreateCube);

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

    void CreateCube()
    {
        if (cube != null)
        {
            Destroy(cube);
        }
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Reset sliders to default values
        ResetSlidersToDefault();

        // Apply the initial values to the cube
        UpdatePosition(posXSlider.value);
        UpdateRotation(rotXSlider.value);
        UpdateScale(scaleXSlider.value);
    }

    void ResetSlidersToDefault()
    {
        // Set your desired default values for each slider here
        posXSlider.value = 0;  // Default position X
        posYSlider.value = 1;  // Default position Y
        posZSlider.value = 0;  // Default position Z

        rotXSlider.value = 0;  // Default rotation X
        rotYSlider.value = 0;  // Default rotation Y
        rotZSlider.value = 0;  // Default rotation Z

        scaleXSlider.value = 1;  // Default scale X
        scaleYSlider.value = 1;  // Default scale Y
        scaleZSlider.value = 1;  // Default scale Z
    }

    void UpdatePosition(float value)
    {
        if (cube != null)
        {
            cube.transform.position = new Vector3(posXSlider.value, posYSlider.value, posZSlider.value);
        }
    }

    void UpdateRotation(float value)
    {
        if (cube != null)
        {
            cube.transform.rotation = Quaternion.Euler(rotXSlider.value, rotYSlider.value, rotZSlider.value);
        }
    }

    void UpdateScale(float value)
    {
        if (cube != null)
        {
            cube.transform.localScale = new Vector3(scaleXSlider.value, scaleYSlider.value, scaleZSlider.value); 
        }
    }
}
