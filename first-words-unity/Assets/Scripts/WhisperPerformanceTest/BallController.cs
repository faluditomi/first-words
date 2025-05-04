using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody myRigidbody;
    private Transform camTransform;
    private Vector3 startingPos;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        camTransform = FindFirstObjectByType<Camera>().transform;
    }

    private void Start()
    {
        startingPos = myRigidbody.position;
    }

    private void OnEnable()
    {
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Forward, MoveBall);
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Back, MoveBall);
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Left, MoveBall);
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Right, MoveBall);
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Jump, JumpBall);
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Stop, StopBall);
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Reset, ResetBall);
        SpellEventSubscriber.Instance().SubscribeToSpell(SpellWords.Fuck, Fuck);
    }

    // private void OnDisable()
    // {
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Forward, MoveBall);
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Back, MoveBall);
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Left, MoveBall);
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Right, MoveBall);
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Jump, JumpBall);
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Stop, StopBall);
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Reset, ResetBall);
    //     SpellEventSubscriber.Instance().UnsubscribeFromSpell(SpellWords.Fuck, Fuck);
    // }

    private void MoveBall(SpellArgs args)
    {
        MoveBallArgs myArgs = SessionSpellCache.GetSpellArgs<MoveBallArgs>(args);

        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 relativeDirection = (camForward * myArgs.direction.z) + (camRight * myArgs.direction.x);

        myRigidbody.AddForce(relativeDirection * myArgs.strength, ForceMode.Impulse);
    }

    private void JumpBall(SpellArgs args)
    {
        MoveBallArgs myArgs = SessionSpellCache.GetSpellArgs<MoveBallArgs>(args);

        myRigidbody.AddForce(myArgs.direction * myArgs.strength, ForceMode.Impulse);
    }

    private void StopBall(SpellArgs args)
    {
        myRigidbody.linearVelocity = Vector3.zero;
        myRigidbody.angularVelocity = Vector3.zero;
    }

    private void ResetBall(SpellArgs args)
    {
        StopBall(args);
        myRigidbody.MovePosition(startingPos);
    }

    private void Fuck(SpellArgs args)
    {
        Vector3 direction = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        
        myRigidbody.AddForce(direction * 70f, ForceMode.Impulse);
    }

}
