using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {

    [SerializeField]
    bool _isTreeDestroyed = false;

    [SerializeField]
    GameObject _tree;

    [SerializeField]
    GameObject _choppedTree;


	void Start ()
    {
        _tree.active = true;
        _choppedTree.active = false;

	}
	
	void Update ()
    {
		
        if(_isTreeDestroyed == true)
            DestroyTree();


	}

    private void DestroyTree()
    {
        _tree.active = false;
        _choppedTree.active = true;
    }
}
