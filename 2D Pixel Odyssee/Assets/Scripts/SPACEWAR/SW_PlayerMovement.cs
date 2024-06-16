using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform center; // Zentrum des Kreises
    float radius = 5f; // Radius des Kreises
    public float speed = 1.5f; // Geschwindigkeit des Spielers
    float rotationSpeed = 100f; // Rotationsgeschwindigkeit
    float maxAttractionForce = 2f; // Maximale Staerke der Anziehungskraft zum Zentrum
    float attractionIncrement = 0.8f; // Inkrement fuer die Erhoehung der Anziehungskraft

    private float angle = 0f;
    private float currentAttractionForce = 0f;
    SWSoundManager SWSoundManager;

    public GameObject enemy;
    public GameObject winPanel;
    public GameObject losePanel; 

//___________________________________________________________________________________________________________
//---------------------------------Start() and Update() below------------------------------------------------

    private void Start()
    {
        SWSoundManager = GameObject.FindGameObjectWithTag("SoundSpaceWar").GetComponent<SWSoundManager>();
    }
    void Update()
    {
        if(enemy != null) {

            //--------------------------Circle movement below------------------------------------------------
            //--------------------------from script SW_BlackHoleMovement-------------------------------------

            // Berechne den neuen Winkel basierend auf der Geschwindigkeit
            angle += speed * Time.deltaTime;

            // Berechne die neue Position des Spielers auf dem Kreis
            float x = center.position.x + Mathf.Cos(angle) * radius;
            float y = center.position.y + Mathf.Sin(angle) * radius;

            // Setze die Position des Spielers
                transform.position = new Vector2(x, y);

//____________________________________________________________________________________________________________
//--------------------------Movement sounds below-------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.D))
                {
                    SWSoundManager.PlaySfxSW(SWSoundManager.BoostSW);
                }
            //--------------------------Player Movement below--------------------------------------------------------------
            
            // Rotation mit den Pfeiltasten/AD
            float rotationInput = -Input.GetAxis("Horizontal"); // Negativer Wert fuer umgekehrte Richtung
            transform.Rotate(Vector3.forward, rotationInput * rotationSpeed * Time.deltaTime);

            // Berechne den neuen Winkel basierend auf der Geschwindigkeit
            angle += speed * Time.deltaTime;

            // Berechne die neue Position des Spielers auf dem Kreis
            float xp = center.position.x + Mathf.Cos(angle) * radius;
            float yp = center.position.y + Mathf.Sin(angle) * radius;

            // Berechne die Richtung zum Zentrum
            Vector2 directionToCenter = ((Vector2)center.position - new Vector2(x, y)).normalized;

            // Erhöhe die Anziehungskraft schrittweise
            currentAttractionForce = Mathf.Min(currentAttractionForce + attractionIncrement, maxAttractionForce);

            // Berechne die Anziehungskraft zum Zentrum
            Vector2 attraction = directionToCenter * currentAttractionForce;

            // Berechne die endgültige Position des Spielers mit Anziehungskraft
            x += attraction.x;
            y += attraction.y;

            // Setze die Position des Spielers
            transform.position = new Vector2(x, y);
        }
        //--------------------------Winning screen below--------------------------------------------------------------
        else if (enemy == null){
            speed = 0f;
            winPanel.SetActive(true);
        }
    }
//____________________________________________________________________________________________________________
//--------------------------Loosing screen below--------------------------------------------------------------
    void OnCollisiionEnter2D(Collision2D col) {     //DOESNT WORK DUNNO WHY DOESNT MATTER
        losePanel.SetActive(true);                  //Player is never hit anyway
        Destroy(this);
    }
}
