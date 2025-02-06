![Title](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Title.png)

This Project aims to create a implementation of a combination of Drone-Tracking and Drone-Controlling. After the Setup has been completed, you
will be able to get the accurate location of the Drone into the Unity-Scene while also sending control inputs to the Drone.

# Requirements

## 1. Hardware/Parts
- A _DJI-Tello_ Drone
    - Multiple Batteries recommended 
- Installed _OptiTrack_-System
- Tracking-Markers (for the Drone)
- _Wand-Tool_ (or any other Object equipped with markers and trackable via _Motive_)

If two PCs are used - One running _Motive_ + _OptiTrack_ and the second one running Unity:
- A PC/Laptop with a dedicated LAN-Controller (available if the Laptop has a designated LAN-Port - compatibility with Adapters is not guaranteed)
	- LAN Cable
	- ... or any other Way of connecting to the PC, where _Motive_ is running
- A dedicated WiFI-PCIe Network Card (no WiFi-Stick!) within the same PC/Laptop
- Alternatively: Two __bridgeable__ WiFI-PCIe Network Cards

If _Motive_ is running on the same PC as Unity and OptiTrack is set up and running:
- A dedicated WiFI-PCIe Network Card (no WiFi-Stick!) within the PC/Laptop running _Motive_

## 2. Software
- [Unity 2022.3.15f1](https://download.unity3d.com/download_unity/b58023a2b463/Windows64EditorInstaller/UnitySetup64-2022.3.15f1.exe) (other 
Versions may also be used -> check for compatibility with the used Plugins!)
- [_Motive_](https://d2mzlempwep3hb.cloudfront.net/Motive/Motive_3.1.4_Final.exe) (anyways used for _OptiTrack_)
- At least Windows 10 V.1511 (for Connection-Bridging if two WiFi-Cards are used)

## 3. Plugins
- [Unity _OptiTrack_-Plugin](https://d2mzlempwep3hb.cloudfront.net/Plugins/Unity/OptiTrack_Unity_Plugin_1.6.0.unitypackage)
- [_TelloForUnity_](https://github.com/comoc/TelloForUnity) (needs to be cloned -> described in the installation setup)

# Setup

## 1. Create an empty Unity-Project
1. Either use the downloaded Unity-Version or create the Project within the Unity-Hub
	- The Unity-Hub is the recommended way to go and will therefore be explained in this section
	- A _UnityID_ is required to proceed. This process can be completed within the Unity-Hub or by creating a account [here](https://id.unity.com/account/new)
2. Link the installed Editor to the Unity-Hub
	- Installs -> Locate
	- In most cases, the Editor is installed within the ```C:\\Program Files\``` directory
	- Alternatively, the Editor can be installed through the Archive found in the Unity-Hub
    ![Unity_Versions](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Unity_Versions.png)
3. Ensure, that at least a personal License is active
	- Account (arrow in the top left) -> Manage Licenses -> Add -> Get a free personal license
    ![Unity_License](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Unity_License.png)
4. Create the new Project
	- Projects -> New Project -> Select _2022.3.15f1_ in the Version Selector -> Universal 3D -> Select a Name and the Location -> Create Project
    ![Unity_Project](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Unity_Project.png)
    
## 2. Import the required Assets/Plugins
### OptiTrack Plugin
1. Run the downloaded _.unitypackage_-File
2. Ensure that only one instance of the Editor is running and that the Package Importer shows up
3. Hit _All_ and then _Import_ <br>
   ![Unity_OT-Import](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Unity_OT-Import.png)
4. Wait, until the Import-Process completes

### TelloForUnity
1. Clone the GitHub-Repo or download the Code as a ZIP-File
2. Copy the _Assets_-Folder within ```.\TelloForUnity\TelloUnityDemo\``` into your Project-Folder
3. The Console will now show an error, that the _TelloLib_ is missing
![Unity_ConsoleError TelloLib missing](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Unity_Console_TelloLib-Error.png)
	- This is an error within GitHub, as the required folder cannot be cloned/downloaded
	- To fix that, clone or download the [_TelloLib_-Repo](https://github.com/comoc/TelloLib)
	- Copy the _TelloLib_-Folder (not the whole Repo!) into the Unity-Project
4. Fix for the _'Tello' does not contain a definition for 'stopConnecting'_ Error
	- Open the _TelloController.cs_-Script and change the Code in Line 101 from _Tello.stopConnecting()_ to _Tello.disconnect()_
	- Open the _Tello.cs_-Script on line 390 and make the _disconnect_ function _public_
5. It might be necessary to install _Newtonsoft-JSON_ for the Project if is is not already installed as a System-Library within Windows
	- Window -> Package Manager -> Plus-Icon (top left corner) -> _Add package by name..._ -> _com.unity.nuget.newtonsoft-json_ -> Add
    ![Unity_JSON](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Unity_JSON.png)

### UnityGLTF
You might want to add this Package to the Unity-Project if a custom model for the virtual Drone will be used. It is not required to proceed, as
these changes will only be visual - the Code running in the background does not requre any specific RenderMesh
1. Window -> Package Manager -> Plus-Icon (top left corner) -> _Add package from GIT URL..._ -> _https://github.com/KhronosGroup/UnityGLTF.git_ -> Add

**You should now have an open 'SampleScene' without any Errors in the Console. Warnings about deprecated functions can be discarded at this time.**

## 3. Creation of the Unity-Scene
You can choose to create a new Scene, or to use the existing SampleScene. Visual aspects like Lighting is not important for the implementation of
the project, and will for simplicity reasons therefore not be explained here.
1. Main Object for OptiTrack
	- Create an empty GameObject and move it to the Origin of the Level (X=0, Y=0, Z=0)
	- Suggested name: _'Origin(OptiTrack)'_
	- Attach the _OptiTrack Streaming Client_ component to the GameObject
2. Settings for OptiTrack
	- **ServerAddress: IP of the OptiTrack-PC or _localhost_ if Motive is running locally**
	- **LocalAddress: IP of the own PC/Laptop for the OptiTrack Network Interface (will be described later)**
	- **Connection Type: Unicast**
	- Draw Markers: Enable
	- Draw Cameras: Enable
	- **Record on Play: Enable**

    ![Unity_OT-Component](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Unity_OT-Component.png)

	The IP-Address of the own PC/Laptop must be entered manually into the corresponding field, so that the communication between Unity and Motive
	can be handled. It is possible to obtain this IP automatically at every startup, but this requires the LAN Network Controller to be available 
	for the system at any time - a typical LAN-Adapter will not register as a Network Device within Windows, so a PC/Laptop with a dedicated 
    LAN-Port is recommended. <br>
	To implement the automatic obtainment of the IP-Address, you will have to edit the existing Script:
	1. 'Three-Dot Menu' (top right corner of the component) -> Edit Script
	2. Add the following section of code to the script (e.g. before the _Update_-Function)
	```
	void Awake()
     {
        LocalAddress = Dns.GetHostEntry(Dns.GetHostName())
                       .AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                       .ToString();
     }
	```
	Hit _Play_ within the Editor and check if the correct IP has been received (possible via CMD -> _ipconfig /all_). If yes, proceed with the next
    step. If no, follow the steps within the _Troubleshooting_-Section.
    ![CMD_IP](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/CMD_IP.png)
3. TelloController
	- Create an empty GameObject and move it to the Origin of the Level (X=0, Y=0, Z=0)
	- Attach the _TelloController.cs_-Script as a Component (included in the _TelloLib_-Repo)
    ```
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TelloLib;
    using System.Net.Sockets;
    using System.Net;
    using System.Threading;
    using System;

    public class TelloController : SingletonMonoBehaviour<TelloController> {

	    private static bool isLoaded = false;

	    private TelloVideoTexture telloVideoTexture;

	    // FlipType is used for the various flips supported by the Tello.
	    public enum FlipType
	    {

		    // FlipFront flips forward.
		    FlipFront = 0,

		    // FlipLeft flips left.
		    FlipLeft = 1,

		    // FlipBack flips backwards.
		    FlipBack = 2,

		    // FlipRight flips to the right.
		    FlipRight = 3,

		    // FlipForwardLeft flips forwards and to the left.
		    FlipForwardLeft = 4,

		    // FlipBackLeft flips backwards and to the left.
		    FlipBackLeft = 5,

		    // FlipBackRight flips backwards and to the right.
		    FlipBackRight = 6,

		    // FlipForwardRight flips forewards and to the right.
		    FlipForwardRight = 7,
	    };

	    // VideoBitRate is used to set the bit rate for the streaming video returned by the Tello.
	    public enum VideoBitRate
	    {
		    // VideoBitRateAuto sets the bitrate for streaming video to auto-adjust.
		    VideoBitRateAuto = 0,

		    // VideoBitRate1M sets the bitrate for streaming video to 1 Mb/s.
		    VideoBitRate1M = 1,

		    // VideoBitRate15M sets the bitrate for streaming video to 1.5 Mb/s
		    VideoBitRate15M = 2,

		    // VideoBitRate2M sets the bitrate for streaming video to 2 Mb/s.
		    VideoBitRate2M = 3,

		    // VideoBitRate3M sets the bitrate for streaming video to 3 Mb/s.
		    VideoBitRate3M = 4,

		    // VideoBitRate4M sets the bitrate for streaming video to 4 Mb/s.
		    VideoBitRate4M = 5,

	    };

	    override protected void Awake()
	    {
		    if (!isLoaded) {
			    DontDestroyOnLoad(this.gameObject);
			    isLoaded = true;
		    }
		    base.Awake();

		    Tello.onConnection += Tello_onConnection;
		    Tello.onUpdate += Tello_onUpdate;
		    Tello.onVideoData += Tello_onVideoData;

		    if (telloVideoTexture == null)
			    telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

	    }

	    private void OnEnable()
	    {
		    if (telloVideoTexture == null)
			    telloVideoTexture = FindObjectOfType<TelloVideoTexture>();
	    }

	    private void Start()
	    {
		    if (telloVideoTexture == null)
			    telloVideoTexture = FindObjectOfType<TelloVideoTexture>();

		    Tello.startConnecting();
	    }

	    void OnApplicationQuit()
	    {
		    Tello.disconnect();
	    }

	    // Update is called once per frame
	    void Update () {

		    if (Input.GetKeyDown(KeyCode.T)) {
			    Tello.takeOff();
		    } else if (Input.GetKeyDown(KeyCode.L)) {
			    Tello.land();
		    }

		    float lx = 0f;
		    float ly = 0f;
		    float rx = 0f;
		    float ry = 0f;

		    if (Input.GetKey(KeyCode.UpArrow)) {
			    ry = 1;
		    }
		    if (Input.GetKey(KeyCode.DownArrow)) {
			    ry = -1;
		    }
		    if (Input.GetKey(KeyCode.RightArrow)) {
			    rx = 1;
		    }
		    if (Input.GetKey(KeyCode.LeftArrow)) {
			    rx = -1;
		    }
		    if (Input.GetKey(KeyCode.W)) {
			    ly = 1;
		    }
		    if (Input.GetKey(KeyCode.S)) {
			    ly = -1;
		    }
		    if (Input.GetKey(KeyCode.D)) {
			    lx = 1;
		    }
		    if (Input.GetKey(KeyCode.A)) {
			    lx = -1;
		    }
		    //Tello.controllerState.setAxis(lx, ly, rx, ry);
	    }

	    private void Tello_onUpdate(int cmdId)
	    {
		    //throw new System.NotImplementedException();
		    //Debug.Log("Tello_onUpdate : " + Tello.state);
	    }

	    private void Tello_onConnection(Tello.ConnectionState newState)
	    {
		    //throw new System.NotImplementedException();
		    //Debug.Log("Tello_onConnection : " + newState);
		    if (newState == Tello.ConnectionState.Connected) {
                Tello.queryAttAngle();
                Tello.setMaxHeight(50);

			    Tello.setPicVidMode(1); // 0: picture, 1: video
			    Tello.setVideoBitRate((int)VideoBitRate.VideoBitRateAuto);
			    //Tello.setEV(0);
			    Tello.requestIframe();
		    }
	    }

	    private void Tello_onVideoData(byte[] data)
	    {
		    //Debug.Log("Tello_onVideoData: " + data.Length);
		    if (telloVideoTexture != null)
			    telloVideoTexture.PutVideoData(data);
	    }
    }
    ```
4. Canvas for Drone Info (optional)
	- Open the _Master_-Scene within _Assets/Scenes_ as an additional scene (drag it into the empty space of the Hierarchy) and copy the 
      _Canvas_-GameObject to the own Scene
	- The Canvas will show the common instruction for manual flight, for Take Off and Landing and the current Battery SoC.
5. Recreation of the physical flying space (optional)
	- You may recreate the current room, where the drone is flying for a better visual replication. I choose to simply measure the tracking area
	  from the origin and place a modified cube for every wall, so that it is easier to follow the virtual Drone within the space.
![Example Unity-Scene](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/ExampleScene.png)

## 4. Tello-Drone flight controls
The Tello Drone requires a control input containing a total of four float values:
- Lateral movement (left/right)
- Vertical movement (up/down)
- Longitudinal movement (forward/backward)
- Rotation (left/right)

I created a custom _"Flight-System"_, which takes the current position of the drone and the location of the target as the input. There is also the
possibility of providing the target angle, but for simple tracking this field is not required.
```
using System.Collections;
using System.Collections.Generic;
using TelloLib;
using UnityEngine;

public class DroneMover : MonoBehaviour
{
    public (float, float, float, float) CalculateFlightMotion(Transform drone, Vector3 target, float targetAngle)
    {
        float distanceToTarget = Vector3.Distance(drone.position, target);
        Vector3 deltaPosToTarget = target - drone.position;

        deltaPosToTarget = new Vector3(deltaPosToTarget.x, 0.0f, deltaPosToTarget.z);

        float angleToTarget = Vector3.Angle(drone.transform.right, deltaPosToTarget);
        Vector3 crossAngle = Vector3.Cross(drone.transform.right, deltaPosToTarget);
        if (crossAngle.y < 0)
        {
            angleToTarget *= -1;
        }

        Debug.Log("Target | Distance: " + distanceToTarget.ToString("0.000") + "m - Angle: " + angleToTarget.ToString("000.000") + "°");

        float droneLateral = 0.0f;
        droneLateral = AllignAngleWithTarget(angleToTarget);
        float droneVertical = 0.0f;
        droneVertical = AllignHeightWithTarget(target.y, drone.position.y);

        float droneForward = 0.0f;
        float droneHorizontal = 0.0f;

        (droneForward, droneHorizontal) = MoveHorizontal(angleToTarget, distanceToTarget);

        return (droneLateral, droneVertical, droneForward, droneHorizontal);
    }

    private float AllignAngleWithTarget(float angleToTarget)
    {
        float lx = 0f;

        if (Mathf.Abs(angleToTarget) > 45.0f)
        {
            lx = angleToTarget / Mathf.Abs(angleToTarget);
        }
        else
        {
            lx = angleToTarget / 45.0f;
        }

        return lx;
    }

    private float AllignHeightWithTarget(float targetHeight, float droneHeight)
    {
        float ly = 0f;

        if (Mathf.Abs(targetHeight - droneHeight) > 1.0f) //0.1f)
        {
            ly = (targetHeight - droneHeight) / Mathf.Abs(targetHeight - droneHeight);
        }
        else
        {
            ly = targetHeight - droneHeight; //* 10.0f;
        }

        return ly;
    }

    private (float, float) MoveHorizontal(float targetAngle, float targetDistance)
    {
        float rx = 0.0f;
        float ry = 0.0f;

        rx = Mathf.Sin(targetAngle * Mathf.Deg2Rad) * targetDistance;
        ry = Mathf.Cos(targetAngle * Mathf.Deg2Rad) * targetDistance;

        if (targetAngle <= -90.0f && targetAngle >= 90.0f)
        {
            ry *= -1;
        }

        rx = Mathf.Clamp(rx, -1.0f, 1.0f);
        ry = Mathf.Clamp(ry, -1.0f, 1.0f);

        return (rx, ry);
    }
}

```

1. Copy the code above into a new Script and name it _DroneMover.cs_
2. Create a new empty GameObject and attach the Script to it.

The advantage of this approach is the splitting of components into the essential controls and the commands. With my (simple) implementation, the
Script calculates the direct Path to the destination and sends the corresponding flight input to the drone. There are many other ways of implementing
such a flight-system, which are more precise and better adapted to the Tello-Drone, but this code is more like a proof of concept and does its job
anyways.

## 5. Finish Scene-Setup
1. (Optional) Copy the _dji_tello.glb_ Drone Model from the Project Repo (provided at the end of this Wiki page) and import it to your project <br>
    ![Tello_Model](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_Model.png)
    - Alternatively you may create any empty GameObject and proceed with this one - the only disadvantage is, that there won't be any visualization
      of the Drones position.
2. Attach the _OptiTrack Rigid Body_ Component to the Drone/GameObject
    - Streaming Client: Drag the _Origin(OptiTrack)_-GameObject from the Hierarchy onto this field
    - Rigid Body ID: 100 (recommended) -> You can choose any number, but remember is for later usage!
3. (Optional) Attach the Render-Camera as a child Object so that it follows the Drone

## 6. Setup for OptiTrack
1. Launch _Motive_
2. Do the usual calibration including the _Wanding_
    **This has to be done every time the camera position changes**
    - Enter the Calibration-Mode within _Motive_
    - Ensure, that no markers are visible for any camera. You can mask any visible markers, which cannot be removed. This can also be reflections
      from flat surfaces, like screens and mirrors.
    - Follow the Calibration-Setup
    - Do the _Wanding_ with the _Wand-Tool_
    - Use the _"Triangle"_ (precisely called _CS-200 Calibration Square_) and place it at the center of the tracking space to calibrate also the 
      ground plane

**At this point, you may create a new _DataEntry_ within Motive or remove the other already available Rigidbodies. This is not a requirement, but
may help keeping the Scene clean and minimize the Bandwidth usage. This tutorial will assume an empty _DataEntry_ in Motive for the next steps.

### Settings for Data Streaming
Open the Preferences within _Motive_ and switch to the _Streaming_ Tab
- Enable: Of course :wink:
- Local Interface: Select the IP, that is in the same Subnet as your own PC (typically this will be an IP starting with ```10.X.X.X``` or
  ```192.168.X.X```). If only one PC is used, select ```127.0.0.1``` or _loopback_!
- Transmission Type: Unicast
- Up-Axis: Y-Axis
- **Remote Trigger: Enable**

![OptiTrack_Streaming](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/OptiTrack_Streaming.png)
Write down the IP-Address and enter it in Unity within the _Origin(OptiTrack)_-GameObject (Server Address).

### Getting the Drone into OptiTrack
1. Equip the Drone with some reflective tracking markers (how many, is your choice -> but ensure, that the drone can be tracked correctly at any
   height below the cameras view) <br>
   **Hint: As the Drone is a symmetrical object, the lateral orientation may not be captured correctly. To counteract this behavior, place another
   marker at the front or at the back of the Drone. The positioning of that marker is not important - simply remember it, OptiTrack will do the rest.**
   <br>![Tello_with_markers](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_with_markers.png)
2. Place the Drone at the center of the tracking space
3. **Rotate the Drone so that it faces precisely in the -X direction** <br>
   This step is crucial and by far the hardest for the whole Recipe as the orientation of the virtual drone **has to match** the physical one. <br>
   If the orientation mismatch is great enough, the Drone will always fly with a this rotation offset, multiplying with the distance flown!
4. Select all visible Drone-Markers (including the _asymmetrical_ one) within _Motive_
5. Right click -> Create Rigidbody
6. In the bottom right corner, enter 100 as the StreamingID (or the previously chosen number for the Drone)

**Enter the PlayMode within Unity and move the drone manually. You should see the virtual Model moving corresponding the real Drone - if any kind
of rotation offset is clearly visible, do not proceed and redo the last 5 Steps! <br>
Do not try to fly the drone at this point, as it is not connected with the PC!**

## 7. Connecting the Drone to the PC
1. Enable WiFi within the Windows-Settings
    - Remove or disable the automatic connection to any known network
    - Windows will recognize, that the Drones WiFi does not provide any connection to the Internet any may change to another WiFi. <br>
      To prevent this, keep only the Tello-WiFi in the _Connection_-List.
2. Boot up the Drone by pressing the side button and wait for the indicator light to begin blinking.
3. Connect to the Tello-WiFi
4. Check with _ipconfig_, that you now have two active IP-Addresses, one from the LAN-Controller and one from the Drones WiFi. <br>
   If not, go to the _Troubleshooting_ section. You only have to do this step once.
5. Place the Drone on any safe spot within the tracking area

### Testing the Drones connectivity
1. Hit _Play_ within Unity
2. **Step away from the Drone**
3. Press and hold _T_ for at least one Second -> the Drone will now initialize the TakeOff sequence
4. Move the Drone with _W,A,S,D_ for first testing
5. Land the Drone by pressing and holding _L_
6. **Wait until all propellers have stopped rotating while holding the land-key** <br>
   Do not reach for the Drone (especially from above) until it has stopped moving! If the landing is aborted in any way, the Drone will fly back
   straight up to the TakeOff position again!
   
# Flying Modes
Now that the Drone is up and flying and even tracked by OptiTrack, it's time to give it some commands.

## General Setup
Simply having a trackable Drone in Unity is quite boring, so let's add some other components. Every tracked object by OptiTrack can be moved into
Unity by following a few simple steps:
1. Create a new empty GameObject and give it a meaningful name
2. Attach a Mesh Renderer to the Object for visual output
3. Attach a Mesh to the Mesh Filter
4. Attach the _OptiTrackRigidBody_-Script to the GameObject
5. Drag the _StreamingClient_ (in our case, the _Origin(OptiTrack)_) onto the corresponding field
6. Enter any ID and remember it
7. Prepare the tracked object with some reflective markers
8. Within _Motive_, select the tracked markers and create a new Rigidbody
9. Enter the previously assigned ID as the StreamingID

## Attack-Mode
![Tello_Attack](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_Attack.gif)
With the Attack-Mode enabled, the Drone will try to reach the target destination, which is constantly moving. This control-setup is more aggressive
than the normal _DroneMover_, providing about 10 Minutes of action by fleeing from the Drone (until the Battery dies)! :laughing:

```
using System;
using System.Collections;
using System.Collections.Generic;
using TelloLib;
using UnityEngine;

public class DroneAttack : MonoBehaviour
{
    [SerializeField] private GameObject drone;
    private bool attackModeEnabled = false;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attackModeEnabled = !attackModeEnabled;
        }
        if (!attackModeEnabled)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(drone.transform.position, this.transform.position);
        Vector3 deltaPosToTarget = this.transform.position - drone.transform.position;

        deltaPosToTarget = new Vector3(deltaPosToTarget.x, 0.0f, deltaPosToTarget.z);

        float angleToTarget = Vector3.Angle(drone.transform.right, deltaPosToTarget);
        Vector3 crossAngle = Vector3.Cross(drone.transform.right, deltaPosToTarget);
        if (crossAngle.y < 0)
        {
            angleToTarget *= -1;
        }

        Debug.Log("Target | Distance: " + distanceToTarget.ToString("0.000") + "m - Angle: " + angleToTarget.ToString("000.000") + "°");

        float droneLateral = 0.0f;
        droneLateral = AllignAngleWithTarget(angleToTarget);
        float droneVertical = 0.0f;
        droneVertical = AllignHeightWithTarget(this.transform.position.y, drone.transform.position.y);

        float droneForward = 0.0f;
        float droneHorizontal = 0.0f;
        //droneForward = MoveForwardTowardsTarget(angleToTarget, distanceToTarget);

        (droneForward, droneHorizontal) = MoveForAttack(angleToTarget, distanceToTarget);

        ControlDrone(droneLateral, droneVertical, droneForward, droneHorizontal);
    }

    private float AllignAngleWithTarget(float angleToTarget)
    {
        float lx = 0f;

        if (Mathf.Abs(angleToTarget) > 20.0f)
        {
            lx = angleToTarget / Mathf.Abs(angleToTarget);
        }
        else
        {
            lx = angleToTarget / 20.0f;
        }

        return lx;
    }

    private float AllignHeightWithTarget(float targetHeight, float droneHeight)
    {
        float ly = 0f;
        targetHeight += 0.2f;

        if (Mathf.Abs(targetHeight - droneHeight) > 0.5f)
        {
            ly = (targetHeight - droneHeight) / Mathf.Abs(targetHeight - droneHeight);
        }
        else
        {
            ly = targetHeight - droneHeight * 2.0f;
        }

        return ly;
    }

    private float MoveForwardTowardsTarget(float angleToTarget, float distanceToTarget)
    {
        float ry = 0f;

        if (Mathf.Abs(angleToTarget) <= 20.0)
        {
            if (distanceToTarget >= 0.2f)
            {
                ry = 1.0f;
            }
            else
            {
                ry = distanceToTarget * 5.0f;
            }
        }

        return ry;
    }

    private (float, float) MoveForAttack(float targetAngle, float targetDistance)
    {
        float rx = 0.0f;
        float ry = 0.0f;

        rx = Mathf.Sin(targetAngle * Mathf.Deg2Rad);
        ry = Mathf.Cos(targetAngle * Mathf.Deg2Rad);

        if (targetAngle < 0.0f)
        {
            //rx *= -1;
        }
        if (targetAngle <= -90.0f && targetAngle >= 90.0f)
        {
            ry *= -1;
        }

        //rx *= targetDistance;
        //ry *= targetDistance;

        rx = Mathf.Clamp(rx, -1.0f, 1.0f);
        ry = Mathf.Clamp(ry, -1.0f, 1.0f);

        return (rx, ry);
    }

    private void ControlDrone(float droneLateral, float droneVertical, float droneHorizontal, float droneForward)
    {
        Debug.Log("RX: " + droneHorizontal + " | RY: " + droneForward);

        Tello.controllerState.setAxis(droneLateral, droneVertical, droneHorizontal, droneForward);
        
        if (Tello.controllerState.speed != 1)
        {
            Tello.controllerState.setSpeedMode(1);
        }
    }
}
```
1. Copy the Code listed above and create a new Script
2. Attach the Script to the tracked target as a component
3. Start the _PlayMode_
4. Press _T_ for TakeOff
5. **Get yourself ready and hit the Spacebar!**

To stop the Drone:
- Hit _Space_ again _or_
- ... press and hold _L_
- ... wait until the Battery runs out of charge :wink:

## Route-Mode
![Tello_FlightPath](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_FlightPath.gif)
In this mode, you can draw any flight route within the tracking area by moving the tracked GameObject. The Drone will fly according to that path
in a loop until the mode is exited.

```
using System.Collections;
using System.Collections.Generic;
using TelloLib;
using UnityEngine;

public class DroneFlightPath : MonoBehaviour
{
    private bool drawPath;
    private bool flyPath;
    private Vector3 nextWaypoint;
    private LineRenderer flightPath;

    private List<Vector3> flightPathAngles;

    [SerializeField] private Transform drone;

    private DroneMover droneMover;
    [SerializeField] private GameObject target;

    IEnumerator drawCoroutine;
    IEnumerator flyCoroutine;

    void Start()
    {
        flightPath = GameObject.Find("FlightPath").GetComponent<LineRenderer>();
        flightPathAngles = new List<Vector3>();

        droneMover = GameObject.Find("DroneMover").GetComponent<DroneMover>();

        drawCoroutine = DrawFlightPath();
        flyCoroutine = FollowFlightPath();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(flyCoroutine);
        }

        if (Input.GetKeyDown(KeyCode.P) && !drawPath && !flyPath)
        {
            drawPath = true;
            flyPath = false;
            StartCoroutine(drawCoroutine);
        }
        else if ((Input.GetKeyDown(KeyCode.P) && drawPath && !flyPath))
        {
            target.SetActive(true);
            drawPath = false;
            flyPath = true;
            StopCoroutine(drawCoroutine);
            StartCoroutine(flyCoroutine);
        }
        else if ((Input.GetKeyDown(KeyCode.P) && !drawPath && flyPath))
        {
            target.SetActive(false);
            drawPath = false;
            flyPath = false;
            StopCoroutine(flyCoroutine);
            drawCoroutine = DrawFlightPath();
            flyCoroutine = FollowFlightPath();
        }
    }

    private IEnumerator DrawFlightPath()
    {
        flightPath.positionCount = 0;
        flightPathAngles = new List<Vector3>();

        while (true)
        {
            flightPath.positionCount += 1;
            flightPath.SetPosition(flightPath.positionCount - 1, this.transform.position);
            flightPathAngles.Add(this.transform.right);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    private IEnumerator FollowFlightPath()
    {
        nextWaypoint = flightPath.GetPosition(0);
        int currentWaypoint = 0;
        Tello.takeOff();

        while (true)
        {
            target.transform.SetPositionAndRotation(nextWaypoint, Quaternion.identity);

            while (Vector3.Distance(drone.position, nextWaypoint) > 0.33f)
            {
                float lx, ly, rx, ry;
                (lx, ly, rx, ry) = droneMover.CalculateFlightMotion(drone, nextWaypoint, 0.0f);
                //rx = AllignAngleToTarget(drone, flightPathAngles[currentWaypoint]);
                if (Vector3.Distance(drone.position, nextWaypoint) < 0.5f)
                {
                    lx = 0.0f;
                }

                Tello.controllerState.setAxis(lx, ly, rx, ry);
                //Tello.controllerState.setSpeedMode(2);
                yield return new WaitForSecondsRealtime(0.1f);
            }

            if (currentWaypoint < flightPath.positionCount - 1)
            {
                currentWaypoint++;
            }
            else
            {
                currentWaypoint = 0;
            }
            nextWaypoint = flightPath.GetPosition(currentWaypoint);
        }
    }

    private float AllignAngleToTarget(Transform drone, Vector3 targetAngle)
    {
        float angleToTarget = Vector3.Angle(drone.transform.right, targetAngle);
        float lx = 0.0f;

        Vector3 crossAngle = Vector3.Cross(drone.transform.right, targetAngle);
        if (crossAngle.y < 0)
        {
            angleToTarget *= -1;
        }

        if (Mathf.Abs(angleToTarget) > 20.0f)
        {
            lx = angleToTarget / Mathf.Abs(angleToTarget);
        }
        else
        {
            lx = angleToTarget / 20.0f;
        }

        return lx;
    }
}
```

### Requirements
- A tracked GameObject for creating the flight path (follow the General Setup for instructions)
- A GameObject called _FlightPath_ with a attached _LineRenderer_

### Setup
1. Copy the code above and create a script
2. Attach the Script to the flight path creation tool
3. Create another empty GameObject (e.g. a small sphere) and attach it on the _Target_-Field (as the name suggests, this will be the visual
   indication of the next waypoint)

### Usage
1. Again, boot up the Drone and connect to the Tello-WiFi <br>
   **Do not press _T_ for TakeOff this time!**
2. Press _P_ to begin with the flight path tracking <br>
   ![Tello_FlightPathDrawing](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_FlightPathDrawing.gif)
3. Move the tracked object to create the path
4. Step away from the Drone and hit _P_ again
5. Observe the Drone replicating the flight path

To stop the Drone:
- Press _P_ again
- Press and hold _L_

If the Drone is stopped you may...
- ... create a new Path by pressing _P_ again
- ... resume on the old path by pressing _O_

# Extensions
Extending this Project is quite simple as it only requires two steps:
1. Adding a new tracked object into Unity
2. Writing custom code for the flight controls

Some possible extensions would be a _KeepDistance_-Mode, a _Flee_-Mode or a combination of all already existing Modes together. Feel free to try
out the available Project and experiment a bit with the combination of Drone-Flying and OptiTrack!

## Tracking a Skeleton
![Tello_Skeleton](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_Skeleton.gif)
There is also the possibility to track a skeleton instead of a simple Rigidbody. There is no additional Work to do within Unity, you simply have
to attach the _OptitrackSkeletonAnimator_-Script to the GameObject. <br>
For simplicity, it is recommended to simply use the skeleton Prefab available within ```.\Assets\optitrack-unity\OptiTrack\Prefabs\```

![Tello_SkeletonUnity](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_SkeletonUnity.png) <br>
In _Motive_ it is necessary to create a new trackable skeleton, so put on the Motion-Suit! :smiley: <br>
After that, follow the steps for Skeleton-Creation within _Motive_ or simply select all of the tracked Markers and put them into a Skeleton. The
first way might be easier, as you will get visual information from _Motive_ which markers are at the correct place and which ones are not trackable.
When the Skeleton has been created, name it, so you can get the data in Unity. That's now, where the only difference comes in - you don't have to
assign an ID, but a name.

At the end, simply assign the wanted Script to any body part and maybe **do not connect the Attack-Script with the head of the Skeleton! :wink:**

# Safety
![Tello_Safety](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/Tello_Safety.gif)
You may have noticed, that the Drone is not able to stay near the ground by itself and landing can be a quite tedious process. This is
because of a built-in mechanism preventing the Drone from hitting any stationary (thin) object, placed on the floor. This can be used as a
safety-mechanism, especially, when tracking a Skeleton with the Attack-Mode. Simply lay on the ground with the chest down and wait until the Drone
begins circling above the target - then move away in one quick motion as the drone will need some time, to begin attacking again. <br>
... or simply hit the Drone with the leg/hand -> not really the way to go, but better than being injured by the propellers.

# Troubleshooting
Any problems? This section is here to help!

## I've started the PlayMode but Unity is not responding anymore
**Very important: Check if _Motive_ is frozen! (Look at the Timer at the bottom of the Window)**
- _Motive_ works flawlessly: <br>
    ... then it's a simple Bug in Unity. Restart the Editor and try it again.
- _Motive_ is also not responding: <br>
    ... **DO NOT EXIT UNITY AT THIS POINT OR DISCONNECT THE PC FROM THE LAN-NETWORK!** If you do so, you **will have to** recalibrate the whole
    OptiTrack System, including _Wanding_, etc. - and if you didn't reset the network interface at the PC running _Motive_, you can do that again,    
    as the connection to Unity never has been closed by Windows! <br>
    ... try to close _Motive_! Use the whole Windows-Arsenal, including the Task-Manager and ```taskkill``` if necessary!

There is a critical Bug somewhere located in the Code of the _Motive_-Service, which is running in the Background and manages the connection to the
cameras. Unity might seem not responding, but it is awaiting data from _Motive_. If now _Motive_ is able to continue within its Code (e.g. by
closing Unity), it will throw an Exception and delete the calibration data a a safety-mechanism, but without restarting the OptiTrack-Service. If
the process gets interrupted by stopping _Motive_ first, nothing critical will happen and Unity will receive the Data right after restarting
_Motive_ again.

## I can't have two simultaneous connections via LAN and WiFi! What to do?
It might be necessary to _bridge_ the two connections together. But at first, ensure, that the PC is connecting only to the Tello-WiFi and not to
any other one providing an internet connection! If the _bridging_ is done too early ... congratulations! **You've now created a big "Infinity"-Loop
in any Domain-Network including alerting every IT-Admin responsible for the infrastructure.** <br>
Check also, if you have any virtual Network-Card installed, like _Hyper-V_ or _Wireguard_.

No and no? Then, let's bridge the connections together!
1. Open the _old_ control panel in Windows
2. _Network and Internet_ -> _Network and Sharing Center_
3. _Change adapter settings_
4. Both select the WiFi and the LAN-Interface (or both WiFi-Networks
![Connection Bridging - Network-Interfaces](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/ConnectionBridging_Interfaces.png)
5. Right click -> _Bridge Connections_
6. You will now see a new Network Interface -> Remember removing the Bridge before changing the WiFi or you won't have any Internet-Connection <br>
![Connection Bridging - New Bridge](https://raw.githubusercontent.com/PonjevicDino/SCO-OptiDrone/refs/heads/main/Wiki_Resources/ConnectionBridging_Bridge.png)

7. Try connecting to the WiFi again -> Windows will show the LAN-Interface as _disconnected_, but it is available via the Bridge

## The Drone crashed and won't start again!
That's a safety-mechanism -> simply reboot the Drone, connect to the WiFi and hit _T_ to trigger TakeOff!

## The Drone is "oscillating" up and down!
Sadly, that's a limitation of the Drones capabilities in small spaces if the _FlyMode_ is set to _fast_. There is a latency from tracking the Drone
via OptiTrack, sending the Data to Unity, proceeding them and sending the flight commands to the Tello-Drone. This, combined with the unstable
motion within small areas, makes the Drone to always _"overshoot"_ the target.

## The Drone is flying in the wrong direction!
Recalibrate the Drone within _Motive_ by placing it in the center of the tracking space, facing the X-Axis as precise as possible!

## The Drone is not reacting to the landing command!
Is the WiFi disconnected? If yes, the Drone continues with the last received command and will therefore crash at any point. If no, check if the
Unity Window is in _"focus"_ for the Keyboard-Input.

## The Drone seems like "idling" on the ground with the propellers spinning but not taking off!
This can be the result of two conditions. Either, a motor has overheated and needs to cool down or the Drone is not able to spin up the propellers
at a constant rate. In both cases, you may hold down the _L_ key and the Drone should spin down slowly. Then, check the Drone and the propellers
for any damage.

## The LED on the Drone is blinking red and it is not able to sustain a selected height!
The Battery is almost empty. Either land the Drone or brace for impact after a few minutes!

## OptiTrack is tracking two Drones/Objects somehow, but it should only be one!
Recalibrate the cameras! The position of some cameras has been changed since the last calibration.

# The GitHub Repo
Get the current state of the Project from [here](https://github.com/PonjevicDino/SCO-OptiDrone)

****Videos created with kind help from Luca Geiger :smile:****