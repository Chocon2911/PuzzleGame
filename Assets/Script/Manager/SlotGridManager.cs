using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotGridManager : HuyMonoBehaviour
{
#if UNITY_EDITOR
    //============================================Menu============================================
    [MenuItem("CONTEXT/SlotGridManager/Update Slot")]
    private static void UpdateSlotButton()
    {
        SlotGridManager manager = Selection.activeGameObject.GetComponent<SlotGridManager>();
        manager.UpdateSlots();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("CONTEXT/SlotGridManager/Disappear Background")]
    private static void DisappearBackgroundButton()
    {
        SlotGridManager manager = Selection.activeGameObject.GetComponent<SlotGridManager>();
        manager.DisappearBackground();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("CONTEXT/SlotGridManager/Appear Background")]
    private static void AppearBackgroundButton()
    {
        SlotGridManager manager = Selection.activeGameObject.GetComponent<SlotGridManager>();
        manager.AppearBackground();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    [MenuItem("CONTEXT/SlotGridManager/Spawn Blocks")]
    private static void SpawnBlocksButton()
    {
        SlotGridManager manager = Selection.activeGameObject.GetComponent<SlotGridManager>();
        manager.SpawnBlocks();
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
#endif
    //==========================================Variable==========================================
    [SerializeField] private SlotGridSO so;
    [SerializeField] private List<Slot> slots;
    [SerializeField] private TextAsset csvFile;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.SpawnSlots();
    }

    //===========================================Method===========================================
    private void UpdateSlots()
    {
        foreach (Slot slot in this.slots) slot.LoadComponents();
    }

    private void AppearBackground()
    {
        foreach (Slot slot in this.slots) slot.Appear();
    }

    private void DisappearBackground()
    {
        foreach (Slot slot in this.slots) slot.Disappear();
    }

    private void SpawnSlots()
    {
        List<int> blockTypeIndexs = new();
        if (this.csvFile != null) blockTypeIndexs = this.ReadCSVToList();

        foreach (Slot slot in this.slots)
        {
            if (slot != null) DestroyImmediate(slot.gameObject);
            else this.slots.Remove(slot);
        }
        this.slots.Clear();
        Slot slotPrefab = Resources.Load<Slot>("Prefab/Slot");
        Vector3 spawnPos = Vector3.zero;
        spawnPos.x = -this.so.Size.x / 2f;
        spawnPos.z = this.so.Size.y / 2f;

        int index = -1;
        while (spawnPos.z >= -this.so.Size.y / 2f && spawnPos.x <= this.so.Size.x / 2f)
        {
            index++;

            GameObject newSlotObj = Instantiate(slotPrefab.gameObject, spawnPos, Quaternion.identity, this.transform);
            Slot newSlot = newSlotObj.GetComponent<Slot>();
            if (csvFile != null) newSlot.spawnBlockType = (BlockType)blockTypeIndexs[index];
            newSlot.LoadComponents();
            newSlot.transform.SetParent(transform);
            newSlot.transform.localPosition = spawnPos;
            this.slots.Add(newSlot);
            spawnPos.x += this.so.Space.x;

            if (spawnPos.x < this.so.Size.x / 2f) continue;
            spawnPos.z -= this.so.Space.y;
            spawnPos.x = -this.so.Size.x / 2f;
        }
    }

    private void SpawnBlocks()
    {
        foreach (Slot slot in this.slots) slot.SpawnBlock();
    }

    private List<int> ReadCSVToList()
    {
        List<int> result = new List<int>();

        string[] lines = csvFile.text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] cells = line.Trim().Split(',');

            foreach (string cell in cells)
            {
                if (int.TryParse(cell, out int number))
                {
                    result.Add(number);
                }
                else
                {
                    Debug.LogWarning("Invalid cell value: " + cell);
                }
            }
        }

        return result;
    }
}
