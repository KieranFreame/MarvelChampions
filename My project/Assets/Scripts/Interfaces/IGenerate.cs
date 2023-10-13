using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerate
{
    public bool CanGenerateResource(ICard cardToPlay);
    public List<Resource> GenerateResource();
    public bool CompareResource(Resource resource);
    public int ResourceCount() { return 1; }
}
