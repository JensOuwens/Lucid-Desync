using System.Collections;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public static LoopManager Instance { get; private set; }

    [Header("Loop Data")]
    [SerializeField] private LoopData[] loops;
    [SerializeField] private int currentLoopId;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerStartPosition;

    [Header("Portal")]
    [SerializeField] private GameObject portalBarrier;

    [Header("Timing")]
    [SerializeField] private float playerSpawnDelay = 1f;

    private bool portalOpen;
    private bool loopInProgress;

    private Coroutine loopRoutine;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    // Called when the game first starts.
    public void StartLoops()
    {
        if (loops == null || loops.Length == 0)
        {
            Debug.LogWarning("No loops have been assigned.");
            return;
        }

        currentLoopId = 0;

        StartCurrentLoop();
    }


    // Starts whatever loop currentLoopId currently points to.
    private void StartCurrentLoop()
    {
        if (currentLoopId >= loops.Length)
        {
            CompleteAllLoops();
            return;
        }

        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
        }

        loopRoutine = StartCoroutine(BeginLoop());
    }


    private IEnumerator BeginLoop()
    {
        LoopData currentLoop = loops[currentLoopId];

        loopInProgress = true;
        portalOpen = false;

        // Make sure the portal is closed at the beginning.
        SetPortalOpen(false);

        // --------------------------------------------------
        // Reset / position player
        // --------------------------------------------------

        ResetPlayerPosition();

        // --------------------------------------------------
        // Apply this loop's rules
        // --------------------------------------------------

        RuleManager.Instance.ChangeRules(currentLoop);

        // --------------------------------------------------
        // Give the player a moment before combat starts
        // --------------------------------------------------

        yield return new WaitForSeconds(playerSpawnDelay);

        // --------------------------------------------------
        // Display transition message
        // --------------------------------------------------

        DisplayTransitionMessage(currentLoop.TransitionMessage);

        // --------------------------------------------------
        // Start enemy encounter
        // --------------------------------------------------

        EnemyManager.Instance.StartLoop(currentLoop);

        loopRoutine = null;
    }


    // Called by EnemyManager once the entire LoopData
    // has been completed and every enemy is dead.
    public void AllEnemiesDead()
    {
        if (!loopInProgress)
        {
            return;
        }

        if (portalOpen)
        {
            return;
        }

        portalOpen = true;

        SetPortalOpen(true);

        Debug.Log($"Loop {currentLoopId} completed combat.");
    }


    // Call this from the portal's trigger when the player
    // successfully enters the portal.
    public void PlayerEnteredPortal()
    {
        if (!loopInProgress)
        {
            return;
        }

        if (!portalOpen)
        {
            return;
        }

        loopInProgress = false;

        currentLoopId++;

        if (currentLoopId >= loops.Length)
        {
            CompleteAllLoops();
            return;
        }

        StartCurrentLoop();
    }


    // Called when the player dies.
    // The current loop starts again from the beginning.
    public void ResetCurrentLoop()
    {
        if (!loopInProgress)
        {
            return;
        }

        Debug.Log($"Resetting loop {currentLoopId}.");

        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }

        loopInProgress = false;

        // Stop and destroy the current enemy encounter.
        EnemyManager.Instance.ResetEncounter();

        // Close the portal.
        SetPortalOpen(false);

        // Reset the player position.
        ResetPlayerPosition();

        // Start the exact same loop again.
        StartCurrentLoop();
    }


    private void ResetPlayerPosition()
    {
        if (player == null)
        {
            Debug.LogWarning("LoopManager has no player assigned.");
            return;
        }

        if (playerStartPosition == null)
        {
            Debug.LogWarning(
                "LoopManager has no player start position assigned."
            );

            return;
        }

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        player.position = playerStartPosition.position;
        player.rotation = playerStartPosition.rotation;

        if (controller != null)
        {
            controller.enabled = true;
        }
    }


    private void SetPortalOpen(bool open)
    {
        portalOpen = open;

        if (portalBarrier == null)
        {
            Debug.LogWarning("LoopManager has no portal barrier assigned.");
            return;
        }

        portalBarrier.SetActive(!open);
    }


    private void DisplayTransitionMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Debug.Log($"Transition message: {message}");

        // TODO:
        // Send the message to your UI manager.
        //
        // The intended animation:
        //
        // Black screen
        //      ↓
        // Display message
        //      ↓
        // Wait / animation
        //      ↓
        // Fade black screen away
    }


    private void CompleteAllLoops()
    {
        loopInProgress = false;

        SetPortalOpen(false);

        Debug.Log("ALL LOOPS COMPLETED.");

        // TODO:
        // Trigger whatever happens after the final loop.
        // For example:
        // - End screen
        // - Credits
        // - Final boss
        // - Game completion
    }


    // --------------------------------------------------
    // Optional getters
    // --------------------------------------------------

    public int GetCurrentLoopId()
    {
        return currentLoopId;
    }


    public LoopData GetCurrentLoop()
    {
        if (currentLoopId < 0 || currentLoopId >= loops.Length)
        {
            return null;
        }

        return loops[currentLoopId];
    }


    public bool IsPortalOpen()
    {
        return portalOpen;
    }
}