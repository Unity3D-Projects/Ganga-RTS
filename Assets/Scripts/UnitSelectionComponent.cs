﻿using GangaGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSelectionComponent : MonoBehaviour
{
    [HideInInspector]
    public bool canBeSelected = true;

    public bool isSelected = false;
    public GameObject projector;
    BaseBehavior baseBehaviorComponent;
    CameraController cameraController;
    BuildingBehavior buildingBehavior;
    Color projectorColor = new Color(1, 1, 1, 1f);
    int projectorColorId = -1;
    Projector projectorComponent;
    bool outlineState = true;
    [HideInInspector]
    public bool intersection = false;

    // Use this for initialization
    void Start ()
    {
        projectorComponent = projector.GetComponent<Projector>();
        projectorComponent.material = new Material(projectorComponent.material);
        baseBehaviorComponent = gameObject.GetComponent<BaseBehavior>();
        cameraController = Camera.main.GetComponent<CameraController>();
        buildingBehavior = gameObject.GetComponent<BuildingBehavior>();
        canBeSelected = baseBehaviorComponent.canBeSelected;
        
        SetOutline(false);

        if (buildingBehavior != null && buildingBehavior.DisableUpdate())
            enabled = false;

        SetSelect(false);
    }

    private void UpdateColor()
    {
        if (baseBehaviorComponent.team <= 0 || GameInfo.playerSpectate)
        {
            projectorColorId = 2;
            projectorColor = new Color(1, 1, 1, 1f);
        }
        else if (cameraController.team != baseBehaviorComponent.team)
        {
            projectorColorId = 0;
            projectorColor = new Color(1, 0, 0, 1f);
        }
        else
        {
            if (cameraController.userId == baseBehaviorComponent.ownerId)
            {
                projectorColorId = 1;
                projectorColor = new Color(0, 1, 0, 1f);
            }
            else
            {
                projectorColorId = 2;
                projectorColor = new Color(1, 1, 1, 1f);
            }
        }

        foreach (cakeslice.Outline outline in baseBehaviorComponent.allOutlines)
            outline.color = projectorColorId;

        projectorComponent.material.color = projectorColor;
    }

    public void SetSelect(bool newState)
    {
        if (!canBeSelected)
            return;

        if (projector.activeSelf != newState)
        {
            UpdateColor();

            projector.SetActive(newState);
            SetOutline(newState);
        }
        if (newState == false)
            intersection = false;
        isSelected = newState;
    }

    public void SetOutline(bool newState)
    {
        if (newState != outlineState)
        {
            UpdateColor();

            foreach (cakeslice.Outline outline in baseBehaviorComponent.allOutlines)
                outline.enabled = newState;

            outlineState = newState;
        }
    }
}
