using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour {


    [SerializeField]
    private CharacterControllerBehaviour _charCTRLScript;

    [SerializeField]
    GameObject _tree;

    [SerializeField]
    GameObject _choppedTree;

    [SerializeField]
    private int _treeHealth;

    public bool _isTreeDestroyed;

	void Start ()
    {
        _isTreeDestroyed = false;
        _tree.SetActive(true);
        _choppedTree.SetActive(false);
    }
	
	void Update ()
    {
		
        if(_treeHealth <= 0)
            DestroyTree();





    }

    private void DestroyTree()
    {
        _tree.SetActive(false);
        _choppedTree.SetActive(true);
    }

    void OnTriggerStay(Collider other)
    {

        if(other.tag == "Character" && _charCTRLScript._swingAxe == true && _charCTRLScript._hitTree == true)
        {
            _treeHealth -= 1;
            Debug.Log("[TREE] Tree Hit");
        }

        
    }

}
