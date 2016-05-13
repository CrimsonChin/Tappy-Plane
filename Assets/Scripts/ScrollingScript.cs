using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScrollingScript : MonoBehaviour
{
    public Vector2 Speed = new Vector2(2, 2);
    public Vector2 Direction = new Vector2(-1, 0);
    public bool IsLooping = false;
    private IList<Transform> backgroundItems;
    private bool isGameStarted;

    void Start()
    {
        backgroundItems = new List<Transform>();

        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.GetComponent<Renderer>() != null)
            {
                backgroundItems.Add(child);
            }
        }

        backgroundItems = backgroundItems.OrderBy(t => t.position.x).ToList();
    }

    void Update()
    {
        if (!isGameStarted)
        {
            return;
        }

        var movement = new Vector3(Speed.x * Direction.x, Speed.y * Direction.y, 0);

        movement *= Time.deltaTime;
        transform.Translate(movement);

        if (IsLooping)
        {
            Loop();
        }
    }

    void OnEnable ()
    {
        EventManager.Subscribe("GameStarted", HandleGameStarted); 
    }

    void OnDisable()
    {
        EventManager.Unsubscribe("GameStarted", HandleGameStarted); 
    }

    private void HandleGameStarted()
    {
        isGameStarted = true;
    }

    private void Loop()
    {
        var firstChild = backgroundItems.FirstOrDefault();
        if (firstChild == null)
        {
            return;
        }

        if (firstChild.position.x < Camera.main.transform.position.x &&
            firstChild.GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false)
        {
            var lastChild = backgroundItems.LastOrDefault();
            if (lastChild == null)
            {
                return;
            }

            var lastPosition = lastChild.transform.position;
            var lastChildRenderer = lastChild.GetComponent<Renderer>();
            var lastSize = (lastChildRenderer.bounds.max - lastChildRenderer.bounds.min);

            firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);

            backgroundItems.Remove(firstChild);
            backgroundItems.Add(firstChild);
        }
    }
}