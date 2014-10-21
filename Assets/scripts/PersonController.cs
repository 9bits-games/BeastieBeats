using UnityEngine;
using System.Collections;

public class PersonController : MonoBehaviour9Bits {

    // Triggered when the emotionometer reaches zero.
    public delegate void ReachedInterestPosition(PersonController personController);
    public event ReachedInterestPosition OnReachedInterestPosition;

    public float walkSpeed = 0.1f;
    public float interestPosition;
    public bool isInterested;
    public float distanceToDispose;
    public Texture walkingSS;
    public Texture ApplauseSS;

    public void PickRandomInterestPosition(float min, float max) {
        interestPosition = Random.Range(min, max);
    }

    public void GetInterested() {
        isInterested = true;
        changeSpriteSheet(ApplauseSS);
    }

    public void LooseInterest() {
        isInterested = false;
        changeSpriteSheet(walkingSS);
    }

    private void changeSpriteSheet(Texture texture, bool play = true) {
        renderer.material.SetTexture("_MainTex", texture);
        SpriteSheetNG sprite = GetComponent<SpriteSheetNG>();
        sprite.Stop();
        if(play) sprite.Play();
    }

    private float distanceToInterestPosition;

    public void Initialize () {
        if (interestPosition <= 0f) {
            PickRandomInterestPosition(3f, 9f);
        }

        if (distanceToDispose <= 0f) {
            distanceToDispose = 14f;
        }

        distanceToInterestPosition = interestPosition;
        LooseInterest();
    }

    void Update () {
        if (!isInterested) {
            float absWalkSpeed = Mathf.Abs(walkSpeed);
            transform.position += new Vector3(walkSpeed, 0f, 0f);

            if (distanceToInterestPosition > 0) {
                distanceToInterestPosition -= absWalkSpeed;

                if (distanceToInterestPosition <= 0) {
                    if (OnReachedInterestPosition != null) {
                        OnReachedInterestPosition(this);
                    }
                }
            }

            distanceToDispose -= absWalkSpeed;
            if(distanceToDispose <= 0) this.Destroy();
        }
    }

    void Destroy() {
        interestPosition = 0f;
        distanceToDispose = 0f;
        OnReachedInterestPosition = null;
        this.gameObject.DestroyAPS();
    }

    //TODO: Implement onDisable
}
