using NaughtyAttributes;
using NUnit.Framework.Internal.Filters;
using UnityEngine;

public class NumberTestScript : MonoBehaviour
{
    [SerializeField] private int TestNumber;
    [Button]
    public void PrintTestNumber()
    {
        foreach (var translation in NumberTranslator.GetNumberTranslation(TestNumber))
        {
            Debug.Log(translation);
        }
    }
    [Button]
    public void TestNumbers()
    {
        for (int i = 0; i < 100; i++)
        {
            foreach (var translation in NumberTranslator.GetNumberTranslation(i))
            {
                Debug.Log(translation);
            }
        }
    }
}
