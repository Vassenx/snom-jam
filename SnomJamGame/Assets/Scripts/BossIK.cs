using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

// Script found on first child of each tentacle bone list on the mesh
// as moving the parent also moves the octopus head a little bit (weight paint issue)
public class BossIK : MonoBehaviour
{
    private Transform[] bones; // bones[0] is the tentacle bone closest to its body
    private float[] boneLengths;
    private Vector3[] positions;
    private Vector3[] directions;
    private float totalLength;
    private Vector3 startPosition;
    
    [SerializeField] private Transform target;
    [SerializeField] private int numOfBones;
    [SerializeField] private int IKFABRIKIterations = 5;
    [SerializeField] private float IKEpsilon = 0.5f;

    private void Start()
    {
        bones = new Transform[numOfBones];
        positions = new Vector3[numOfBones];
        directions = new Vector3[numOfBones];
        boneLengths = new float[numOfBones - 1];
        
        Transform curBone = transform;
        startPosition = curBone.position;
        bones[0] = curBone;
        for (int i = 1; i < numOfBones; i++)
        {
            bones[i] = curBone;
            boneLengths[i - 1] = (bones[i].position - bones[i - 1].position).magnitude;
            totalLength += boneLengths[i - 1];
            
            curBone = curBone.GetChild(0);
        }
        
        /*for (int i = numOfBones - 1; i >= 0; i--)
        {
            bones[i] = curBone;
            if (i != numOfBones - 1)
            {
                boneLengths[i] = (bones[i + 1].position - bones[i].position).magnitude;
                totalLength += boneLengths[i];
            }

            if (i != 0)
            {
                curBone = curBone.GetChild(0);
            }
        }*/
    }

    private void Update()
    {
        IK();
    }

    private void IK()
    {
        for (int i = 0; i < numOfBones; i++)
        {
            positions[i] = bones[i].position;
        }

        if ((target.position - startPosition).sqrMagnitude > (totalLength * totalLength))
        {
            OutOfRangeIK();
        }
        else
        {
            InRangeIK();
        }
        
        for (int i = 0; i < numOfBones; i++)
        {
            bones[i].position = positions[i];
        }
    }

    // straight line to target
    private void OutOfRangeIK()
    {
        // bones are not equal distances apart
        positions[0] = startPosition;
        for (int i = 1; i < numOfBones; i++)
        {
            positions[i] = positions[i-1] + (boneLengths[i - 1] * (target.position - startPosition).normalized);
        }
    }
    
    private void InRangeIK()
    {
        positions[0] = startPosition; // constraint

        for (int iter = 0; iter < IKFABRIKIterations; iter++)
        {
            // forward
            positions[numOfBones - 1] = target.position;
            for (int i = numOfBones - 2; i > 0; i--) // off-by-one errors galore
            {
                var dirFromNextBone = (positions[i] - positions[i + 1]).normalized;
                positions[i] = positions[i + 1] + (dirFromNextBone * boneLengths[i - 1]);
            }

            // backward
            for (int i = 1; i < numOfBones; i++)
            {
                var dirFromPreviousBone = (positions[i] - positions[i - 1]).normalized;
                positions[i] = positions[i - 1] + (dirFromPreviousBone * boneLengths[i - 1]);
            }

            if (((target.position - startPosition).sqrMagnitude - (totalLength * totalLength)) <= IKEpsilon)
                break;
        }
    }

    /*private void OnDrawGizmos()
    {
        var curBone = transform;
        for (int i = 0; i < numOfBones; i++)
        {
            Gizmos.DrawWireSphere(curBone.position, 2f);
            
            if (i < numOfBones - 1 && curBone.childCount > 0)
            {
                curBone = curBone.GetChild(0);
            }
        }
    }*/
}
