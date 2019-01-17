using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour {

    private INode _startNode;

    private NavMeshAgent _agent;

    private float _maxWanderDist = 5;



	void Start ()
    {

        _startNode = new SelectorNode
            (
                new SequenceNode
                    (
                        new ConditionNode(AxeInRange),
                        new ActionNode(StealAxe)
                    ),

                 new ActionNode(Wandering)
             );

         StartCoroutine(ExecuteTree());

	}

    IEnumerator ExecuteTree()
    {
        while(Application.isPlaying)
        {
            yield return _startNode.Tick();
        }
    }

    bool AxeInRange()
    {
        return false;
    }

    IEnumerator<NodeResult>StealAxe()
    {
        yield return NodeResult.Succes;
    }

    IEnumerator<NodeResult> Wandering()
    {
        yield return NodeResult.Succes;
    }

}
