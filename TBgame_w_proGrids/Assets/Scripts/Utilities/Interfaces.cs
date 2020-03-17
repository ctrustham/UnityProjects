using UnityEngine;
using System.Collections;
namespace SA {
    //allows a player to select something (ie a character)
    public interface ISelectable // must be put on an object that is a mono-behaviour -> grid character
    {
        void OnSelect(PlayerHolder player);
    }
    public interface IDeselect
    {
        void OnDeselect(PlayerHolder player); // probably dont need the (PlayerHolder player) but pass it anyway
    }
    public interface IHighlight
    {
        void OnHighlight(PlayerHolder player);
    }
    public interface IDeHighlight
    {
        void OnDeHighlight(PlayerHolder player);
    }

    //detected by mouse
    public interface IDetectable{
        Node OnDetect();
    }
}
