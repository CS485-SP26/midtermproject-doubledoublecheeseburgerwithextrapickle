using Farming;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    // ************** RADIAL SELECTOR **************
    /* Although I am sure the logic for what I call "radial selector"
     * is truly named something else, I chose this name because it
     picks the tile closest to the player within a certain radius.
    
    This AI-peer tile selector defines a radius around the player 
    using a invisible box below the player as a reference to their position.
    Then, the radialSelector will call "OverlapSphere" within a certain radius
    around the box to detect any tiles that it is in contact with. It will
    add these tiles to a list, and then calculate the tile closest to the player,
    selecting it. I would have like to been able to select several tiles at once
    to emulate a sort of power up or improved tool that can water or till
    several tiles
    */
    public class RadialSelector : TileSelector
    {
        [SerializeField] float radius = 1.5f;
        

        private void Update()
        {
            FarmTile closest = null;
            float closestDistance = float.MaxValue;

            Vector3 center = transform.position;

            Collider[] hits = Physics.OverlapSphere(center, radius);


            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<FarmTile>(out FarmTile tile))
                {
                    float distance = Vector3.Distance(center, tile.transform.position);
                    if (distance < closestDistance)
                    {
                        closest = tile;
                        closestDistance = distance;
                    }
                }
            }

            if (closest == null || closestDistance > radius)
            {
                closest = null;
            }
            SetActiveTile(closest);

        }
    }
}