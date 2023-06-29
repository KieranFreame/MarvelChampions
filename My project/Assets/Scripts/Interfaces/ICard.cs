
using UnityEngine;

public interface ICard
{
    public string CardName { get; }
    public string CardDesc { get; }
    public Zone CurrZone { get; set; }
    public Zone PrevZone { get; set; }
    public bool InPlay { get; set; }
    public bool FaceUp { get; set; }
}
