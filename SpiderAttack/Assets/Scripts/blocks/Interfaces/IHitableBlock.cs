
public interface IHitableBlock
{
    void Hit();
    byte CrackCount { get; set; }
    byte MinPickLvl { get; set; }
    bool IsGround { get; set; }
}
