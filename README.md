# Dungeon Fall

Game on itch.io: [Dungeon Fall on itch.io](https://bragadavi.itch.io/dungeon-fall)

Game Code Version on Git: 0.5.0

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

Feel free to reach out for clarifications or inquiries.

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
