using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MaterialsList")]
public class MaterialsDataSO : ScriptableObject
{
    public List<Material> materialsList;

    public Material GetMaterials(int id) => materialsList[(id)];
}
