using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public sealed class ImageSwitcher : MonoBehaviour, IInteractable
{
	[SerializeField] private Sprite[] images;
	private int index;

	private Image displayImage;
    private void Awake()
    {
        displayImage = GetComponent<Image>();
        displayImage.sprite = images[index];
    }

    public bool OnBeginInteract(PlayerInteract _)
    {
        displayImage.sprite = images[++index % images.Length];
        return false;
    }

    public void OnEndInteract() { }
    
}
