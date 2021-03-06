using UnityEngine;

public class GrapplingGun : MonoBehaviour {
    
    public LayerMask whatIsGrappleable;
    public Transform gunTip, _camera, player;
    [SerializeField] private KeyCode grappleKey = KeyCode.E;
    [SerializeField] private KeyCode grappleJumpKey = KeyCode.Q;
    private Vector3 grapplePoint;
    private float maxDistance = 50f;
    private SpringJoint joint = null;

    [SerializeField] private float spring = 4f;
    [SerializeField] private float damper = 7f;
    [SerializeField] private float massSalce = 4.5f;
    [SerializeField] private float grapleJumpPower = 50f;


    void Update() {
        if (Input.GetKeyDown(grappleKey) && !IsGrappling()) {
            StartGrapple();
        }
        else if (Input.GetKeyUp(grappleKey)) {
            StopGrapple();
        }

        if (Input.GetKeyDown(grappleJumpKey) && !IsGrappling()){
            StartGrappleJump();
        }
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = spring;
            joint.damper = damper; 
            joint.massScale = massSalce;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        Destroy(joint);
    }

    void StartGrappleJump() {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            Vector3 direction = hit.point - player.transform.position;

            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Rigidbody>().AddForce(direction * grapleJumpPower, ForceMode.VelocityChange);

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = spring;
            joint.damper = damper; 
            joint.massScale = massSalce;

            Destroy(joint, .1f);
        }
    }


    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}
