using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    [SerializeField]
    GameObject _tree;

    [SerializeField]
    GameObject _choppedTree;

    public bool _isTreeDestroyed;

	void Start ()
    {
        _isTreeDestroyed = false;
        _tree.SetActive(true);
        _choppedTree.SetActive(false);

    }
	
	void Update ()
    {
		
        if(_isTreeDestroyed == true)
            DestroyTree();


	}

    private void DestroyTree()
    {
        _tree.SetActive(false);
        _choppedTree.SetActive(true);
    }
}
