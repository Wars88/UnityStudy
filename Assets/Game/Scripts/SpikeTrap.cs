using UnityEngine;

public class SpikeTrap : Trap
{
    protected override void ActivateTrap(PlayerInvokeCsharp player)
    {
        player.KnockBack();
    }
}
