using System.Collections.Generic;
using UnityEngine;

public class TrashBallCounter : MonoBehaviour
{
    [Header("Filter")]
    public string ballRootTag = "Ball"; // tag sur l'objet RACINE de la balle

    private int _count = 0;

    // Stocke les "balles déjà comptées"
    private readonly HashSet<int> _scoredBallIds = new HashSet<int>();

    private void OnTriggerEnter(Collider other)
    {
        // On remonte au Rigidbody (racine physique) => évite le double comptage si plusieurs colliders
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return; // si pas de rigidbody, on ignore

        // filtre par tag sur l'objet rigidbody (racine de la balle)
        if (!string.IsNullOrEmpty(ballRootTag) && !rb.CompareTag(ballRootTag))
            return;

        int ballId = rb.GetInstanceID();

        // déjà comptée => ignore
        if (_scoredBallIds.Contains(ballId))
            return;

        _scoredBallIds.Add(ballId);
        _count++;

        Debug.Log($" Balle entrée (1 seule fois) ! Total = {_count}");
    }

    // Option : si tu veux permettre de recompter quand la balle ressort :
    // Décommente ce bloc. Sinon, tu compteras 1 fois par balle par partie.
    /*
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        if (!string.IsNullOrEmpty(ballRootTag) && !rb.CompareTag(ballRootTag))
            return;

        _scoredBallIds.Remove(rb.GetInstanceID());
    }
    */

    // Permet aux autres scripts (comme l'interface) de lire le score
    public int GetScore()
    {
        return _count;
    }
}
