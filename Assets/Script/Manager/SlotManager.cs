using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotManager : HuyMonoBehaviour
{
#if UNITY_EDITOR
    //==========================================CustomUI==========================================
    [MenuItem("CONTEXT/SlotManager/Appear")]
    private static void AppearBtn()
    {
        SlotManager slotManager = Selection.activeGameObject.GetComponent<SlotManager>();
        slotManager.Appear();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("CONTEXT/SlotManager/Disappear")]
    private static void DisappearBtn()
    {
        SlotManager slotManager = Selection.activeGameObject.GetComponent<SlotManager>();
        slotManager.Disappear();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
#endif

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        List<Slot> slots = this.GetSlots();
        foreach (Slot slot in slots)
        {
            slot.LoadComponents();
        }
    }

    //===========================================Method===========================================
    public void Appear()
    {
        List<Slot> slots = this.GetSlots();
        foreach (Slot slot in slots) slot.Appear();
    }

    public void Disappear()
    {
        List<Slot> slots = this.GetSlots();
        foreach (Slot slot in slots) slot.Disappear();
    }

    private List<Slot> GetSlots()
    {
        List<Slot> slots = new();
        this.LoadChildComponent(ref slots, transform, "LoadSlots");
        return slots;
    }
}
