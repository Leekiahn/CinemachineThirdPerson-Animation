# CinemachineThirdPerson & Animation
## 🎥 데모 영상
[![프로젝트 데모](https://img.youtube.com/vi/SvJ4OtsqqB4/maxresdefault.jpg)](https://youtu.be/SvJ4OtsqqB4)

## ✨ 구현 기능

### 🎮 입력 시스템
- Unity Input System 기반 입력 관리
- 이동, 다이브롤, 스프린트, 시점 회전, 줌 입력 처리

### 🏃 플레이어 이동
- Root Motion 기반 물리 이동
- 부드러운 블렌드 애니메이션
- 스프린트 기능
- 다이브롤 기능

### 📷 카메라 컨트롤
- Cinemachine 기반 3인칭 카메라
- 마우스 시점 회전
- 줌 인/아웃 기능

### 🎭 애니메이션
- Root Motion 기반 애니메이션
- 이동 방향별 블렌드 애니메이션
- 다이브롤 애니메이션
- 착지 애니메이션
- 지면 감지 시스템

### 🔊 사운드 시스템
- 걷기/달리기 발자국 소리 (랜덤 재생)
- 다이브롤 효과음 및 음성
- 착지 효과음 및 음성
- 애니메이션 이벤트 기반 사운드 재생

## 🛠️ 기술 스택
- Unity 2022.3 LTS
- C# 9.0 / .NET Framework 4.7.1
- Unity Input System
- Cinemachine
- Root Motion Animation
- Physics-based Movement (Rigidbody)

## 🎮 조작법
- **WASD**: 이동
- **Shift**: 스프린트
- **Space**: 다이브롤
- **Mouse**: 시점 회전
- **Mouse Wheel**: 줌

## 📂 프로젝트 구조  
Player GameObject  
├── PlayerInputHandler (입력 관리)  
├── PlayerMovement (이동 처리)  
├── PlayerCameraController (카메라 제어)  
├── PlayerAnimationController (애니메이션 제어)  
├── Rigidbody (물리) └── Animator (애니메이션)  
