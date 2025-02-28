using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionManager : MonoBehaviour
{
	[Header("General")]
	[SerializeField] private Text versionText;
	[SerializeField] private string versionPrefix = "v";

    private void Start()
    {
        versionText.text = versionPrefix + Application.version;
    }
}
