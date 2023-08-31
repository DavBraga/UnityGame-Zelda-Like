# Dungeon Fall
[PT-BR Readme](readme_PT_BR.md)

Game on itch.io: [Dungeon Fall on itch.io](https://bragadavi.itch.io/dungeon-fall)

Game Code Version on Git: 0.5.1

## About

Dungeon Fall is a game developed in the Unity Engine as the final project for the [Unity 3D Game Developer Course](https://www.dio.me/curso-unity-3d) offered by DIO.me.

The project has been delivered and certified, meeting the course completion requirements. [Check the certificate here](https://www.dio.me/certificate/94C79951/share).

This game pays homage to classics like Zelda, Brave Fencer Musashiden, and Alundra, featuring a top-down camera perspective that aims to bring elements of exploration, combat, power-ups, cinematics, valuable treasures, and hidden secrets within a dungeon filled with mysteries.

### Player Actions

- Move around,
- Attack enemies,
- Jump,
- Use consumables,
- Interact with nearby objects,
- Utilize the bomb tool to destroy obstacles.

## Additional Development

Although intended as a showcase of technical capabilities as a game developer, programmer, and Game Designer, I aimed to deliver a complete experience with a clear beginning, middle, and end. To achieve this, additional features were gradually incorporated. Several enhancements have been introduced to enrich the game. Here's a list of what's included in this project.

### Technical Features

- Camera system using CineMachine for top-down view.
- Character movement with custom physics.
- Support for gamepads and virtual controls.
- Ported to PC Windows, Browser (WebGL), and Android platforms.
- Localization system with two implemented languages and support for expansion: PT-BR, EN.
- Audio and graphical quality control menus.
- Use of post-processing for scene and area atmosphere.
- Customized particles for in-game effects.
- State machines for various character and creature states.

### Gameplay Features

- Combat against monsters with simple behavior scripts and NAVMESH navigation.
- Scripted final battle with unique mechanics.
- Power-ups.
- Map and minimap systems.
- Random drop system.
- Cutscenes implemented using Unity Timeline.

## Code Refinement

With the objectives and features now achieved and implemented, the code is undergoing a review to enhance human readability, modularity, scalability, and to reduce code coupling. This aims to facilitate the reuse of code components in other projects, streamline the code optimization process, and accommodate future expansion.

This process applies principles of Clean Code and SOLID in a flexible manner, targeting critical areas where the need is most apparent.

The first area being modified is the `PlayerController`, which has exhibited high coupling and an accumulation of functionalities, making it the largest script in the project. The applied design involves the use of events and delegates to partition each aggregated functionality within the script into independent scripts. These scripts will then utilize the `PlayerController` as an interface for actions, interactions, and reactions.

#### PlayerController Refactoring

The previous `PlayerController` script has been restructured into two separate scripts: `PlayerController` and `PlayerAvatar`. This division of functionality allows for clearer communication and organization within the game architecture.

- `PlayerController`: This script now handles player character interactions related to game rules and player input domains. It serves as the communication hub for interactions related to these aspects.

- `PlayerAvatar`: Responsible for managing the player avatar in the world and its physics interactions. it takes care of animation, rendering, physics and sounds.It serves as the communication hub for interactions related to these aspects.

These scripts are assigned to different game objects. The communication between them is bidirectional, ensuring that interactions are properly synchronized.

To achieve this refactor, the functionalities of the old `PlayerController` have been distributed among several new scripts:

- `PlayerPhysics`: Manages player physics interactions, such as movements and collisions.

- `PlayerCombat`: Handles combat-related mechanics, such as attacks, defenses, and damage calculations.

- `PlayerLifeCycle`: Manages the player's lifecycle events, including death, respawning, health gain and loss.

- `PlayerPowerUps`: Deals with the application and effects of power-ups collected by the player.

Alongside these new scripts, existing scripts that previously communicated with the old `PlayerController` have been adapted to work seamlessly with the new structure. This adaptation ensures that interactions between the revamped `PlayerController` and `PlayerAvatar` remain cohesive and effective. By distributing and redefining the responsibilities, the overall game architecture has been improved, leading to more organized and maintainable code.

## Third Party Assets

### Assets de Terceiros

Authors of the Third Party Assets used in this project:

- YouFulca
- CatBorg Studio
- Dungeon Mason
- Just Labbing
- Polygonal Stuff
- Kenny
- Leohpaz
- Imphenzia


Feel free to reach out for clarifications or inquiries.
