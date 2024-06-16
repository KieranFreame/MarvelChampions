using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

public interface ICharacter
{
    public string Name { get; }
    public CharacterStats CharStats { get; set; }
    public ObservableCollection<IAttachment> Attachments { get; set; }
    public void WhenDefeated();
}
