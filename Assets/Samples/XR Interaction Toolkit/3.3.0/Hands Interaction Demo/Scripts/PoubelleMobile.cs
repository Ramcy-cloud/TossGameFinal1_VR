using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PoubelleMobile : MonoBehaviour
{
    [Header("Paramètres de déplacement")]
    public float rayonDeDeplacement = 5f; // Distance max pour choisir son prochain point
    public float tempsMaximumBloque = 4f; // Sécurité si elle reste coincée

    private NavMeshAgent agent;
    private float timerBloque;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangerDeDestination();
    }

    void Update()
    {
        timerBloque += Time.deltaTime;

        // Si elle est arrivée à destination, elle en choisit une nouvelle
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangerDeDestination();
        }
        // Sécurité : si elle met trop de temps, on force un nouveau trajet
        else if (timerBloque >= tempsMaximumBloque)
        {
            ChangerDeDestination();
        }
    }

    void ChangerDeDestination()
    {
        // On cherche un point au hasard autour d'elle
        Vector3 directionAleatoire = Random.insideUnitSphere * rayonDeDeplacement;
        directionAleatoire += transform.position;

        NavMeshHit hit;

        // On vérifie que ce point est bien sur le "calque bleu" du sol
        if (NavMesh.SamplePosition(directionAleatoire, out hit, rayonDeDeplacement, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // On lui donne l'ordre d'y aller
            timerBloque = 0f;
        }
    }
}