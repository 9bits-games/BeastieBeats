using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdController : MonoBehaviour9Bits {
    public ScoreManager scoreManager;
    //The maximun number of persons allowed to be in scene.
    public int maximunNumberOfPersons = 20;
    //Time that elapses for a person to appear in the street.
    public float apearingTime = 3f;
    public float personAppearPosition = 7f;
 
    public CrowdController() {}

    private float timeToAppear;
//    private List<PersonController> persons;
    private List<PersonController> fans;
    public int expectedFanCrowdCount;

    void Start() {
        fans = new List<PersonController>();
        expectedFanCrowdCount = fans.Count;
        timeToAppear = 0f;
//        persons = new List<PersonController>();
    }

    void Update () {
        if (timeToAppear <= 0f) {
            timeToAppear = apearingTime;

            if (fans.Count <= maximunNumberOfPersons) {
                SpawnPerson();
            }

        } else {
            timeToAppear -= Time.deltaTime;
        }

        CalculateExpectedFanCrowdSize();
    }

    /**
     * Spawns a person in the streets.
     **/
    private void SpawnPerson() {
        GameObject person;
        PoolingSystem pS = PoolingSystem.Instance;

        float zPos = Random.Range(-5f, -2f);
        float direction = Random.Range(-1f, 1f) > 0f ? 1f : -1f;
        float xPos = -direction * personAppearPosition;

        person = pS.InstantiateAPS("Person", Vector3.zero, Quaternion.identity, this.gameObject);
        //Setting facing direction:
        Vector3 oScate = person.transform.localScale;
        oScate.x = direction * Mathf.Abs(oScate.x);
        person.transform.localScale = oScate;
        //Setting position
        person.transform.position = new Vector3(xPos, 0.89f, zPos);

        //Initializing the controller
        PersonController personController = person.GetComponent<PersonController>();
        personController.walkSpeed *= direction;
        personController.OnReachedInterestPosition += OnPersonReachedInterestPosition;
        personController.Initialize();

//        persons.Add(personController);
    }

    private void CalculateExpectedFanCrowdSize() {
        int newExpectedFanCrowdSize = (int) (maximunNumberOfPersons * scoreManager.EmotionMeter / scoreManager.MaxEmotionMeter);
        while(newExpectedFanCrowdSize < fans.Count && fans.Count != 0) {
            ReduceFanCrowd();
        }
        expectedFanCrowdCount = newExpectedFanCrowdSize;
    }

    private void ReduceFanCrowd() {
        int range = Random.Range(0, fans.Count - 1);
        Debug.Log(range);
        PersonController person = fans[range];
        person.LooseInterest();
        fans.Remove(person);
    }

    private void AddPersonToCrowd(PersonController personController) {
        personController.GetInterested();
        fans.Add(personController);
    }

    private void OnPersonReachedInterestPosition(PersonController personController) {
        if (fans.Count < expectedFanCrowdCount) {
            AddPersonToCrowd(personController);
        }
    }
}
