using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NewEditorTest {

    [Test]
    public void EditorTest()
    {
        Debug.Log(typeof(System.Object).AssemblyQualifiedName);
    }
}
