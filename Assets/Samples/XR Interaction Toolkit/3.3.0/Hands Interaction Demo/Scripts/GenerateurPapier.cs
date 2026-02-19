using System.Collections; // Indispensable pour utiliser les Coroutines
using System.Collections.Generic;
using UnityEngine;

public class GestionnaireJeuPapier : MonoBehaviour
{
    [Header("Ce qu'on génère")]
    public GameObject prefabPapier; // Le Prefab de la boule de papier

    [Header("Les emplacements exacts")]
    public Transform[] pointsDapparition; // La liste des points fixes

    [Header("Chronomètre et Apparition")]
    public float tempsImparti = 30f; // Le temps total de la manche (ex: 30 sec)
    public float delaiEntreApparitions = 0.05f; // Le temps entre l'apparition de chaque papier

    private float timer = 0f;
    private List<GameObject> papiersActifs = new List<GameObject>();

    void Start()
    {
        // Au lancement, on démarre la Coroutine (la vague en cascade)
        StartCoroutine(GenererVagueAvecDelai());
    }

    void Update()
    {
        // Le chronomètre global de la manche avance
        timer += Time.deltaTime;

        if (timer >= tempsImparti)
        {
            NettoyerAnciensPapiers();

            // On relance la vague en cascade
            StartCoroutine(GenererVagueAvecDelai());

            timer = 0f;
        }
    }

    // IEnumerator = Une fonction qui peut se mettre en pause
    IEnumerator GenererVagueAvecDelai()
    {
        if (pointsDapparition.Length == 0) yield break;

        foreach (Transform point in pointsDapparition)
        {
            GameObject nouveauPapier = Instantiate(prefabPapier, point.position, point.rotation);
            papiersActifs.Add(nouveauPapier);

            // C'est ici que la magie opère : le code fait une pause de 0.05 sec avant de passer au point suivant !
            yield return new WaitForSeconds(delaiEntreApparitions);
        }
    }

    void NettoyerAnciensPapiers()
    {
        foreach (GameObject papier in papiersActifs)
        {
            if (papier != null)
            {
                Destroy(papier);
            }
        }
        papiersActifs.Clear();
    }
}