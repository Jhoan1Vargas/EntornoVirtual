using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCameraLocal : MonoBehaviour
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

  void Awake()
  {
    vrcontrols = new VRControls();
  }

  void Start()
  {
    reticleTrs.localScale = initialScale;
    vrcontrols.Gameplay.VRClick.performed += _=> ClickOverObject(); 
  }

  void ClickOverObject()
  {
    
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
        if(target) target.Action();
        gameObject.SetActive(false);
    }
    if(isCounting){
        countdown += Time.deltaTime;
        loadingImage.fillAmount = countdown/3f;
        if(target)
        {
            float color = countdown/3f > 0.4f ? countdown/3f : 0.4f;
            target.buttonImage.color = new Color(color, color, color);
        }
    } 
  }
}
