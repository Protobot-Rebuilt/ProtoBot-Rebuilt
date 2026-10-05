using Protobot.SelectionSystem;
using Protobot;
using System;
using UnityEngine;
using Protobot.InputEvents;
using UnityEngine.InputSystem;

public class ColorPicker : Selector
{
    private Action<ISelection> setEventHandler;
    public override event Action<ISelection> setEvent
    {
        add { setEventHandler += value; }
        remove { setEventHandler -= value; }
    }
    public override event Action clearEvent
    {
        add { }
        remove { }
    }

    [SerializeField] private MouseCast mouseCast;
    [SerializeField] private InputEvent input;
    [SerializeField] private ColorTool colorToolGameObject;
    public string colorPickerKeybind;

    public void Awake()
    {
        input.performed += () => OnPerformInput();
    }
    //100% there is a better way to do this but whatever
    //TODO: make this good and all that jazz
    private void OnPerformInput()
    {
        if (!Protobot.InputEvents.RebindAction.PrimaryModifierPressed || colorPickerKeybind.Contains("Ctrl"))
        {
            if (ColorTool.CustomColor)
            {
                if (!MouseInput.overUI)
                {
                    if (mouseCast.overObj)
                    {
                        GameObject targetGameObject = mouseCast.gameObject;
                        Renderer component;
                        if (targetGameObject != null)
                        {
                            targetGameObject.TryGetComponent<Renderer>(out component);

                            if (component == null)
                            {
                                if (targetGameObject.transform.parent != null)
                                {
                                    targetGameObject.transform.parent.gameObject.TryGetComponent<Renderer>(
                                        out component);
                                    if (targetGameObject.transform.parent.gameObject != null)
                                    {
                                        targetGameObject.transform.parent.gameObject.TryGetComponent<Renderer>(
                                            out component);
                                    }
                                }
                            }

                            if (component == null)
                                return;
                        }

                        if (targetGameObject.TryGetComponent<Renderer>(out component) ||
                            targetGameObject.transform.parent.gameObject.TryGetComponent<Renderer>(out component))
                        {
                            if (component == null)
                                return;
                            if (RendererColorUtility.IsMetallicSelectable(component))
                            {
                                ColorTool.Material = component.sharedMaterial;
                                ColorTool.ColorToSet = RendererColorUtility.GetTintColor(component);
                                colorToolGameObject.UpdateColorSliders();
                            }
                        }
                    }
                }
            }
        }
    }
}