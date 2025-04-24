using UnityEngine;

public class BallController : MonoBehaviour
{
    private Spell<MoveBallArgs> forward;
    private Spell<MoveBallArgs> back;
    private Spell<MoveBallArgs> left;
    private Spell<MoveBallArgs> right;
    private Spell<MoveBallArgs> jump;
    private Spell<MoveBallArgs> reset;
    private Spell<MoveBallArgs> fuck;
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
        SpellEventSubscriber.Instance.SubscribeToSpell<MoveBallArgs>(SpellWords.Forward, MoveBall, (spell) => forward = spell);
        SpellEventSubscriber.Instance.SubscribeToSpell<MoveBallArgs>(SpellWords.Back, MoveBall, (spell) => back = spell);
        SpellEventSubscriber.Instance.SubscribeToSpell<MoveBallArgs>(SpellWords.Left, MoveBall, (spell) => left = spell);
        SpellEventSubscriber.Instance.SubscribeToSpell<MoveBallArgs>(SpellWords.Right, MoveBall, (spell) => right = spell);
        SpellEventSubscriber.Instance.SubscribeToSpell<MoveBallArgs>(SpellWords.Jump, MoveBall, (spell) => jump = spell);
        SpellEventSubscriber.Instance.SubscribeToSpell<MoveBallArgs>(SpellWords.Reset, ResetBall, (spell) => reset = spell);
        SpellEventSubscriber.Instance.SubscribeToSpell<MoveBallArgs>(SpellWords.Fuck, Fuck, (spell) => fuck = spell);
    }

    private void OnDisable()
    {
        forward.cast -= MoveBall;
        back.cast -= MoveBall;
        left.cast -= MoveBall;
        right.cast -= MoveBall;
        jump.cast -= MoveBall;
        reset.cast -= ResetBall;
        fuck.cast -= Fuck;
    }

    //REVIEW
    // this might handle Jump in a funky way
    private void MoveBall(MoveBallArgs args)
    {
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 relativeDirection = (camForward * args.direction.z) + (camRight * args.direction.x);

        myRigidbody.AddForce(relativeDirection * args.strength, ForceMode.Impulse);
    }

    private void ResetBall(MoveBallArgs args)
    {
        myRigidbody.MovePosition(startingPos);
    }

    private void Fuck(MoveBallArgs args)
    {
        Vector3 direction = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        
        myRigidbody.AddForce(direction * 100f, ForceMode.Impulse);
    }

}
