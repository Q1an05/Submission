using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class Versializer : MonoBehaviour
{
    public GameObject jointPrefab;
    public PXR_Hand targetHand;  // 引用要可视化的手部对象
    
    private List<GameObject> jointSpheres = new List<GameObject>();
    
    void Start()
    {
        if (targetHand == null)
        {
            Debug.LogError("请在Inspector中设置目标手部对象!");
            return;
        }
        
        if (jointPrefab == null)
        {
            Debug.LogError("请设置关节小球预制体!");
            return;
        }
        
        // 为每个手部关节创建小球
        CreateJointVisualizers();
    }

    void Update()
    {
        // 实时更新小球位置
        UpdateJointVisualizersPosition();
    }
    
    private void CreateJointVisualizers()
    {
        // 清除可能已存在的小球
        foreach (var sphere in jointSpheres)
        {
            if (sphere != null)
                Destroy(sphere);
        }
        jointSpheres.Clear();
        
        // 为每个手部关节创建可视化小球
        for (int i = 0; i < targetHand.handJoints.Count; i++)
        {
            if (targetHand.handJoints[i] != null)
            {
                GameObject sphere = Instantiate(jointPrefab, targetHand.handJoints[i].position, Quaternion.identity);
                sphere.name = "Joint_" + i.ToString();
                jointSpheres.Add(sphere);
            }
            else
            {
                // 如果关节为空，添加null占位，保持索引一致
                jointSpheres.Add(null);
            }
        }
    }
    
    private void UpdateJointVisualizersPosition()
    {
        // 确保joint数量匹配
        if (jointSpheres.Count != targetHand.handJoints.Count)
        {
            CreateJointVisualizers();
            return;
        }
        
        // 更新每个小球的位置
        for (int i = 0; i < jointSpheres.Count; i++)
        {
            if (jointSpheres[i] != null && targetHand.handJoints[i] != null)
            {
                jointSpheres[i].transform.position = targetHand.handJoints[i].position;
            }
        }
    }
}
