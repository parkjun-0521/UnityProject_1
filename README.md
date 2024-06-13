# Skul: The Hero Slayer 기능 모작 

## 개요 
  - Skul의 있는 핵심적인 기능을 구현하였습니다. 
  - 구현한 핵심 기능
    ```
     1. 강화 및 NPC
     2. 아이템 및 셋트 아이템
     3. 인벤토리 기능
     4. 강화 및 아이템 적용 능력치 업그레이드 
     5. 아이템 상점 맵 
     6. 맵 전환 및 클리어 보상
    ```
  - 무료 스프라이트로 형태만 구현, 기능을 중점적으로 구현하려고 하였습니다. 
  
## 구현 

  ### 강화 및 NPC
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/GameLobby.PNG" width="30%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Upgrade.PNG" width="30%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Upgrade2.PNG" width="30%" height="30%" /> 
  
  - 왼쪽 노란 NPC 에게 상호작용을 하면 강화 UI가 등장
  - 강화를 하게 되면 특정 제화를 소모하여 능력치를 업그레이드
  - 현재 강화의 수치를 알려주기 위해 색을 변경한다.

  ### 아이템 및 셋트 아이템 
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/ItemDrop.PNG" width="45%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/PlayerStatus.PNG" width="45%" height="30%" />

  - 파란 NPC는 무기, 빨간 NPC는 아이템을 드랍
  - 각각의 무기와 아이템의 증가된 능력치를 인벤토리 창으로 알려준다.
  - 인벤토리 왼쪽에 현재 먹은 아이템의 셋트 효과를 알려준다.
    - 셋트 효과 구현
      - 아이템에 아이템 id 이외에 셋트 id 를 추가한다.
      - 아이템을 습득 시 인벤토리의 모든 아이템을 순회하여 동일한 셋트 id를 가진 오브젝트가 있는지 판단.
      - 있다고 가정할 시 몇개가 있는지 count로 체크 하여 능력치를 증가시켜 준다. 

## 미구현 

  - 무기 스킬 

## 사용한 기술 
