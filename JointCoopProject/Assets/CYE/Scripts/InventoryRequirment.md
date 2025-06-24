### 인벤토리 요구사항
 
 active item slot
  -> 활성된 아이템(active item) slot n칸
  -> item 사용을 위한 게이지(6)->논의중
  -> 아이템 신규 획득시 게이지 초기화(최대로)->논의중
  -> 플레이어측에서 아이템 사용 입력을 받으면 사용함(스킬 사용)
  -> 필요 기능: 아이템 획득(대체)/아이템 사용/게이지 누적(논의중)/게이지 초기화(논의중)
 
 passive item(AT/AU) slot
  -> 각 타입(AT(2-3)/AU(1))별 활성 아이템 slot 1칸 씩
  -> 개별 cooldown 계산 필요
  -> 필요 기능: 활성 아이템 교체(등록 및 해제)/활성 item cooldown 계산/아이템 사용
 
 passive item list
  -> passive item list(List<ProtoItemPassive>)
  -> 활성 및 비활성화 된 passive item 목록
  -> 필요 기능: 목록 출력(목록 또는 아이콘)
  
 UI에 대한 내용 -> 별도의 키 할당을 통해 별도의 창을 구성할것인가, 혹은 간략한 UI를 통해 표시할것인지
