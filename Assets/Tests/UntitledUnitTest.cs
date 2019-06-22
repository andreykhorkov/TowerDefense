using Zenject;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

[TestFixture]
public class UntitledUnitTest : ZenjectUnitTestFixture
{
    [Test]
    public void RunTest1()
    {
        var prefab = Resources.Load("Cube");
        var cube = Object.Instantiate(prefab);
        var orig = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
        Debug.Log(orig);

        var a = 5;
        var b = 1;
        var s = a + b;

        Assert.AreEqual(s, 6);
    }
}