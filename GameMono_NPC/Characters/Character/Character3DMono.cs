/*
script used for raycasts.
3d code for character mono. 
basically just links to character 2d Mono, the important one. 
*/
using UnityEngine;
public class Character3DMono : MonoBehaviour, IExecution
{
    [SerializeField]
    private CharacterMono characterMono2D;
    public CharacterMono GetCharacterMono2d()
    {
        return characterMono2D;
    }
    public void m_Awake()
    {
    }

    public void m_OnEnable()
    {

    }

    public void m_GameExecute()
    {
        // throw new System.NotImplementedException();
        if (gameObject.transform.parent.gameObject.GetComponentsInChildren<CharacterMono>().Length > 0)
        {
            if (gameObject.transform.parent.gameObject.GetComponentsInChildren<CharacterMono>()[0] != null)
            {
                characterMono2D = gameObject.transform.parent.gameObject.GetComponentsInChildren<CharacterMono>()[0];
            }
        }
        else
        {
            Debug.LogError("character 3d mono " + gameObject.name + " is not found in object's parent's first child. Hope that you manually assigned it!");
        }
    }
}