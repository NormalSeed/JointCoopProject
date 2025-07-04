using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
   public bool useNextStage = true;
   public string targetSceneName = "";


   
   // OntriggerEnter로 먼저 접속후 
   // 필요한게 뭐가있을까
   
   // playerRoomMovement하고 ? 
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (!other.CompareTag("Player")) return;
      
      var playerMovement = other.GetComponent<PlayerRoomMovement>();
      if (playerMovement != null)
      {
         playerMovement.PrepareForScene();
      }

      if (useNextStage)
      {
         GoToNextScene();
      }
      else if (!string.IsNullOrEmpty(targetSceneName))
      {
         GoToScene(targetSceneName);
      }
   }


   public static void GoToNextScene()
   {
      int currentIndex = SceneManager.GetActiveScene().buildIndex;
      int nextIndex = currentIndex + 1;

      
      // 씬 로드가 필요함. 그리고 음 else 에서 이걸 한번더 해야하나 ? 싶긴해. 
      // 맞긴해 나 못하긴해 
      // ㄹㅇ 맞긴함 개ㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐㅐ 못하긴함.
      // 
      
      if (nextIndex < SceneManager.sceneCountInBuildSettings)
      {
         GameSceneManager.Instance.LoadScene(nextIndex);
      }
   }

   
   // 다음씬으로 넘어가는거 하나 
   // 메인메뉴 가는거 하나하고 
   // 재시작 넣으려나 ? 
   
   public static void GoToScene(string sceneName)
   {
      GameSceneManager.Instance.LoadScene(sceneName);
   }

   // 재시작 기능 넣는다면 넣기. 
   //public static void ReStartScene()
   //{
   //   SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   //}
   
   public static void GoToMainMenu()
   {
      GameSceneManager.Instance.LoadScene(0);
   }
   //private void OnTriggerEnter2D(Collider2D other)
   //{
   //   if (other.CompareTag("Player"))
   //   {
   //      SceneManager.LoadScene("Real_Stage2");
   //   }
   //}
}
