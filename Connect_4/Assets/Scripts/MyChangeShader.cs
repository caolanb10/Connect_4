using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyChangeShader : MonoBehaviour
{
	Renderer renderer;

	Material NormalMaterial;

	public Material SelectedMaterial;

	void Start()
	{
		renderer = GetComponent<Renderer>();
		NormalMaterial = renderer.material;
	}

	public void ChangeShaderSelected()
	{
		renderer.material = SelectedMaterial;
	}

	public void ChangeShaderNormal()
	{
		renderer.material = NormalMaterial;
	}
}
