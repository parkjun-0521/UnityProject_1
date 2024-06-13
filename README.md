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

  ### 1. 강화 및 NPC
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/GameLobby.PNG" width="30%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Upgrade.PNG" width="30%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Upgrade2.PNG" width="30%" height="30%" /> 
  
  - 왼쪽 노란 NPC 에게 상호작용을 하면 강화 UI가 등장
  - 강화를 하게 되면 특정 제화를 소모하여 능력치를 업그레이드
  - 현재 강화의 수치를 알려주기 위해 색을 변경한다.

  ### 2. 아이템 및 셋트 아이템 
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/ItemDrop.PNG" width="45%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/PlayerStatus.PNG" width="45%" height="30%" />

  - 파란 NPC는 무기, 빨간 NPC는 아이템을 드랍
  - 각각의 무기와 아이템의 증가된 능력치를 인벤토리 창으로 알려준다.
  - 인벤토리 왼쪽에 현재 먹은 아이템의 셋트 효과를 알려준다.
    - 셋트 효과 구현
      - 아이템에 아이템 id 이외에 셋트 id 를 추가한다.
      - 아이템을 습득 시 인벤토리의 모든 아이템을 순회하여 동일한 셋트 id를 가진 오브젝트가 있는지 판단
      - 있다고 가정할 시 몇개가 있는지 count로 체크 하여 능력치를 증가시켜 준다.
      - 아이템을 버리거나 팔 때도 모든 아이템을 순회하여 셋트 아이템이 각 몇개있는지 판단 후 능력치 적용
      - 자료구조는 리스트를 사용하여 구현하였다.
     
  ### 3. 인벤토리 & 4. 강화 및 아이템 적용 능력치 업그레이드 
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/PlayerStatus.PNG" width="50%" height="30%" />

  - 왼쪽은 셋트효과, 중앙은 아이템 인벤토리, 오른쪽은 능력치를 보여주는 종합 status UI
  - 가운데 인벤토리는 리스트를 사용하여 구현하였습니다.
  - 따라서 아이템을 버리거나 팔 시 아이템이 정리가 되도록 하였습니다.
    - 중앙 인벤토리 구현
      - 아이템을 습득 시 해당 Sprite 와 아이템 고유 id 를 가져옵니다.
      - Sprite 들을 List에 저장 후 
      - 인벤토리는 버튼으로 구현, 버튼의 이미지를 List에 접근하여 각 번호에 맞는 이미지를 가져온다. (UI 인벤토리의 이미지 적용) 
      - 단, 각 아이템은 무슨 아이템인지 구분해야 하므로 해당 고유 id 정보를 같이 저장하고 있습니다. (각 아이템의 효과 정보 접근 가능)  
      - 버리거나 판매 시 List에서 해당 이미지 Sprite 제거, 해당 고유 id로 아이템에 접근하여 능력치 감소  


  - 강화, 아이템, 무기, 셋트효과 의 모든 능력치를 합연산으로 구현 
    - 오른쪽 능력치 구현 
      - 하나의 변수로는 강화 능력치를 적용시킨다는 것이 힘들다는 것을 판단
      - 고유의 기본 체력, 속도, 공격력 값 변수A 를 생성
      - 고유의 값에 능력치 종합을 더해줄 변수B 를 생성
      - 변수B 에 강화, 아이템, 무기, 셋트효과 의 모든 능력치 증가를 연산 후 고유의 변수A 에 값을 대입하는 방식으로 구현
      - 따라서 기본 고유 능력치 변수A 는 변하지 않기 때문에 체력 증감, 속도 증감 등 여러 요인에도 값을 쉽게 적용할 수 있었음 
  
## 미구현 

  - 무기 스킬 

## 사용한 기술 