using UnityEngine;

public class EmissiveOscillator : MonoBehaviour {

	MeshRenderer emissiveRenderer;
	Material[] emissiveMaterials;
    public Color oscilateLow;
    public Color oscilateHigh;


	void Start () {
		emissiveRenderer = GetComponent<MeshRenderer>();
        emissiveMaterials = emissiveRenderer.materials;
    }
	
    
	void Update () {
		Color c = Color.Lerp(
			oscilateHigh, oscilateLow,
			Mathf.Sin(Time.time * Mathf.PI) * 0.5f + 0.5f
		);
        foreach(Material emissiveMaterial in emissiveMaterials)
        {
            emissiveMaterial.SetColor("_Emission", c);
        }		
	}
}