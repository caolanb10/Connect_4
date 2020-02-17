using GoogleARCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class MyGameplayController : MonoBehaviour
{

	public GameObject ARCoreRoot;

	// Need to define ourselves
	public ARCoreWorldOriginHelper ARCoreWorldOriginHelper;

	private const float k_ResolvingPrepareTime = 3.0f;

	private float m_TimeSinceStart = 0.0f;

	private bool m_PassedResolvingPreparedTime = false;

	private bool m_AnchorAlreadyInstantiated = false;

	private bool m_AnchorFinishedHosting = false;

	private bool m_IsQuitting = false;

	private Component m_WorldOriginAnchor = null;

	private Pose? m_LastHitPose = null;

	private ApplicationMode m_CurrentMode = ApplicationMode.Ready;

	private ActiveScreen m_CurrentActiveScreen = ActiveScreen.LobbyScreen;

	// Need to define ourselves
	private CloudAnchorsNetworkManager m_NetworkManager;

	public bool IsOriginPlaced { get; private set; }

	public enum ApplicationMode
	{
		Ready,
		Hosting,
		Resolving,
	};

	// Start is called before the first frame update
	void Awake()
    {
		Application.targetFrameRate = 60;
    }

	public void Start()
	{
		ARCoreRoot.SetActive(true);

	}
		// Update is called once per frame
	void Update()
    {
        
    }
}
