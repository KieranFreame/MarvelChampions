using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsDeck
{
    private static WeaponsDeck instance;

    public static WeaponsDeck Instance
    {
        get
        {
            instance ??= new WeaponsDeck();
            return instance;
        }
    }

    public List<AttachmentCardData> Weapons { get; private set; }

    public WeaponsDeck()
    {
        Weapons = TextReader.PopulateDeck("ExperimentalWeapons.txt").Cast<AttachmentCardData>().ToList();
        Shuffle();
    }

    public async void RevealTopWeapon()
    {
        AttachmentCardData weapon = Weapons[0];
        Weapons.RemoveAt(0);

        ScenarioManager.inst.EncounterDeck.limbo.Add(weapon);

        AttachmentCard card = CreateCardFactory.Instance.CreateCard(weapon, GameObject.Find("AttachmentTransform").transform) as AttachmentCard;
        await card.OnRevealCard();
    }

    private void Shuffle()
    {
        //Fisher-Yates
        System.Random r = new();
        int n = Weapons.Count;

        while (n > 1)
        {
            int k = r.Next(n);
            n--;
            (Weapons[n], Weapons[k]) = (Weapons[k], Weapons[n]);
        }
    }
}
