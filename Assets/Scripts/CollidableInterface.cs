using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CollidableInterface 
{
    void hit(GameObject Mouse, Audio audioManager, GameUi UIManager);
}
