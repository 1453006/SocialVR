using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour {

    public GameObject VRplayer;
    public Vector3 spawnPoint;
    public Text status;

    public static GamePlay instance;
    private static List<GvrPermissionsRequester.PermissionStatus> permissionList =
    new List<GvrPermissionsRequester.PermissionStatus>();

    private void Awake()
    {
        spawnPoint = new Vector3(Random.RandomRange(0, 10), 0, Random.RandomRange(0, 10));
        instance = this;
    }
    // Use this for initialization
    void Start () {
        //get audio permission
        string[]  permissionNames = { "android.permission.RECORD_AUDIO" };
        /// request permisson only call when build on devices
#if SHOULD_REQUEST_PREMISSION
        RequestPermissions(permissionNames, status);
#endif
        //spawn VR player to screen
        VRplayer.transform.position = spawnPoint;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void RequestPermissions(string[] permissionNames,Text statusText)
    {
        if (statusText != null)
        {
            statusText.text = "Requesting permission....";
        }
        GvrPermissionsRequester permissionRequester = GvrPermissionsRequester.Instance;
        if (permissionRequester == null)
        {
            statusText.text = "Permission requester cannot be initialized.";
            return;
        }
        Debug.Log("Permissions.RequestPermisions: Check if permission has been granted");
        if (!permissionRequester.IsPermissionGranted(permissionNames[0]))
        {
            Debug.Log("Permissions.RequestPermisions: Permission has not been previously granted");
            if (permissionRequester.ShouldShowRational(permissionNames[0]))
            {
                statusText.text = "This game needs to access external storage.  Please grant permission when prompted.";
                statusText.color = Color.red;
            }
            permissionRequester.RequestPermissions(permissionNames,
                (GvrPermissionsRequester.PermissionStatus[] permissionResults) =>
                {
                    statusText.color = Color.cyan;
                    permissionList.Clear();
                    permissionList.AddRange(permissionResults);
                    string msg = "";
                    foreach (GvrPermissionsRequester.PermissionStatus p in permissionList)
                    {
                        msg += p.Name + ": " + (p.Granted ? "Granted" : "Denied") + "\n";
                    }
                    statusText.text = msg;
                });
        }
        else
        {
            statusText.text = "ExternalStorage permission already granted!";
        }
    }

}
