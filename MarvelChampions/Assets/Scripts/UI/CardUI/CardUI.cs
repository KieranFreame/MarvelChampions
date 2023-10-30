using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CardUI : MonoBehaviour
{
    public Sprite CardArt { get; set; }
    protected abstract void LoadData();
    protected virtual void Refresh() { }
}
