public abstract class Reward_Effect : Reward
{
    public Reward_Effect()
    {
        rewardType = RewardType.EFFECT;
        string_RewardType = rewardType.ToString();
    }
    public abstract override bool Rewarding();
}
