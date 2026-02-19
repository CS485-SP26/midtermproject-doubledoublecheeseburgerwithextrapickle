using Farming;
using System.Text.RegularExpressions;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Character
{
    public class RaycastSelector : TileSelector
    {

        [SerializeField] float maxDistance = 5f;
        
        private void Update()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
            {
                if(hitInfo.collider.TryGetComponent<FarmTile>(out FarmTile tile))
                {
                    SetActiveTile(tile);
                }
                else
                {
                    SetActiveTile(null);
                }
            }
            else //did not hit with raycast
            {
                SetActiveTile(null);
            }

            

        }
    }
}