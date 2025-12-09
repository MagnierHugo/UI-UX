using UnityEngine;


public static class Layers
{
    public static readonly LayerMask Default = LayerMask.GetMask(nameof(Default));
    public static readonly LayerMask Interactable = LayerMask.GetMask(nameof(Interactable));
}
