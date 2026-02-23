using UnityEngine;

public class ClotheItem : ItemAndObject
{
    protected override void OnInteract()
    {
        PlayerState state = playerTransform.GetComponent<PlayerState>();
        PlayerMove move = playerTransform.GetComponent<PlayerMove>();

        if (state != null && !state.isWearingClothe)
        {
            state.isWearingClothe = true;

            if (move != null) move.ChangeToClothedRabbit();

            StageManager.Instance.ShowMessage("Player complete ready to meet Alice!!");
            Destroy(gameObject);
        }
    }
}