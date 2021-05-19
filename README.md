# Zombie_VR

# 프로젝트 개요
VR을 이용한 공포 게임 만들기

# 프로젝트 상세

## VR을 이용한 공포 슈팅 게임 

## 게임은 총 3개의 난이도가 존재하며, 난이도가 높을수록 몹의 체력, 공격력, 속도가 증가합니다.
![캡처1](https://user-images.githubusercontent.com/14048756/118852991-413b0600-b90e-11eb-9e1f-044f66cee70e.PNG)  
  
## 설정창에서는 소리 조절과 총의 Aim활성화, UI활성화 버튼이 있습니다. 설정값과 거리에 따라 괴물의 소리가 들립니다.
![캡처](https://user-images.githubusercontent.com/14048756/118852988-40a26f80-b90e-11eb-9cbf-340d45f096e0.PNG)  

## 무기는 권총과 돌격소총이 있으며 돌격소총의 경우 연사속도가 매우 빠르며 Aim설정을 킨경우 두손으로 집어야만 Aim이 보입니다.  
![캡3처](https://user-images.githubusercontent.com/14048756/118852967-3d0ee880-b90e-11eb-808f-cb04b0a1a135.PNG)

## 무기를 집을경우 무기의 정보값을 받아 UI에서 총알의 갯수를 표현합니다.
## 허벅지에는 무기만, 머리에는 모자만 장착할 수 있게 구현하였습니다. 머리에 쓴다는것을 표현하기위해 Mesh를 표현.
![1123](https://user-images.githubusercontent.com/14048756/118852972-3e401580-b90e-11eb-8756-0c0abc68f194.PNG)  

## 카운트 다운이 지날시 문이 열리고 열쇠를 찾아 집으로 들어가면 1스테이지를 클리어합니다.
![캡처g](https://user-images.githubusercontent.com/14048756/118854377-a2afa480-b90f-11eb-8551-107a26e8d8ce.PNG)

## 괴물들은 총알의 궤적을 Line Render로 표현했으며, 맞은 위치에 피가나오는 Blood Effect를 구현하였습니다.
 ![캡f처](https://user-images.githubusercontent.com/14048756/118854392-a6432b80-b90f-11eb-8b09-508db7efeb00.PNG)  
 
## 좀비는 속도에 따라 이동 애니메이션이 다릅니다. 부위별 피격이 존재하며 머리가 파괴될시 사망, 팔이 파괴될시 공격력 감소, 다리가 파괴될시 속도가 감소합니다. 또한 다른 부위가 파괴되기 이전에 총 체력이 0이되면 사망합니다.
![캡처44](https://user-images.githubusercontent.com/14048756/118854366-a04d4a80-b90f-11eb-93d2-862e88930b05.PNG)

## 열쇠를 얻기위해 등산을 해야합니다. 
![aaa캡처](https://user-images.githubusercontent.com/14048756/118852980-3f714280-b90e-11eb-9a6d-b29bb3baf892.PNG)  

## 집 근처에는 좀비들이 몰려있으며 일정하게 랜덤위치로 느릿느릿하게 이동합니다.
## 총알 또는 플레이어가 다가갈시 소리를 듣고 쫓아옵니다.
![캡333처](https://user-images.githubusercontent.com/14048756/118854395-a6dbc200-b90f-11eb-8ba3-75b3d46fbd37.PNG)  

## 스테이지2 에서는 이동을 금지 시켰습니다. 다가오는 적들을 쏘아 목적지까지 도착하면 됩니다.
![dddd캡처](https://user-images.githubusercontent.com/14048756/118854382-a3e0d180-b90f-11eb-863a-3f04b6c368f8.PNG)  

## 스테이지3는 주변에 총 4마리의 고블린이 있습니다. 기본적으로 체력과 공격력이 높습니다.
![d캡처](https://user-images.githubusercontent.com/14048756/118854389-a511fe80-b90f-11eb-9b30-bfa0f02b056c.PNG)  

## 피격시 피격효과가 나오며 플레이어의 체력이 0이되면 패배합니다

## 맨 윗쪽 탑 고블린은 공격후에 다른 위치로 이동을 하며 공격합니다. 또한 고블린의 화살 장전 애니메이션에 따라 화살의 위치가 실시간으로 바뀌도록 구현하였습니다.


# 개발 환경
개발 도구: vscode, visual stdio, Unity   

