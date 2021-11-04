# Tactical Rescue

Dans le cadre du module CO6SFPA0 de programmation avancée de première année à l’ENSC, nous avons été amenés à concevoir un jeu de type Roguelike comprenant les caractéristiques du style : 
- l’exploration par le personnage-joueur d’un univers de jeu qu’il découvre au fur et à mesure, dans le but d’atteindre un objectif donné ;
- un environnement représenté en deux dimensions par des symboles textuels ;
- un système de jeu au tour par tour ;
- la gestion de ressources pour survivre (temps, nourriture, etc) ;
- un système de permadeath (la mort du personnage-joueur est définitive).. Ce programme devait être structuré par des classes et répondre aux normes de bases de la programmation orientée objet. 

Dans ce rapport, nous présenterons en premier lieu une notice d’utilisation du programme, puis nous détaillerons les choix techniques et présenterons nos méthodes de gestion de projet. Enfin nous discuterons des perspectives et des améliorations envisageables.

Il est à noter que le projet se base sur un [tutoriel](https://roguesharp.wordpress.com/2016/02/20/roguesharp-v3-tutorial-introduction-and-goals/)



## Principe du jeu

Tactical Rescue est un jeu d'exploration d'un univers post-invasion extraterrestre. En effet, la Terre a été envahie par des extraterrestres et une petite minorité d'humains s'organise pour se rebeller. Ainsi, le commandant G.I. Joe est envoyé pour récolter des informations et récupérer un CODEX en zone ennemi où il devra survivre à l'opposition toujours plus hostile.

Dès le lancement du jeu, un rappel de son objectif est donné au commandant G.I. Joe que vous incarnerez. 
Vous vous retrouvez donc sur le terrain, armé d'un pistolet et de votre sac à dos. Sur place, vous trouverez de quoi vous régénérer, améliorer votre précision, ou encore des armes. Mais faites attention, dès que les ennemis vous verront, ils n'attendront pas pour vous tirer dessus et partir à votre poursuite.

![image](https://user-images.githubusercontent.com/75249990/117373531-af71d880-aecb-11eb-8219-7ce848151739.png)
Des ennemis entourent le commandant.

![image](https://user-images.githubusercontent.com/75249990/117373338-53a74f80-aecb-11eb-9c43-6663b9711f1e.png)
Des obstacles pour s'offrir une protection supplémentaire.

![image](https://user-images.githubusercontent.com/75249990/117373458-8a7d6580-aecb-11eb-9439-ef4879a96ada.png)
Une mitraillette, une pomme et une cible. De quoi améliorer les dégâts, la santé et la précision du commandant G.I. Joe !!!

![codex](https://user-images.githubusercontent.com/75249990/117373602-d92aff80-aecb-11eb-8b5a-9686b3de2c3b.png)
Le CODEX recueille un large nombre d'informations concernant les extraterrestre. Une source d'information vitale pour combattre la menace extraterrestre.

### Affichage

![command](https://user-images.githubusercontent.com/75249990/117329225-b7625600-ae94-11eb-8c44-51889c757a3b.png)

Les commandes utilisés vous sont rappelées en haut à gauche de l'écran à tout moment et vous permettront d'achever votre mission.

![inventory](https://user-images.githubusercontent.com/75249990/117329383-e4166d80-ae94-11eb-9b6c-4a6ac208d370.png)

Un récapitulatif de votre inventaire est également disponible.

![stats](https://user-images.githubusercontent.com/75249990/117329456-f1cbf300-ae94-11eb-9764-278f7695f076.png)

De même, un récapitulatif de vos statistiques est disponible en haut à droite de l'écran.

![image](https://user-images.githubusercontent.com/75249990/117373323-4f7b3200-aecb-11eb-9036-a911071e3d34.png)

Enfin, une partie de l'écran est dédiée à l'affichage des différents messages détaillant les actions en cours.

## Choix techniques

Nous avons développé le jeu en C# sous Visual Studio. Ainsi, nous avons utilisé une console d'application (.NET FrameWork) classique associée avec les libraires RogueSharp, RLNET et OpenTK. Ces librairies ont permis de faciliter la gestion des déplacements des ennemis et, plus globalement, de l'affichage du jeu. 


![image](https://user-images.githubusercontent.com/75249990/117373741-24dda900-aecc-11eb-836e-3e4cc7e54ae1.png)


Nous avons décidé d'opter pour une programmation orientée objet. Ainsi, la classe Game est la classe principale au sens où elle permet la création du jeu. Ensuite, il a été question d'optimiser la fusion entre les classes que nous avions crées et pensées avec celles présentes dans le tutoriel.


Ainsi, les classes les plus importantes sont les classes : DungeonMap, CommandSystem et StandardMoveAndAttack. En effet, ces classes gèrent les mécaniques du jeu. La classe DungeonMap permet la manipulation de tous les éléments visibles sur la map et permet de l'afficher. Quant à elle, la classe, CommandSystem gère le bon fonctionnement des actions du joueur et des ennemis comme le tir à distance ou encore le déplacement des joueurs. Enfin, StandardMoveAndAttack permet de définir le mouvement des ennemis.

Concernant les autres classe, on peut distinguer les énumerations et les interfaces des autres. Effectivement, l'énumération Direction et les interfaces IActor, Ibehavior, IDrawable et IScheduleable sont héritées du tutoriel utilisé et permettent d'établir des traits communs entre les sous-classes associées (mais sans qu'elles soient utilisables en tant que telles). Pour la majorité des classes restantes, il s'agit de classes facilitant une fonctionnalité particulière comme la classe MapGenerator, qui gère la création de la map, ou Item, qui permet de gèrer les différents items, par exemple (et potentiellement leurs classes filles).

### Fonctionnalités

En dehors des fonctionnalités classiques associées à un jeu de type Roguelike, comme les mouvements et l'exploration d'une carte créée, nous avons décidé d'ajouter certains fonctionnalités pour faire de Tactical Rescue un jeu à part entière.

Ainsi, nous avons opté pour un système de tir à distance, que ce soit pour le joueur ou pour les ennemis. Dès qu'un ennemi entre dans le champ de vision du joueur, celui-ci peut attaquer à distance ou, si plusieurs ennemis l'entourent, changer de cible. Le système de tir se base sur une probabilité de toucher l'ennemi calculer en fonction de la distance, de sa protection (s'il est derrière des obstacles) et de la précision du joueur. 
De plus, le joueur peut récupérer des items comme de la nourriture, pour se soigner, des armes, pour faire plus de dégats, ou encore des bonus, pour améliorer sa précision.


## Gestion du projet

### Planning du projet

![calendrier](https://user-images.githubusercontent.com/75249990/117348305-56de1380-aeaa-11eb-8746-34491ee1cd68.png)

Concernant la gestion du projet, il a été d'abord important de définir le thème du jeu ainsi que les fonctionnalités que nous voulions implémenter. Dès lors, nous avons conçu la struture du projet en essayant de penser au maximum à comment optimiser la création des classes pour ne pas surcharger le code.

De plus, le tutoriel offre une base de structuration du code solide sur laquelle nous avons décidé de nous appuyer. Ainsi, il a été important de reprendre chaque étape de manière détaillé pour comprendre les mécanismes implémentés par celui-ci.

Après cela, nous avons mis en commun nos propres classes avec les classes du tutoriel pour commencer à ajouter les fonctionnalités que nous souhaitions. Pour ce faire, nous avons décidé de nous séparer le travail en deux grandes parties (chacun ayant la responsabilité d'une branche du projet). D'un côté, la gestion et la modification des éléments à afficher. De l'autre côté, l'implémentation du système de tir à distance. Pour cela, nous avons utilisé la fonctionnalité de branche qu'offre GitHub, qui nous aussi permis de fusionner ces deux branches en une seule lorsque nous avions fini les modifications associées à chaque branche.

Concernant le côté technique, même si l'utilisation de GitHub est assez récente et c'est avérée laborieuse en début de projet, elle nous semble maintenant indispensable à un tel projet pour pouvoir travailler efficacement chacun de son côté.

Par ailleurs, nous avons réalisé des tests au fur et à mesure que des fonctionnalités ont été ajouté au jeu pour s'assurer de leur bon fonctionnement.


## Bilan

Ce projet nous a encore plus permis de nous mettre à l'aise avec les outils informatique tels que Visual Studio et GitHub. Nous avons pu nous apercevoir du potentiel de puissance de ces logiciels. Cela a été une fois de plus une bonne expérience qui nous a aidé à consolider nos connaissances et compétences.
