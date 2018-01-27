using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillerBlockRandomiser : MonoBehaviour {

    [SerializeField]
    private Material[] _materials;

    private Renderer[] _childrenRenderers;

	// Use this for initialization
	void Start () {

        _childrenRenderers = new MeshRenderer[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _childrenRenderers[i] = transform.GetChild(i).GetComponent<Renderer>();
        }

        ScrambleColours();
	}
	
    private void ScrambleColours()
    {
        for(int i = 0; i < _childrenRenderers.Length; i++)
        {
            _childrenRenderers[i].material = _materials[Random.Range(0, _materials.Length)];
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
