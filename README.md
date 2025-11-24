
---
# 2025-02-game-final

Dev Instructions:

- Clone this repository to your workspace
- Download the initial project from [This link](https://drive.google.com/file/d/14PkNCKgChW-yJzDFg0sN7i96q37w8dEj/view?usp=drive_link) and extract all the content to the folder

---


---
## 게임 메커니즘

### 장르 - 로그라이트 (Rouge-lite)

- 매 플레이마다 초기화
- 게임 클리어 시 얻는 재화로 능력치 업그레이드 및 스킬 등 해금 가능

### 파티 시스템

- 마법사 - 메인 캐릭터
- 전사(탱커) - 플레이어 체력
- 사제 - 체력 회복
- 기타 파티원(도적 등)

    게임 시작 전 선택 가능

### 공격 시스템

- 플레이어가 바라보는 방향으로 자동 공격
- 다양한 공격의 스타일을 가진 마법
  - 땅 - 단일 공격에 높은 데미지
  - 화염 - 지속 데미지
  - 번개 - 적 잠시 정지
  - 물 - 방어력 감소
  - 얼음 - 적 속도 감소
- 시작 시 메인 마법 속성 선택
- 마법 자체 업그레이드와 마법에 추가 능력 추가 구별
- 마법 능력치 업그레이드 - 공격력, 공격 속도, 공격 범위, 관통 횟수 등 마법 자체의 능력치 업그레이드
- 보조 스킬 업그레이드 - 적 적중 시 폭발, 적 속도 감소 등 마법 격공과 연계되는 추가 기술
- 스킬 시스템
  - 공격과 별개의 스킬 시스템
  - 쿨타임
  - 전체 데미지 / 공격 속도 증가 / 적 속도 감속 등

### 마법 속성

- 불(Fire) - 지속 데미지
- 물(Water) - 방어력 감소
- 얼음(Ice) - 이속 감소
- 번개(Lightning) - 경직, 연쇄
- 땅(Earth) - 범위 공격(AOE)
- 바람(Wind) - 넉백

### 마법 능력치

- 공격력
- 공격 속도 및 투사체 속도
- 공격 범위
- 관통 횟수

### 마법 보조 스킬

- 적 적중 시 장판 생성
- 적 적중 시 근접한 적 연쇄 공격
- 적 처치 시 범위 공격
- 적 적중 시 체력 회복

### 스킬

- 전체 데미지
- 일정 시간 동안 공격 속도 및 공격력 증가
- 일정 시간 동안 적 속도 감소
- 일정 시간 동안 적 방어력 감소


---
## 목표 및 스토리

용사 파티를 이끌고 마왕을 처치하러 떠나는 스토리

각 챕터 또는 스테이지의 보스를 쓰러뜨려서 마지막에는 마왕을 처치하는 것이 목표


---
## 유니티

- [유니티 에셋스토어](https://assetstore.unity.com/ko-KR)
- [TurboSquid](https://www.turbosquid.com/ko/)
- [Mixamo](https://www.mixamo.com/)


---
### UI


---
### 게임 모델링 및 애니메이션


---
### 배경

스테이지 별 다른 컨셉

오브젝트 및 하늘 설정

1. 초원
2. 숲
3. 사막
4. 용암 지대
5. 마왕성

- [Terrain Sample Asset Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808)

- Object
  - [Low Poly Simple Nature Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-simple-nature-pack-162153)
  - [Low Poly Atmospheric Locations Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-atmospheric-locations-pack-278928)
  - [Free Low Poly Desert Pack](https://assetstore.unity.com/packages/3d/environments/free-low-poly-desert-pack-106709)
  - [Inferno World Free Low Poly 3D Models](https://assetstore.unity.com/packages/3d/environments/fantasy/inferno-world-free-low-poly-3d-models-328402)

- Sky
  - [Farland Skies Cloudy Crown](https://assetstore.unity.com/packages/2d/textures-materials/sky/farland-skies-cloudy-crown-60004)

---

### 용사 파티

- 마법사 (공격)
  - [Battle Wizard Poly Art](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/battle-wizard-poly-art-128097)
  - [Free Wand Pack](https://assetstore.unity.com/packages/3d/props/weapons/3d-items-free-wand-pack-46225) - 3가지 마법사와 지팡이 모델과 모션

- 전사 (방어)
  - [RPG Tiny Hero Duo PBR Polyart](https://assetstore.unity.com/packages/3d/characters/humanoids/rpg-tiny-hero-duo-pbr-polyart-225148)
  - [RPG Hero PBR HP Polyart](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/rpg-hero-pbr-hp-polyart-121480)

- 사제 (회복)

- 무기
  - [Free RPG Weapons](https://assetstore.unity.com/packages/3d/props/weapons/free-rpg-weapons-199738)
  - [Free Cartoon Weapon Pack Mobile VR](https://assetstore.unity.com/packages/3d/props/weapons/free-cartoon-weapon-pack-mobile-vr-23956)
  - [Free Low Poly Fantasy RPG Weapons](https://assetstore.unity.com/packages/3d/props/weapons/free-low-poly-fantasy-rpg-weapons-248405)

- 이동 애니메이션

---

### 몬스터

각 스테이지 별로 메인이 되는 몬스터 존재

1. 초원
    - 슬라임
    - 좀비
    - 스켈레톤
    - 보스 - 거대 슬라임
2. 숲
    - 나무 정령
    - 늑대
    - 보스 - 고대 수호 나무
3. 사막
    - 미라
    - 웜
    - 보스 - 스핑크스
4. 용암 지대
    - 용암 정령
    - 화염 임프
    - 보스 - 용암 거인
5. 마왕성
    - 리빙 아머
    - 마도사
    - 보스 - 마왕

- [Dungeonmason](https://alexkim0415.wixsite.com/dungeonmason) (유료)
- [RPG Monster Duo PBR Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-duo-pbr-polyart-157762) - 슬라임, Turtle Shell
- [Haon SD Creature Pack](https://assetstore.unity.com/packages/3d/characters/creatures/haon-sd-creature-pack-311173#content) - 상자, 토끼, 유령
- [Fuga Spiders with Destructible Eggs and Mummy](https://assetstore.unity.com/packages/3d/characters/creatures/fuga-spiders-with-destructible-eggs-and-mummy-151921#content) - 거미 몬스터
- [Stylized Free Skeleton](https://assetstore.unity.com/packages/3d/characters/creatures/stylized-free-skeleton-298650#reviews) - 스켈레톤
- [Level 1 Monster Pack](https://assetstore.unity.com/packages/3d/characters/creatures/level-1-monster-pack-77703) - 박쥐, 유령, 토끼, 슬라임
- [RPG Monster Partners PBR Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-partners-pbr-polyart-168251) - 상자 몬스터, Beholder
- [Dragon the Soul Eater and Dragon Boar](https://assetstore.unity.com/packages/3d/characters/creatures/dragon-the-soul-eater-and-dragon-boar-77121#content) - 드래곤
- [Mini Legion Lich PBR HP Polyart](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-lich-pbr-hp-polyart-91497) - 리치
- [Dragon for Boss Monster HP](https://assetstore.unity.com/packages/3d/characters/creatures/dragon-for-boss-monster-hp-79398#content) - 보스 드래곤
- [Mini Legion Rock Golem PBR HP Polyart](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-rock-golem-pbr-hp-polyart-94707) - 돌 골렘
- [Mini Legion Grunt PBR HP Polyart](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/mini-legion-grunt-pbr-hp-polyart-98187) - Grunt
- [Monster Minion Survivor PBR Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/monster-minion-survivor-pbr-polyart-269515) - 미니언
- [RPG Monster Buddy PBR Polyart](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-buddy-pbr-polyart-253961) - 버섯, 선인장 몬스터

---

### 마법

공격 속성 별 무기 (지팡이나 마법서 등)

- 속성 별 마법 오브젝트 및 이펙트
  - [Legacy Particle Pack](https://assetstore.unity.com/packages/vfx/particles/legacy-particle-pack-73777)
  - [Particle Pack](https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325)
  - [Polygonal's Low Poly Particle Pack](https://assetstore.unity.com/packages/vfx/particles/polygonal-s-low-poly-particle-pack-118355)
  - [Simple FX Cartoon Particles](https://assetstore.unity.com/packages/vfx/particles/simple-fx-cartoon-particles-67834#content)
  - [Magic Effects Free](https://assetstore.unity.com/packages/vfx/particles/spells/magic-effects-free-247933#content)
  - [Free Quick Effects Vol.1](https://assetstore.unity.com/packages/vfx/particles/free-quick-effects-vol-1-304424)
  - [Effect Textures and Prefabs](https://assetstore.unity.com/packages/vfx/particles/effect-textures-and-prefabs-109031)

- 타격 이펙트

  - [Hit Effects Free](https://assetstore.unity.com/packages/vfx/particles/hit-effects-free-284613)
  - [Hit Impact Effects Free](https://assetstore.unity.com/packages/vfx/particles/hit-impact-effects-free-218385)

---

### 아이템

업그레이드, 무기 등

- [Powerup Particles](https://assetstore.unity.com/packages/vfx/particles/powerup-particles-16458#content)
- [Simple Gems and Items Ultimate Animated Customizable Pack](https://assetstore.unity.com/packages/3d/props/simple-gems-and-items-ultimate-animated-customizable-pack-73764)
