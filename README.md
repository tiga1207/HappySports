# VR _개인프로젝트

제목: Happy Sports

설명:  비트세이버류, 날아오는 큐브들을 자르고 점수를 획득하세요

GitHub 주소 : [https://github.com/tiga1207/HappySports](https://github.com/tiga1207/HappySports)

**[ 개발 기간 ]**    2025.06.12 ~ 2025.06.17

**[ 개발 인원 ]**    1명

**[ 개발 환경 ]**   Unity 2022.3.61f1 URP, Visual Studio Code, Oculus Quest 2

**[ 사용 언어 ]**    C#

**[ 담당 역할 ]** 

1. 비트세이버류 게임 로직 구현
2. UI Design
3. Post-Processing
4. Particle을 통한 각종 효과 구현
5. Json Parsing

### 1.UI

### 1.1. 노래 선택 UI
![image](https://github.com/user-attachments/assets/6b8256ec-73fb-4fa1-84a2-75c7d56900ca)

- 비트세이버에서 추출한 맵 데이터를 `info.json`, `EasyStandard.json`등으로 저장한 구조를 사용.
- 각 곡 폴더에 있는 `info.dat`를 JSON 파싱해 `곡 이름`, `작곡가`, `BPM`, `커버 이미지`, `난이도 목록`을 추출하여 스크롤뷰에 표시.
- 곡 선택 시, 원하는 난이도를 클릭 시 하이라이팅되며, 이후 해당 곡과 난이도에 맞는 json 파일을 불러오도록 구현.

### 1.2.  Setting UI
![image 1](https://github.com/user-attachments/assets/6e471c43-696c-42af-b5c0-1f5cf8bea662)

- `SoundManager` 싱글톤에서 `AudioSource.volume`을 직접 조절하도록 연결하여 오디오 볼륨 조절 슬라이더 구현
- 큐브 절단 시 `OnScoreUp` 이벤트를 호출하여 점수를 누적 하여 UI 상단에 실시간 점수를 표시.

### 2. Saber

![image 2](https://github.com/user-attachments/assets/5826bd66-b476-4ff5-a646-10f085159593)

- **잔상 효과**: Saber에는 `TrailRenderer`를 적용해 휘두를 때 잔상이 자연스럽게 남도록 구현함.
- **큐브 자르기**: 두 프레임 간의 방향 벡터(`transform.position - previousPos`)와 `CubeDir.cs`에 정의된 큐브의 잘리는 방향을 비교해서, 일정 각도 이내일 경우에만 절단 성공 판정.
- **진동 피드백**: 큐브를 자르면 `XRController.SendHapticImpulse`로 손 컨트롤러에 짧은 진동 피드백 제공.
- **파티클 이펙트**: 큐브 절단 시 잘린 위치에 파티클(`ParticleSystem`)이 생성되며, 잘린 오브젝트의 위/아래 절단면이 분리되어 날아가게 함 (Ezy-Slice 사용).

### 3. Beat Cube

![image 3](https://github.com/user-attachments/assets/0921d615-18fc-4bcb-975c-ec325927dcdd)


- 해당 노래의 난이도 JSON(`.dat`)을 파싱해 `_time`, `_lineIndex`, `_lineLayer`, `_type`, `_cutDirection`을 읽어 큐브 생성.
- `NoteSpawner.cs`에서 Json에 있는 모든 노트를 시간 순으로 순회하면서 `Time.time` 기준으로 큐브를 스폰.
- 큐브는 좌우 프리팹 각각에 대해 `ObjectPool`을 사용하여 생성/반납 반복.
- `Pool`은 딕셔너리로 관리하여 타입별로 분리되며, 스폰 시 Pull → 사용 후 자동 Return 처리 구조로 구성.
- `info.dat`에서 추출한 BPM을 기준으로 `60 / BPM`으로 비트 단위 시간 계산.
