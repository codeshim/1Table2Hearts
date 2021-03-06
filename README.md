# 1Table2Hearts

## 기술 스택 
- Unity
- Google Cardboard XR Plugin for Unity
- Photon2, Voice for Photon2
- iOS Build
- Arduino Pysical Computing


## 진행팀 소개   
Table Code : 김혜심(개발), 김보원(아트) 2인으로 이루어진 팀으로 "책상으로 대표되는 가장 작은 곳에서부터 생겨나는 큰 변화"를 지향합니다.


## 내용 소개  
‘심장이 뛴다’ 라는 표현은 우리에게 아주 익숙한 표현입니다. 우리는 주로 감정적으로 동요되는 일들, 즉 사랑하는 사람을 만나거나 새로운 사람에게 설레는 감정을 느낄 때 흔하게 쓰고 있습니다. 하지만 한편으로 심장이 뛰는 현상은 살아있는 생명체라면 공통적으로 나타나는 매우 자연스럽고 당연한 현상이기도 합니다. <1Table2Hearts>는 이렇듯 ‘심장이 뛴다’라는 단순한 표현이 가진 여러가지 뉘앙스에서 출발한 프로젝트입니다.  

<1Table2Hearts>는 가상현실 속에서 사람들이 어떻게 서로를 신뢰하고 소통할 수 있는가라는 고민을 중심으로 발전해나갔습니다. 그 고민의 결과로 껴안거나 몸을 기대는 아주 가까운 사이가 아니라면 느끼기 어려운 상대방의 심장박동을 이 컨텐츠를 경험하는 주된 방법으로 채택했습니다. 이 컨텐츠를 통해 사용자가 느낄 나와 상대방의 존재감은 여타 소셜 VR에서 차용하고 있는 사람이나 동물과 같은 구체적인 형상을 통해서가 아닌, 나와 상대방으로부터 실시간으로 수집하는 실제의 심장박동 데이터를 기반으로 뛰고있는 심장을 통해 느끼게 됩니다. 또한 <1Table2Hearts>에서 경험하게 되는 가상 데이트의 무대는 두 사람의 심장박동 수가 일치하는 순간마다 이벤트가 생성됩니다. 이를테면 두 사람의 보트가 그 순간에만 앞으로 나아간다던지, 함께 만들고 있는 모빌의 파츠가 생성된다던지 하는 방식입니다.  

지금까지 작업한 버전들 이외에 두 사용자가 서로의 심장박동을 느끼고 심박수가 일치하는 경험을 한다는 큰 주제 안에서 함께 좋아하는 요리를 만들거나 게임을 하는 버전들로의 확장 가능성이 무한합니다. <1Table2Hearts>는 사용자들이 일상에서 해볼 수 없는 색다른 만남을 경험할 수 있다는 점 외에도 IoT의 신체 데이터 수집 목적이 건강관리를 위한 것 뿐만 아니라 감성적인 영역으로의 확장 가능성을 제시한다는 점에서 큰 의미를 가집니다.  

  
## 담당 태스크 소개  
- **Cardboard XR 플러그인 활용과 iOS Build**  
iPhoneX에서 VR을 테스트해볼 수 있는 개발환경을 구축했고 Cardboard XR 플러그인을 활용하여 VR 테스트 빌드에 성공했습니다. 
<div>
<img width="1000" alt="iOSVRBuild" src="https://user-images.githubusercontent.com/76104907/102378479-34d21a80-4009-11eb-8250-a8c1092408ac.png">  
</div>

- **기존의 Retical Pointer를 개조하여 만든 GazeButton**  
모바일 환경을 타겟으로 하고 있기 때문에 별도의 컨트롤러 없이 가상공간 속 요소들과 인터랙션하기 위해 GazeButton을 구현했습니다. GazeButton을 구현하는 과정에서 Google VR SDK의 Retical Pointer 쉐이더와 코드를 일정시간 응시하면 게이지바가 차도록 개조하여 사용했습니다.  
<div>
<img width="1000" alt="GazeButton" src="https://user-images.githubusercontent.com/76104907/102379643-67c8de00-400a-11eb-95ba-3e1bc9ef28db.png">  
</div>

- **Photon2와 Voice for Photon2를 이용한 네트워크 구현과 음성채팅**  
Photon2 Network와 Voice for Photon2 플러그인을 사용하여 두 사용자가 가상공간 속에서 만나서 대화할 수 있는 환경을 만들었습니다. Photon View Serialize를 통해 서로의 심장이 뛰는 순간들을 전송하여 동기화합니다.  
<div>
<img width="1000" alt="Photon" src="https://user-images.githubusercontent.com/76104907/102379306-17ea1700-400a-11eb-8d70-8c800611388e.png">
</div>

- **C# 언어와 Unity Editor로 사용자가 체험하게 되는 전반적인 경험의 연출과 조작 구현**  
사용자가 구현한 환경 속에 자연스럽게 동화되기 위해 lerp나 Coroutine을 사용한 연출 스크립트를 작성했습니다. 카메라의 시선 가운데에서부터 쏘는 Ray가 트리거가 되어 3D공간 속에서 다른 요소들과 문제없이 인터랙션할 수 있도록 구현했습니다.  
<div>
<img width="1000" alt="BoatStateControllerClass" src="https://user-images.githubusercontent.com/76104907/102379241-03a61a00-400a-11eb-9d61-3556ae2288d3.png">
</div>

- **Arduino Physical Computing을 통한 심박센서 데이터 수집 설계와 납땜**  
사용자의 실시간 심박수를 수집하기 위해 아두이노와 심박센서를 이용하여 회로를 설계하고 회로의 안정성을 위해 납땜을 했습니다.  
<div>
<img width="1000" alt="ArduinoSensor" src="https://user-images.githubusercontent.com/76104907/102379154-effab380-4009-11eb-9d85-ef9d591fa1d0.png">
</div>



## 프로젝트 진행 기간   
2020.09.01 - 2020.12.04


## 프로젝트 데모 링크   
[2Boat2Hearts](https://youtu.be/F6lRyNzcZys)  
[1Table2Hearts_ver.2](https://youtu.be/bcYr38O3FBk)  
[1Table2Hearts_ver.1](https://youtu.be/kgvXnQ1a2ls)  
