using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour {

  public Text expandText;  // 要扩展的文本
  private int frame = 30;  // 帧数
  private float height = 180;  // 展开文本高度
 
  void Start() {
    Button btn = this.gameObject.GetComponent<Button>();
    btn.onClick.AddListener(OnClick);
    StartCoroutine(Fold());
  }

  IEnumerator Fold() {
    float y = height;
    for (int i = 0; i < frame; ++i) {
      y -= height / frame;
      expandText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, expandText.rectTransform.sizeDelta.x);
      expandText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, y);
      if (i == frame - 1) {
        expandText.gameObject.SetActive(false);
      }
      yield return null;
    }
  }

  IEnumerator Unfold() {
    float y = 0;
    for (int i = 0; i < frame; ++i) {
      y += height / frame;
      expandText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, expandText.rectTransform.sizeDelta.x);
      expandText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, y);
      if (i == 0) {
        expandText.gameObject.SetActive(true);
      }
      yield return null;
    }
  }

  void OnClick() {
    if (expandText.gameObject.activeSelf) {
      StartCoroutine(Fold());
    } else {
      StartCoroutine(Unfold());
    }
  }
}