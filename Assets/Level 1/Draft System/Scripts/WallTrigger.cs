using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public PontCounter pointCounter;  // Asignar en el Inspector
    public GameObject impactPrefab;    // Prefab de la marca de impacto
    public GameObject[] circles;       // Array para los 11 círculos (Circle01 a Circle11)
    public GameObject square;          // El cuadrado de fondo (con puntaje 0)

    private void OnTriggerEnter(Collider other)
    {
        ScoreBall scoreBall = other.GetComponent<ScoreBall>();

        if (scoreBall != null)
        {
            Vector3 impactPoint = other.transform.position;

            // Verificar en qué círculo impactó
            int pointsToAdd = GetPointsForImpact(impactPoint);

            // Sumar los puntos al contador
            pointCounter.AddPoints(pointsToAdd);

            // Instanciar la marca de impacto
            if (impactPrefab != null)
            {
                GameObject impact = Instantiate(impactPrefab, impactPoint, Quaternion.identity);
                float sizeFactor = other.transform.localScale.magnitude * 0.2f;
                impact.transform.localScale = Vector3.one * sizeFactor;
                impact.transform.SetParent(this.transform);
            }

            // Destruir el proyectil después del impacto
            Destroy(other.gameObject);
        }
    }

    // Método para determinar los puntos según el impacto
    private int GetPointsForImpact(Vector3 impactPoint)
    {
        int score = 0;

        // Recorremos del círculo más interno al más externo
        for (int i = circles.Length - 1; i >= 0; i--)
        {
            if (IsInsideCircle(impactPoint, circles[i]))
            {
                score = i + 1;
                break;
            }
        }

        return score;
    }


    // Verifica si el impacto está dentro del área del cuadrado
    private bool IsInsideSquare(Vector3 impactPoint)
    {
        // Compara las coordenadas del impacto con las del cuadrado
        // Asumiendo que el cuadrado es un objeto de tamaño fijo, ajusta estos valores si es necesario
        Bounds squareBounds = square.GetComponent<Renderer>().bounds;
        return squareBounds.Contains(impactPoint);
    }

    // Verifica si el impacto está dentro del área de un círculo específico
    private bool IsInsideCircle(Vector3 impactPoint, GameObject circle)
    {
        // Compara la distancia entre el impacto y el centro del círculo
        Vector3 circleCenter = circle.transform.position;
        float radius = circle.GetComponent<Renderer>().bounds.extents.x;  // Suponiendo que el círculo es un objeto 3D con un collider

        return Vector3.Distance(impactPoint, circleCenter) <= radius;
    }
}
