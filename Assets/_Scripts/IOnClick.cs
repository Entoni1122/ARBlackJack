using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnClick
{
    public void OnClick();
    public void OnClick(GameObject obj);
    public void OnClick(string name, int score);
}
