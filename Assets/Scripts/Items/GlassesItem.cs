using UnityEngine;

public class GlassesItem : ItemAndObject
{
    protected override void OnInteract()
    {
        PlayerState state = playerTransform.GetComponent<PlayerState>();

        if (state != null && !state.isWearingGlasses)
        {
            state.isWearingGlasses = true;
            StageManager.Instance.ShowMessage("Player wearing glasses, So get widen visibility!!");
            Destroy(gameObject);
        }
    }
}