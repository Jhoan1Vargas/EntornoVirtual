using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using TMPro;

public class VRCamera : NetworkBehaviour
{
    [SerializeField, Range(0.1f, 100f)]
    float rayDistance = 5f;
    [SerializeField]
    LayerMask rayLayerDetection;
    RaycastHit hit;
    [SerializeField]
    Transform reticleTrs;
    [SerializeField]
    UnityEngine.UI.Image loadingImage;
    [SerializeField]
    Vector3 initialScale;
    bool isCounting = false;
    float countdown = 0;
    VRControls vrcontrols;
    TargetButton target;
    Camera m_camera;

    public Player player;
    QuestionController question;
    public int id = 0;
    public bool win = false;

    public Transform XRRig;

    void Awake()
    {
        m_camera = GetComponent<Camera>();
        vrcontrols = new VRControls();
        XRRig = transform.parent.parent;
    }

    void Start()
    {
        if(!IsLocalPlayer)
        {
            m_camera.enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            //GetComponent<Rigidbody>().isKinematic = true;
        }
        GameManager.instance.AddPlayer(this);
    }

    public void StartGame()
    {
        if(IsServer) return;
        if(!IsOwner) return;
        if(id != 1)
            player = transform.Find("/Player2").GetComponent<Player>();
        else
            player = transform.Find("/Player1").GetComponent<Player>();
        player.VRPlayer = this;
        GameManager.instance.VRPlayer = this;
        player.StartPlayer();
        player.MoveToNextStep();
        Debug.Log("start game");
    }

    public IEnumerator Win(int id)
    {
        yield return new WaitForSeconds(0.2f);
        if (IsOwner)
        {
            Transform message = transform.Find("Message");
            message.Find("Panel/Text").GetComponent<TMP_Text>().text = $"Player {id} Win";
            message.gameObject.SetActive(true);

            Time.timeScale = 0;
            StartCoroutine(RestartGame());
        }
    }

    IEnumerator RestartGame()
    {
        StopCoroutine("Win");
        if (IsOwner)
        {
            yield return new WaitForSecondsRealtime(5f);
            Transform message = transform.Find("Message");
            message.gameObject.SetActive(false);
            if (!IsServer)
            {
                //player.StartPlayer();
                player.CurrentPos = 0;
                transform.localPosition = new Vector3(0f, -0.17f, 0f);
                XRRig.position = player.Positions[player.CurrentPos].position;
                player.RestartMoveToNextStep();
            }
            win = false;
            Time.timeScale = 1;
        }
    }

    private void Update()
    {
        if (!win && XRRig.position.x <= GameManager.instance.lastPosition.position.x + 1)
        {
            win = true;
            foreach (VRCamera p in GameManager.instance.players)
            {
                StartCoroutine(p.Win(id));
            }
        }
    }

    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, rayDistance, rayLayerDetection))
        {
            reticleTrs.position = hit.point;
            reticleTrs.localScale = initialScale * hit.distance;
            reticleTrs.Find("Reticle").GetComponent<SpriteRenderer>().color = Color.white;
            reticleTrs.Find("Reticle").GetComponent<Light>().range = 0.3f;
            reticleTrs.rotation = Quaternion.LookRotation(hit.normal);
            if(hit.transform.CompareTag("Button")){
                isCounting = true;
                target = hit.collider.GetComponent<TargetButton>();
                question = target.transform.parent.parent.GetComponent<QuestionController>();
                target.buttonImage.color = new Color(0.4f,0.4f,0.4f);
                loadingImage.fillAmount = 0;
            }else{
                isCounting = false;
                countdown = 0;
                if(target) target.buttonImage.color = Color.white;
                loadingImage.fillAmount = 0;
            } 
        }
        else
        {
            reticleTrs.localScale = initialScale;
            reticleTrs.localPosition = new Vector3(0, 0, 1);
            reticleTrs.localRotation = Quaternion.identity;
            reticleTrs.Find("Reticle").GetComponent<Light>().range = 0;

            isCounting = false;
            countdown = 0;
            if(target) target.buttonImage.color = Color.white;
            reticleTrs.Find("Reticle").GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
            loadingImage.fillAmount = 0;
        }
        if(countdown >= 3) { 
            if(question) question.player = player;
            if(target) target.Action();
        }
        if(isCounting)
        {
            countdown += Time.deltaTime;
            loadingImage.fillAmount = countdown/3f;
            if(target)
            {
                float color = countdown/3f > 0.4f ? countdown/3f : 0.4f;
                target.buttonImage.color = new Color(color, color, color);
            }
        }

    }

    public override void NetworkStart()
    {
        base.NetworkStart();
    } 
}
