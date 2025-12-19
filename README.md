Unity 기반 3인칭 액션 게임 프로젝트

## ✨ 주요 기능
  
- 🎮 Unity Input System 기반 플레이어 입력
- 🎥 Cinemachine 3인칭 카메라
- ⚔️ 애니메이션 기반 전투 시스템
- 🤖 NavMesh AI 적 시스템
  
## 🎯 핵심 시스템
  
### Input System
**`PlayerInputHandler`** - Unity Input System 입력 처리
- 이동, 시점, 줌, 스프린트, 회피, 공격
  
### Character System
**`CharacterStats`** - 캐릭터 스탯 관리 (베이스 클래스)
- HP 시스템, 데미지/힐링, 사망 처리
  
**`CharacterAttack`** - 공격 시스템 (추상 클래스)
- 콜라이더 기반 공격, 애니메이션 이벤트 연동
  
### Player
**`PlayerAttack`** - 플레이어 공격 구현
  
### Enemy System
**`EnemyStats`** - 적 스탯 및 NavMesh 이동
**`EnemyAttack`** - 적 공격 구현
**`EnemySpawnTrigger`** - 트리거 기반 적 스폰
  
## 🎮 조작법
  
| 키 | 기능 |
|---|---|
| WASD | 이동 |
| 마우스 | 시점 회전 |
| 마우스 휠 | 줌 |
| Shift | 스프린트 |
| Space | 회피 구르기 |
| 좌클릭 | 공격 |
| E | 적 스폰 (테스트) |

## 🚀 시작하기
https://phikozz.itch.io/punchpunch
  
## 🎮 게임 플레이 영상

[![Gameplay](https://img.youtube.com/vi/JYz7H6ehIl8/maxresdefault.jpg)](https://www.youtube.com/watch?v=JYz7H6ehIl8)

*이미지를 클릭하면 게임 플레이 영상을 시청할 수 있습니다.*
