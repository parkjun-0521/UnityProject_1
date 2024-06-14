# 로그라이크 기능 모작 

## 개요 
  - Skul의 있는 핵심적인 기능을 구현하였습니다. 
  - 구현한 핵심 기능

     [1. 강화 및 NPC](#1.-강화-및-NPC)
     2. 아이템 및 셋트 아이템
     3. 인벤토리 기능
     4. 강화 및 아이템 적용 능력치 업그레이드 
     5. 아이템 상점 맵 
     6. 맵 전환 및 클리어 보상
     7. 보스 패턴
     8. 엔딩 크레딧 
  
  - 무료 스프라이트로 형태만 구현, 기능을 중점적으로 구현하려고 하였습니다.

## 개발 툴 
  - Visual Studio 2022
  - Unity 2022.3.22f1
  
## 구현 
  ### 1. 강화 및 NPC
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/GameLobby.PNG" width="30%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Upgrade.PNG" width="30%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Upgrade2.PNG" width="30%" height="30%" /> 
  
  - 왼쪽 노란 NPC 에게 상호작용을 하면 강화 UI가 등장
  - 강화를 하게 되면 특정 제화를 소모하여 능력치를 업그레이드
  - 현재 강화의 수치를 알려주기 위해 색을 변경한다.
  - Count로 강화의 횟수를 계산, 또한 값이 보존될 수 있도록 PlayerPrefs 를 사용하여 값을 저장하였다.

  - [플레이어 강화 NPC Code](https://github.com/parkjun-0521/UnityProject_1/blob/master/Assets/Script/PlayerStatus.cs)

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
      ```C#
      public void SetItemOption() {
        setItemLogic = GetComponent<SetItem>();
        Dictionary<int, int> countDict = new Dictionary<int, int>();

        // 리스트 안에 있는 각 숫자의 개수를 세기
        foreach (int count in GameManager.instance.setItem) {
            if (countDict.ContainsKey(count)) {
                ++countDict[count];
            }
            else {
                countDict[count] = 1;
            }
        }
        // 아이템 종류와 셋트효과를 모두 초기화 후 다시 적용  
        GameManager.instance.itemSetKey.Clear();
        GameManager.instance.setItemInfo.Clear();

        foreach (var kvp in countDict) {
            Debug.Log(kvp.Key + " " + kvp.Value);
            if (kvp.Value == 1) {
                setItemLogic.Set_1(kvp.Key);
            }
            else if (kvp.Value == 2 || kvp.Value == 3) {
                setItemLogic.Set_2(kvp.Key);
            }
            else if( kvp.Value == 4 || kvp.Value == 5) {
                setItemLogic.Set_4(kvp.Key);
            }
            else if( kvp.Value >= 6) {
                setItemLogic.Set_6(kvp.Key);
            }
        }
      }
      ```
       -  [셋트 능력치 구현 Code](https://github.com/parkjun-0521/UnityProject_1/blob/master/Assets/Script/SetItem.cs)
     
  ### 3. 인벤토리 & 4. 강화 및 아이템 적용 능력치 업그레이드 
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/PlayerStatus.PNG" width="50%" height="30%" />

  - 왼쪽은 셋트효과, 중앙은 아이템 인벤토리, 오른쪽은 능력치를 보여주는 종합 status UI
  - 가운데 인벤토리는 리스트를 사용하여 구현하였습니다.
  - 따라서 아이템을 버리거나 팔 시 아이템이 정리가 되도록 하였습니다.
    - 중앙 인벤토리 구현
      - 아이템을 습득 시 해당 Sprite 와 아이템 고유 id 를 가져옵니다.
      - Sprite 들을 미리 List에 저장 후 
      - 인벤토리는 버튼으로 구현, 버튼의 이미지를 List에 접근하여 각 번호에 맞는 이미지를 가져온다.
        - (UI 인벤토리의 이미지 적용, 아이템의 ID와 리스트의 순서를 맞춰 id로만 Sprite를 접근 ) 
      - 단, 각 아이템은 무슨 아이템인지 구분해야 하므로 해당 고유 id 정보를 같이 저장하고 있습니다. (각 아이템의 효과 정보 접근 가능)  
      - 버리거나 판매 시 List에서 해당 이미지 Sprite 제거, 해당 고유 id로 아이템에 접근하여 능력치 감소  
        - [아이템 버리기 Code](https://github.com/parkjun-0521/UnityProject_1/blob/master/Assets/Script/ItemThrowing.cs)

  - 강화, 아이템, 무기, 셋트효과 의 모든 능력치를 합연산으로 구현 
    - 오른쪽 능력치 구현 
      - 하나의 변수로는 강화 능력치를 적용시킨다는 것이 힘들다는 것을 판단
      - 고유의 기본 체력, 속도, 공격력 값 변수A 를 생성
      - 고유의 값에 능력치 종합을 더해줄 변수B 를 생성
      - 변수B 에 강화, 아이템, 무기, 셋트효과 의 모든 능력치 증가를 연산 후 고유의 변수A 에 값을 대입하는 방식으로 구현
      - 따라서 기본 고유 능력치 변수A 는 변하지 않기 때문에 체력 증감, 속도 증감 등 여러 요인에도 값을 쉽게 적용할 수 있었음 

  ### 5. 아이템 상점 맵 
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Shop2.PNG" width="45%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Shop.PNG" width="45%" height="30%" />

  - 왼쪽은 상점에 있는 회복 포션을 주는 NPC. 상호작용을 하면 일정 hp를 회복시키는 포션을 드랍
  - 오른쪽은 상점 맵에 있는 아이템 상점이다.
    - 해당 아이템을 일정 재화를 소비하여 구매할 수 있다.
    - 옆의 리롤 머신은 재화를 소비하여 현재 있는 아이템 테이블 5개를 랜덤으로 교체할 수 있다. 단, 리롤을 할 수록 리롤의 가격이 증가된다.
    - 리롤 구현
        ```C#
        void ReRoll() {
          // teg가 Item 인것을 다 가져와 비활성화 
          GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
          foreach (GameObject item in items) {
              if (item.GetComponent<ItemManager>().isThrowing == true)
                  continue;
              item.SetActive(false);
          }
          // 이후 다시 아이템을 생성 
          ItemCreate();
          shopItemReRoll.isReRoll = false;
      }
      ```
      - 배열을 사용하여 구현하였다. 
      - 우선 상점 맵에 있는 Item 태그를 가진 모든 아이템을 가져와 리스트에 담아둔다. 
      - 리롤을 할 시 리스트의 모든 아이템을 제거하고 랜덤으로 중복없이 5개의 숫자를 뽑은 후 해당 숫자에 맞는 id 를 가진 아이템으로 교체한다.
      - 단, 이때 문제점으로 캐릭터가 버린 아이템도 사라지는 버그가 있었다. 해당 버그는 플레이어가 한번이라도 주었던 아이템 오브젝트는 bool로 체크하여 해당 bool이 false인 Item 오브젝트만 삭제 후 랜덤 생성하였다. 
  
  ### 6. 맵 전환 및 클리어 보상
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Map.PNG" width="45%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/Stage.PNG" width="45%" height="30%" />

  - 포탈이 총 3종류가 존재 ( 초록 : 무기방, 노랑 : 골드방, 빨강 : 아이템 )
  - 해당 포탈 입장 후 맵을 클리어 하면 중앙에 해당 방에 맞는 아이템이 드랍이됨
    - 랜덤 포탈 입장 및 아이템 드랍 구현
      - 우선 랜덤으로 포탈이 3개중에 중복없이 2개의 포탈이 생성
      - 각 포탈에는 고유의 ID 값이 존재
      - 고유의 ID 값을 씬이 넘어가도 유지하도록 하여 맵을 클리어시 해당 ID를 통해 보상을 결정하는 방식으로 구현하였다.
      ```C#
      if ((Input.GetKeyDown(keyboard.GetKeyCode(KeyCodeTypes.Interaction))) && isPlayerCheck && sceneManager.mapCount < 10) {
            Debug.Log("다음 맵으로 이동합니다.");
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject item in items) {
                item.SetActive(false);
            }
            // 포탈의 ID를 넘겨서 보상을 결정
            GameManager.instance.potalID = this.potalID;
            int sceneCount = ++sceneManager.mapCount;
            switch (sceneCount % 10) {
               //... 해당 값에 맞는 맵으로 이동 
            }
        }
      }
      ```
- 일반 씬 6개, 상점 씬 2개, 보스 씬 2개 로 총 10개의 씬을 구현 
  - 씬의 랜덤 생성 
     - 모든 씬은 일반씬 3개 -> 상점 씬 -> 보스 씬 -> 일반씬 3개 -> 상점 씬 -> 보스 씬 사이클로 돌아간다.
     - 각 씬은 랜덤으로 등장한다. 
     - 씬의 Build Index를 사용하여 랜덤 값을 돌려 방을 무작위로 생성하였다.
     - 또한 이전에 나왔던 방은 중복 확인을 하여 다음에 맵 이동시 등장하지 않도록 구현하였다.
     - 정해진 사이클 로직 
    ```C#
    if ((Input.GetKeyDown(keyboard.GetKeyCode(KeyCodeTypes.Interaction))) && isPlayerCheck && sceneManager.mapCount < 10) {
            Debug.Log("다음 맵으로 이동합니다.");
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject item in items) {
                item.SetActive(false);
            }
            GameManager.instance.potalID = this.potalID;
            int sceneCount = ++sceneManager.mapCount;
            switch (sceneCount % 10) {
                case 1:
                    sceneManager.stageCount++;
                    sceneManager.BasicRoom();
                    break;
                case 2:
                case 3:
                    sceneManager.BasicRoom();
                    break;
                case 4:
                    sceneManager.ShopRoom();
                    break;
                case 5:
                    sceneManager.MiddleBossRoom();
                    break;
                case 6:
                case 7:
                case 8:
                    sceneManager.BasicRoom();
                    break;
                case 9:
                    sceneManager.ShopRoom();
                    break;
                case 0:
                    sceneManager.BossRoom();
                break;
            }
    }
    ```
    - [랜덤 씬 생성 Code](https://github.com/parkjun-0521/UnityProject_1/blob/master/Assets/Script/SceneLoadManager.cs)

  ### 7. 보스 패턴
  <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/BossPattern1.PNG" width="45%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/BossPattern3.PNG" width="45%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/BossPattern4.PNG" width="45%" height="30%" /> <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/BossPattern5.PNG" width="45%" height="30%" />

  - 크게 4개의 패턴을 구현하였다.
  - 각 패턴은 서로 다른 시간을 가지며 랜덤으로 보스가 패턴을 진행한다.
  - 보스의 패턴은 Invoke를 사용하여 재귀함수 형태로 구현하였다.
    - 원리
      - Invoke로 Think()를 실행
      - Think() 에서는 랜덤으로 패턴을 생각하여 실행
      - 각 패턴 실행 후 다시 Think()를 Invoke로 실행을 한다.
      - 플레이어가 죽거나 보스가 죽을때 까지 해당 Invoke를 계속 반복하여 실행하는 방식으로 구현하였다.
    ```C#
    void Stop() {
        if (!gameObject.activeSelf)
            return;
        rigid.velocity = Vector2.zero;
        Debug.Log("패턴 생각");
        Invoke("Think", 1);
    }
    public void Think() {
        patternIndex = Random.Range(0,4);
        if (pastPatternIndex != patternIndex && !enemyDeathCheck) {
            switch (patternIndex) {
                case 0:
                    Pattern1();
                    break;
                case 1:
                    Pattern2();
                    break;
            }
        }
        else 
            Invoke("Think", 1f);
    }
    public void Pattern1() {
        if (enemyId == 10) 
            enemyNiddleBoss1Controller.FireBall();
        pastPatternIndex = 0;
        Invoke("Think", 10);
    }
    public void Pattern2() {
        if (enemyId == 10) 
            enemyNiddleBoss1Controller.Sword();
        pastPatternIndex = 1;
        Invoke("Think", 5);
    }
    ```
    - 각 패턴 
      - 힐 : 초당 50씩 3번에 걸쳐 hp를 회복한다. 
      - 탄막 발사 : 중앙으로 날아가 360도로 터지는 탄막을 생성한다. 
      - 투사체 날리기 : 왼쪽, 오른쪽 랜덤으로 이동하며 반대편으로 표창같은 투사체를 던진다. 
      - 레이저 : 상단에 레이저 스포너에서 레이저를 발사하는 방식
        - 레이저는 오른쪽에서 왼쪽, 왼쪽에서 오른쪽, 무작위 3개중에 하나의 패턴을 실행
      - [보스 패턴 구현 Code](https://github.com/parkjun-0521/UnityProject_1/blob/master/Assets/Script/EnemyNiddleBoss1Controller.cs)

  ### 8. 엔딩 크레딧
   <img src="https://github.com/parkjun-0521/UnityProject_1/blob/master/Image/End.gif" alt="Image Error" width="50%" height="50%" />

  - 애니메이션으로 엔딩 크레딧을 구현
