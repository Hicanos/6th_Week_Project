# 🖥️ 6th_Week_Project
## 6주차 Unity 심화 개인 프로젝트
### 개인 프로젝트 폴더: 6thWeek_JumpUP


## 📖 필수 과제 구현 진행도

- 기본 이동 및 점프 Input System, Rigidbody ForceMode(완료)
- 체력바 UI (완료) - 전부 끝나면 낙하대미지 구현 예정
  
[이동과점프](https://github.com/user-attachments/assets/ff9a429d-3ab6-435f-aee3-9e558145761b)

- 아이템 데이터 ScriptableObject (완료)
- 동적 환경 조사 Raycast UI (완료)

[아이템 데이터 확인](https://github.com/user-attachments/assets/af4f1f31-5e82-4bf6-a0b2-7d566c068cca)


  
- 점프대 Rigidbody ForceMode (완료)
https://github.com/user-attachments/assets/8be0c05f-a0e1-4809-9112-b53c82292f9b



- 아이템 사용 Coroutine (완료)
https://github.com/user-attachments/assets/56f66226-88d6-485c-abe6-62de5297989f


- Dash 기능 추가

https://github.com/user-attachments/assets/68237dc8-ade6-40dd-b093-06d9241af622



## 🤯트러블 슈팅
https://github.com/user-attachments/assets/51c2ae48-1331-4655-b578-5dd53430ecac

### 아이템 습득은 가능하지만(UI에 이미지 반영까지 됨) 아이템이 선택되지 않는 상황   
> SelectItem() 메서드가 제대로 동작하지 않는 것으로 추정됨   
> >확인결과: Itemslot의 Button에 Onclick함수가 할당되지 않아 선택되지 않은 것이었음

![image](https://github.com/user-attachments/assets/d63fa0a9-a1f7-4c55-a27c-2b8bed8bfe26)

### 추가적인 문제: 이번에는 아이템의 할당값이 나타나지 않음(Heatlh, Stamina 등)   
>  dropItem, UseItem 등은 버튼 할당이 되지 않은 것으로 추정됨   
> > 버튼에 클릭 함수를 할당하자, 해결됨.

   
### 추가적인 문제: 아이템에 Index가 할당되지 않은 것을 확인함. -아이템 사용해도 효과가 나타나지 않음!!  
![image](https://github.com/user-attachments/assets/25f6697a-f3d9-4ee4-9af6-83e3d6a2acae)
>
> 버튼도 할당되지 않은 모습을 확인할 수 있음.   
#### 확인 결과: 딱히 위의 내용은 문제될 것없는 내용으로, 실제 데이터로 들어가야할 Consumables에 데이터가 들어가지 않아 발생한 문제였다.   
> >그에따라 consumableData로 명명된 부분은 private로 변경, Consumables에 다시 한 번 더 데이터를 삽입함   

![image](https://github.com/user-attachments/assets/7af410ea-c505-47f6-98fb-13a6188812c1)   
> 실제로 문제가 됐던 부분(데이터를 넣는 곳이 여기였다.)   

https://github.com/user-attachments/assets/adb7e768-cae2-4fe9-a060-b63ec47c31e1
> 남은 문제: 아이템을 사용했을 때, 해당 아이템의 쿨타임만큼 코루틴으로 상호작용이 불가하도록 만들기

> 25.05.22 쿨타임 구현 완료
> 

https://github.com/user-attachments/assets/665484b5-ea7e-4294-a560-d9f7861bbb03

https://github.com/user-attachments/assets/1e30ae3a-2eca-41b1-bb0d-b94174ae5ccd

> 각각의 물건마다 서로 다른 쿨타임을 가지며, 쿨타임이 지속되는 동안에는 UseButton이 비활성화가 됨.   
> 버렸다가 다시 주워도 쿨타임이 초기화 되지 않고 유지됨.
>

### 쿨타임 문제


https://github.com/user-attachments/assets/ff0b36a7-086e-4870-aa06-4913ff518890

### 문제사항1: 쿨타임이 30초, 효과 지속시간이 20초인데 쿨타임이 먼저 종료됨 (쿨타임 종료 후 효과 지속시간의 코루틴이 작동하는 것 같음)   
### 문제사항2: 1의 사유로 쿨타임이 끝난 아이템을 재사용하면, 효과가 중첩되며 가장 최초의 값으로 돌아가지 않음
> (기본값 5, 추가량 5라면, 5>10>15의 순으로 증가 후 시간이 아무리 오래 지나도 10으로 고정됨.)
> 1. 쿨타임 코루틴에 만약 duration이 있으면 같이 작동하도록 수정할 것
> 2. 변경되는 값은 기존값을 그대로 두고, 아이템 회복량(초기값:0)에서 더하며 코루틴이 종료될 때 자신의 회복량(value)값을 제거할 것


### 25.05.23 현재: 2는 해결됨(중첩 적용은 되지만, 각자 시간이 종료되면 아이템 효과가 제거됨)   
> 그러나 여전히 1의 사항이 유지되고 있음.(쿨타임 종료 후 아이템 지속시간이 유지되고 사라짐...)
> 
https://github.com/user-attachments/assets/b11e9804-85d5-4ca9-8261-35edea9e6d88

### 최종 해결


https://github.com/user-attachments/assets/a206c5cd-e1fb-4ec0-9139-87845bdb0251

#### 실제 문제: 쿨타임 감소함수가 3곳에서 동시에 호출됨
> Time.deltaTime 이 3곳에서 동시에 감소시키자, 실제 감소량은 -Time.detlaTime*3이 되어, 쿨타임이 10초밖에 되지않은 것.   
> 불필요한 쿨다운함수는 제거, 쿨타임의 종료여부가 필요한 곳에는 IsReady()를 확인하는 것으로 해결함.   
> 실질적으로 쿨다운은 처음 아이템을 사용했을 때 1회만 작동하도록 만듦.   





https://github.com/user-attachments/assets/284d4914-1294-4f73-b3ab-f7ff9efe79db


> 번외:
> 통나무를 버렸을 때 정상적으로 아이템 정보가 뜨지 않는 현상이 있었음 - ItemData가 프리펩에 할당되지 않아 발생한 문제
> 해당 문제는 Log에 배정될 Data를 인스펙터 창에서 집어 넣는 것으로 해결할 수 있었음.




## 🛠️Commit Convention
> Add:
파일 추가
> 
> Remove :
파일 삭제
> 
> Rename :
파일 혹은 폴더명 수정 혹은 이동
> 
> Feat :
새로운 기능 추가
> 
> Fix : 
버그 수정
> 
> Docs : 
문서 수정
> 
> Style : 
코드 포맷팅, 세미콜론 누락, 코드 변경이 없는 경우
> 
> Refactor : 
코드 리펙토링
> 
> Test : 
테스트(테스트용 디버그 코드 추가, 수정, 삭제 등 기본 로직에 변경이 없는 경우)
> 
> Chore : 
위에 걸리지 않는 기타 변경사항   
> 
> Comment : 
필요한 주석 추가 및 변경   
> 
> Init : 
프로젝트 초기 생성


