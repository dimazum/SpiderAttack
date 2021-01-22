
public interface IHitableBlock
{
    void Hit();
    byte CrackCount { get; set; }
    byte MinPickLvl { get; set; }
}
