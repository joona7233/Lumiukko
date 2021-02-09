using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowstormWiggle : MonoBehaviour {

    public enum Move { right, left };
    public Move move;

    [SerializeField] private float speed;

    [SerializeField] private Vector3 pos1;
    [SerializeField] private Vector3 pos2;
   

	// Use this for initialization
	void Start () {
        move = Move.right;
        StartCoroutine(runMe());
	}

    IEnumerator runMe()
    {
        //move = Move.right;

        while (true)
        {
            if(pos1.x > this.transform.position.x)
            {
                // Oikealle.
                move = Move.right;
            }
            else if(pos2.x < this.transform.position.x)
            {
                move = Move.left;
            }

            if (move == Move.right)
            {
                this.transform.position += new Vector3(1 * speed, 0, 0);
                yield return null;
            }

            if (move == Move.left)
            {
                this.transform.position -= new Vector3(1 * speed, 0, 0);
                yield return null;


            }

            yield return null;
        }
    }
}
