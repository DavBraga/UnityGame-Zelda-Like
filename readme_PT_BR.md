# [Unity Game] Dungeon Fall

Jogo no itch.io: [Link para o Dungeon Fall no itch.io](https://bragadavi.itch.io/dungeon-fall)

Versão do código do jogo aqui no Git é 0.5.0

## Sobre

Dungeon Fall é um jogo desenvolvido na Unity Engine como projeto final do curso [Formação Unity 3D Game Developer](https://www.dio.me/curso-unity-3d) oferecido pela DIO.me.

O projeto já foi entregue e certificado atendendo aos requisitos de conclusão do curso. [Confira o certificado aqui.](https://www.dio.me/certificate/94C79951/share).

Este jogo presta homenagem aos clássicos como Zelda, Brave Fencer Musashiden e Alundra, com uma câmera top-down que busca trazer elementos de exploração, combate, power-ups, cinemáticas, tesouros valiosos e segredos escondidos em uma masmorra repleta de mistérios.

### Ações do Jogador

- Movimentar-se,
- Atacar inimigos,
- Pular,
- Utilizar consumíveis,
- Interagir com objetos próximos,
- Utilizar a ferramenta bomba para destruir obstáculos.

## Desenvolvimento Adicional

Apesar de ser um jogo para demonstrar capacidades técnicas como desenvolvedor de jogos, programador e Game Designer, busquei entregar uma experiência completa com início, meio e fim. Para isso, novas funcionalidades foram adicionadas com o tempo. Diversas características foram incluídas para aprimorar o jogo. A seguir está uma lista do que está presente neste projeto.

### Funcionalidades Técnicas

- Uso da CineMachine para câmera top-down.
- Movimento do personagem com física personalizada.
- Suporte a gamepads e controles virtuais.
- Portabilidade para PC Windows, Browser (WebGL) e Android.
- Sistema de localização com duas línguas implementadas e suporte para expansão: PT-BR, EN.
- Menus de controle de áudio e qualidade gráfica.
- Utilização de pós-processamento para a atmosfera das cenas e áreas.
- Partículas personalizadas para o jogo.
- Máquinas de estados para os diversos estados dos personagens e criaturas do jogo.

### Funcionalidades do Jogo

- Combate contra monstros com script de comportamento simples e navegação via NAVMESH.
- Batalha final scriptada com mecânicas únicas.
- Power-ups.
- Sistema de mapa e minimapa.
- Sistema de drops aleatórios.
- Cutscenes implementadas usando a Unity Timeline.

## Melhorias no Código

Agora que os objetivos e funcionalidades foram alcançados e implementados, o código passa por uma revisão para melhorar a legibilidade humana, a modularidade, a escalabilidade e reduzir o acoplamento do código, facilitando o reuso de partes em outros projetos, bem como a otimização e expansão futura.

Esse processo é realizado aplicando princípios de Clean Code e SOLID de maneira flexível, focando em pontos críticos onde a necessidade é mais evidente.

A primeira área a ser modificada é o `PlayerController`, onde há um alto acoplamento e acumulação de funcionalidades, tornando-o o maior script do projeto. O design empregado envolve o uso de eventos e delegates para separar cada funcionalidade agregada no script em scripts independentes, que utilizarão o `PlayerController` como interface para ações, interações e reações.

Estou à disposição para qualquer esclarecimento ou dúvida.

### Assets de Terceiros

O jogo fez uso de assets gráficos e de áudio disponibilizados gratuitamente de terceiros. Uma lista de seus autores:

- YouFulca
- CatBorg Studio
- Dungeon Mason
- Just Labbing
- Polygonal Stuff
- Kenny
- Leohpaz
- Imphenzia
