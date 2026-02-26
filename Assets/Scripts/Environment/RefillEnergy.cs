using UnityEngine;
using Core;

public class RefillEnergy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
        {
            
          Debug.Log("Player has entered the refill energy zone!");
            GameManager.Instance.SetEnergyLevel(100f);
        }

        /*void OnCollisionExit(Collision collision)
        {
            
            
          Debug.Log("Player has exited the refill energy zone!");
            
        }
        */
}
