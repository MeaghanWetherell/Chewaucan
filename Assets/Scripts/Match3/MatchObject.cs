using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3
{
    public class MatchObject : MonoBehaviour
    {
        private static List<Sprite> sprites;

        private static float gravity = 10;
    
        public int myType;

        public int index;

        public MatchLine parent;
        
        private void Start()
        {
            if (sprites == null)
            {
                sprites = Resources.Load<SpriteCollection>("Match3Sprites").spriteList;
            }
            myType = Random.Range((int) 0, (int) sprites.Count);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[myType];
        }

        private void Update()
        {
            Vector3 snapPoint = parent.snapPoints[index].position;
            if (this.transform.position != snapPoint)
            {
                this.transform.position = Vector3.MoveTowards(transform.position, snapPoint,
                    gravity * Time.deltaTime);
            }
        }

        public void remove()
        {
            Destroy(this.gameObject);
        }
    }
}
