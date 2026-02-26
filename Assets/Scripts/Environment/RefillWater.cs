using UnityEngine;
using Core;
public class RefillWater : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    void OnCollisionEnter(Collision collision)
        {
            
          Debug.Log("Player has entered the refill water zone!");
            GameManager.Instance.SetWaterLevel(1f);
        }

        /*void OnCollisionExit(Collision collision)
        {
            
            
          Debug.Log("Player has exited the refill water zone!");
            
        }
        */
}
