public abstract class Reward_Action : Reward
{
    public Reward_Action()
    {
        rewardType = RewardType.ACTION;
        string_RewardType = rewardType.ToString();
    }
    public abstract override bool Rewarding();
}
